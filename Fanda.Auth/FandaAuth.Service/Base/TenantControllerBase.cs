using Fanda.Core;
using Fanda.Core.Base;
using Fanda.Core.Extensions;
using FandaAuth.Service.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace FandaAuth.Service.Base
{
    public abstract class TenantControllerBase<TRepository, TModel, TListModel> :
        RootControllerBase<TRepository, TModel, TListModel>
        where TRepository : ITenantRepository<TModel, TListModel>
        where TModel : BaseDto
        where TListModel : BaseListDto
    {
        private readonly string _moduleName;
        private readonly TRepository _repository;

        public TenantControllerBase(TRepository repository, string moduleName)
            : base(repository, moduleName)
        {
            _repository = repository;
            _moduleName = moduleName;
        }

        // roles/all/5
        [HttpGet("all/{tenantId}")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.OK)]  // typeof(DataResponse<List<TListModel>>)
        public async Task<IActionResult> GetAll([Required] Guid tenantId)
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

                var response = await _repository.GetData(tenantId, query);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex, _moduleName);
            }
        }

        [HttpPost("{tenantId}")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.Created)]     // typeof(DataResponse<TModel>)
        public async Task<IActionResult> Create(Guid tenantId, TModel model)
        {
            try
            {
                #region Validation

                var validationResult = await _repository.ValidateAsync(tenantId, model);

                #endregion Validation

                if (validationResult.IsValid)
                {
                    var modelCreated = await _repository.CreateAsync(tenantId, model);
                    return CreatedAtAction(nameof(GetById), new { id = modelCreated.Id },
                        DataResponse<TModel>.Succeeded(modelCreated));
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
                    return BadRequest(MessageResponse.Failure($"{_moduleName} Id mismatch"));
                }

                #region Validation

                var validationResult = await _repository.ValidateAsync(id, model);

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

        //[HttpDelete("{id}")]
        //[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        //[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        //[ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        //public async Task<IActionResult> Delete(Guid id)
        //{
        //    try
        //    {
        //        if (id == null || id == Guid.Empty)
        //        {
        //            return BadRequest(MessageResponse.Failure($"{ModuleName} id is missing"));
        //        }
        //        var success = await repository.DeleteAsync(id);
        //        if (success)
        //        {
        //            return NoContent();
        //        }
        //        else
        //        {
        //            return NotFound(MessageResponse.Failure($"{ModuleName} not found"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex, ModuleName);
        //    }
        //}

        //[HttpPatch("active/{id}")]
        //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
        //[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        //[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        //[ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        //public async Task<IActionResult> Active([Required, FromRoute] Guid id, [Required, FromQuery] bool active)
        //{
        //    try
        //    {
        //        bool success = await repository.ChangeStatusAsync(new ActiveStatus
        //        {
        //            Id = id,
        //            Active = active
        //        });
        //        if (success)
        //        {
        //            return Ok(MessageResponse.Succeeded("Status changed successfully"));
        //        }
        //        return NotFound(MessageResponse.Failure($"{ModuleName} id '{id}' not found"));
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex, ModuleName);
        //    }
        //}

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
                bool success = await _repository.ExistsAsync(new TenantKeyData
                {
                    Id = id,
                    Field = exists.Field,
                    Value = exists.Value,
                    TenantId = exists.ParentId
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
