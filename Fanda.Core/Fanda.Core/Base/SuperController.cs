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
    public abstract class SuperController<TRepository, TEntity, TModel, TListModel> : BaseController
        where TRepository : ISuperRepository<TEntity, TModel, TListModel>
        where TModel : BaseDto
        where TListModel : class
    {
        private readonly TRepository _repository;
        private readonly string _moduleName;

        public SuperController(TRepository repository)
        {
            _repository = repository;
            _moduleName = typeof(TEntity).Name;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.OK)] // typeof(DataResponse<List<TListModel>>)
        public async Task<IActionResult> GetAll()
        {
            try
            {
                NameValueCollection queryString = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var query = new Query(queryString["pageIndex"], queryString["pageSize"])
                {
                    Filter = queryString["filter"],
                    FilterArgs = queryString["filterArgs"]?.Split(','),
                    //Search = queryString["search"],
                    Sort = queryString["sort"],
                };

                var response = await _repository.GetAll(Guid.Empty, query);
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
        public async Task<IActionResult> GetById([Required, FromRoute] Guid id)
        {
            try
            {
                var app = await _repository.GetByIdAsync(id);
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
                var app = await _repository.CreateAsync(model);
                return CreatedAtAction(nameof(GetById), new { id = app.Id },
                    DataResponse<TModel>.Succeeded(app));
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
                await _repository.UpdateAsync(id, model);
                return NoContent();
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

        [HttpPatch("activate/{id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Active([Required, FromRoute] Guid id, [Required] Activate activate)
        {
            try
            {
                bool success = await _repository.ActivateAsync(id, activate.Active);
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

        //[HttpGet("exists")]
        //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
        //[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        //[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        //[ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        //public async Task<IActionResult> Exists(ExistsDto exists)
        //{
        //    try
        //    {
        //        bool success = await _repository.ExistsAsync(new TKeyData
        //        {
        //            Id = id,
        //            Field = exists.Field,
        //            Value = exists.Value
        //        });
        //        if (success)
        //        {
        //            return Ok(MessageResponse.Succeeded("Found"));
        //        }
        //        return NotFound(MessageResponse.Failure("Not found"));
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex, _moduleName);
        //    }
        //}
    }
}