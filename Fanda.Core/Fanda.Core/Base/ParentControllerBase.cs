using Fanda.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Fanda.Core.Base
{
    public abstract class ParentControllerBase<TRepository, TModel, TListModel> :
        RootControllerBase<TRepository, TModel, TListModel>
        where TRepository : IParentRepositoryBase<TModel, TListModel>
        where TModel : BaseDto
        where TListModel : BaseListDto
    {
        private readonly string _moduleName;
        private readonly TRepository _repository;

        public ParentControllerBase(TRepository repository, string moduleName)
            : base(repository, moduleName)
        {
            _repository = repository;
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
