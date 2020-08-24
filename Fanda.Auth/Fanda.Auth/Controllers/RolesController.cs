using Fanda.Core;
using Fanda.Core.Base;
using Fanda.Core.Extensions;
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

namespace Fanda.Auth.Controllers
{
    public class RolesController : BaseController
    {
        private const string ModuleName = "Role";
        private readonly IRoleRepository repository;

        public RolesController(IRoleRepository repository)
        {
            this.repository = repository;
        }

        // roles/all/5
        [HttpGet("all/{tenantId}")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(DataResponse<List<RoleListDto>>), (int)HttpStatusCode.OK)]
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

                var response = await repository.GetData(tenantId, query);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex, ModuleName);
            }
        }

        // roles/5
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(DataResponse<RoleDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById([Required, FromRoute] Guid id, [FromQuery] bool include)
        {
            try
            {
                var role = await repository.GetByIdAsync(id, include);
                if (role == null)
                {
                    return NotFound(MessageResponse.Failure($"{ModuleName} id '{id}' not found"));
                }
                return Ok(DataResponse<RoleDto>.Succeeded(role));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex, ModuleName);
            }
        }

        [HttpPost("{tenantId}")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(DataResponse<RoleDto>), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create(Guid tenantId, RoleDto model)
        {
            try
            {
                #region Validation

                var validationResult = await repository.ValidateAsync(tenantId, model);

                #endregion Validation

                if (validationResult.IsValid)
                {
                    var role = await repository.CreateAsync(tenantId, model);
                    return CreatedAtAction(nameof(GetById), new { id = role.Id },
                        DataResponse<RoleDto>.Succeeded(role));
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

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Update(Guid id, RoleDto model)
        {
            try
            {
                if (id != model.Id)
                {
                    return BadRequest(MessageResponse.Failure($"{ModuleName} Id mismatch"));
                }

                #region Validation

                var validationResult = await repository.ValidateAsync(id, model);

                #endregion Validation

                if (validationResult.IsValid)
                {
                    await repository.UpdateAsync(id, model);
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
                var success = await repository.DeleteAsync(id);
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
                bool success = await repository.ChangeStatusAsync(new ActiveStatus
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
                bool success = await repository.ExistsAsync(new TenantKeyData
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

        // [HttpPost("add-privilege")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        // public async Task<IActionResult> AddPrivilege([FromBody] PrivilegeDto model)
        // {
        //     try
        //     {
        //         bool success = await _repository.AddPrivilege(model);
        //         if (success)
        //         {
        //             return Ok(DataResponse.Succeeded("Mapped privilege successfully"));
        //         }
        //         return NotFound(DataResponse.Failure($"Privilege not found"));
        //     }
        //     catch (Exception ex)
        //     {
        //         if (ex is BadRequestException || ex is ArgumentNullException || ex is ArgumentException)
        //         {
        //             return BadRequest(DataResponse.Failure("Invalid privilege id"));
        //         }
        //         else if (ex is NotFoundException)
        //         {
        //             return NotFound(DataResponse.Failure("Privilege not found"));
        //         }
        //         else
        //         {
        //             return InternalServerError(DataResponse.Failure(ex.Message));
        //         }
        //     }
        // }

        // [HttpPost("remove-privilege")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        // public async Task<IActionResult> RemovePrivilege([FromBody] PrivilegeDto model)
        // {
        //     try
        //     {
        //         bool success = await _repository.RemovePrivilege(model);
        //         if (success)
        //         {
        //             return Ok(DataResponse.Succeeded("Unmapped privilege successfully"));
        //         }
        //         return NotFound(DataResponse.Failure($"Privilege not found"));
        //     }
        //     catch (Exception ex)
        //     {
        //         if (ex is BadRequestException || ex is ArgumentNullException || ex is ArgumentException)
        //         {
        //             return BadRequest(DataResponse.Failure("Invalid privilege id"));
        //         }
        //         else if (ex is NotFoundException)
        //         {
        //             return NotFound(DataResponse.Failure("Privilege not found"));
        //         }
        //         else
        //         {
        //             return InternalServerError(DataResponse.Failure(ex.Message));
        //         }
        //     }
        // }
    }
}
