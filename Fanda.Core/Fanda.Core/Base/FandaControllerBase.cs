using Fanda.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Fanda.Core.Base
{
    public class FandaControllerBase<TRepository, TModel, TListModel> : RootControllerBase
        where TRepository : IParentRepository<TModel>, IListRepository<TListModel>
        where TModel : BaseDto
        where TListModel : BaseListDto
    {
        private readonly string _moduleName;
        private readonly TRepository _repository;

        public FandaControllerBase(TRepository repository, string moduleName)
        {
            this._repository = repository;
            _moduleName = moduleName;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.OK)] // typeof(DataResponse<List<TListModel>>)
        public async Task<IActionResult> GetAll()
        {
            try
            {
                NameValueCollection queryString = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var query = new Query(queryString["page"], queryString["pageSize"])
                {
                    Filter = queryString["filter"],
                    FilterArgs = queryString["filterArgs"]?.Split(','),
                    Search = queryString["search"],
                    Sort = queryString["sort"],
                };

                var response = await _repository.GetData(Guid.Empty, query);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex, _moduleName);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.OK)] // typeof(DataResponse<TModel>)
        public async Task<IActionResult> GetById([Required, FromRoute] Guid id/*, [FromQuery] bool include*/)
        {
            try
            {
                var app = await _repository.GetByIdAsync(id/*, include*/);
                if (app == null)
                {
                    return NotFound(MessageResponse.Failure($"{_moduleName} id '{id}' not found"));
                }
                return Ok(DataResponse<TModel>.Succeeded(app));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex, _moduleName);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.Created)]    // typeof(DataResponse<TModel>)
        public async Task<IActionResult> Create(TModel model)
        {
            try
            {
                #region Validation

                var validationResult = await _repository.ValidateAsync(model);

                #endregion Validation

                if (validationResult.IsValid)
                {
                    var app = await _repository.CreateAsync(model);
                    return CreatedAtAction(nameof(GetById), new { id = app.Id },
                        DataResponse<TModel>.Succeeded(app));
                }
                else
                {
                    return BadRequest(MessageResponse.Failure(validationResult));
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex, _moduleName);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Update(Guid id, TModel model)
        {
            try
            {
                if (id != model.Id)
                {
                    return BadRequest(MessageResponse.Failure($"{_moduleName} id mismatch"));
                }

                #region Validation

                var validationResult = await _repository.ValidateAsync(model);

                #endregion Validation

                if (validationResult.IsValid)
                {
                    await _repository.UpdateAsync(id, model);
                    return NoContent();
                }
                else
                {
                    return BadRequest(MessageResponse.Failure(validationResult));
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex, _moduleName);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                if (id == null || id == Guid.Empty)
                {
                    return BadRequest(MessageResponse.Failure($"{_moduleName} id is missing"));
                }
                var success = await _repository.DeleteAsync(id);
                if (success)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound(MessageResponse.Failure($"{_moduleName} not found"));
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex, _moduleName);
            }
        }

        [HttpPatch("active/{id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Active([Required, FromRoute] Guid id, [Required, FromQuery] bool active)
        {
            try
            {
                bool success = await _repository.ChangeStatusAsync(new ActiveStatus
                {
                    Id = id,
                    Active = active
                });
                if (success)
                {
                    return Ok(MessageResponse.Succeeded("Status changed successfully"));
                }
                return NotFound(MessageResponse.Failure($"{_moduleName} id '{id}' not found"));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex, _moduleName);
            }
        }

        [HttpGet("exists/{id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Exists([Required, FromRoute] Guid id, ExistsDto exists)
        {
            try
            {
                bool success = await _repository.ExistsAsync(new KeyData
                {
                    Id = id,
                    Field = exists.Field,
                    Value = exists.Value
                });
                if (success)
                {
                    return Ok(MessageResponse.Succeeded("Found"));
                }
                return NotFound(MessageResponse.Failure("Not found"));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex, _moduleName);
            }
        }
    }
}
