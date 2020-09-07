using Fanda.Core;
using Fanda.Core.Base;
using FandaAuth.Service;
using FandaAuth.Service.Dto;
using FandaAuth.Service.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Fanda.Authentication.Controllers
{
    public class UsersController : BaseController
    {
        private const string ModuleName = "User";
        private readonly IUserRepository _repository;

        public UsersController(IUserRepository repository)
        {
            _repository = repository;
        }

        // users/all/5
        [HttpGet("all/{tenantId}")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(DataResponse<List<UserListDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([Required] Guid tenantId)
        {
            try
            {
                NameValueCollection queryString = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var query = new Query(queryString["page"], queryString["pageSize"])
                {
                    Filter = queryString["filter"],
                    FilterArgs = queryString["filterArgs"]?.Split(','),
                    //Search = queryString["search"],
                    Sort = queryString["sort"],
                };

                var response = await _repository.GetAll(tenantId, query);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex, ModuleName);
            }
        }

        // users/5
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(DataResponse<UserDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById([Required, FromRoute] Guid id/*, [FromQuery] bool include*/)
        {
            try
            {
                var user = await _repository.GetByIdAsync(id/*, include*/);
                if (user == null)
                {
                    return NotFound(MessageResponse.Failure($"{ModuleName} id '{id}' not found"));
                }
                return Ok(DataResponse<UserDto>.Succeeded(user));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex, ModuleName);
            }
        }

        [HttpPost("{tenantId}")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(DataResponse<UserDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create(Guid tenantId, UserDto model)
        {
            try
            {
                #region Validation

                var validationResult = await _repository.ValidateAsync(model, tenantId);

                #endregion Validation

                if (validationResult.IsValid)
                {
                    var userDto = await _repository.CreateAsync(model, tenantId);
                    return CreatedAtAction(nameof(GetById), new { id = model.Id },
                        DataResponse<UserDto>.Succeeded(userDto));
                }
                else
                {
                    return BadRequest(MessageResponse.Failure(validationResult));
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex, ModuleName);
            }
        }

        [HttpPut("{tenantId}")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Update(Guid tenantId, UserDto model)
        {
            try
            {
                //if (id != model.Id)
                //{
                //    return BadRequest(MessageResponse.Failure($"{ModuleName} Id mismatch"));
                //}

                #region Validation

                var validationResult = await _repository.ValidateAsync(model, tenantId);

                #endregion Validation

                if (validationResult.IsValid)
                {
                    await _repository.UpdateAsync(model, tenantId);
                    return NoContent();
                }
                else
                {
                    return BadRequest(MessageResponse.Failure(validationResult));
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex, ModuleName);
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
                    return BadRequest(MessageResponse.Failure($"{ModuleName} id is missing"));
                }
                var success = await _repository.DeleteAsync(id);
                if (success)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound(MessageResponse.Failure($"{ModuleName} not found"));
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex, ModuleName);
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
                return NotFound(MessageResponse.Failure($"{ModuleName} id '{id}' not found"));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex, ModuleName);
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
                bool success = await _repository.ExistsAsync(new UserKeyData
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
                return ExceptionResult(ex, ModuleName);
            }
        }
    }
}
