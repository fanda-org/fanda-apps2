using Fanda.Core;
using Fanda.Core.Base;
using Fanda.Core.Extensions;
using FandaAuth.Service;
using FandaAuth.Service.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;

namespace Fanda.Auth.Controllers
{
    public class RolesController : BaseController
    {
        private readonly IRoleRepository repository;

        public RolesController(IRoleRepository repository)
        {
            this.repository = repository;
        }

        // roles/all/5
        [HttpGet("all/{tenantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(Guid tenantId)
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
                return StatusCode(StatusCodes.Status500InternalServerError,
                    MessageResponse.Failure(ex.Message));
            }
        }

        // roles/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var role = await repository.GetByIdAsync(id);
                return Ok(DataResponse<RoleDto>.Succeeded(role));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    MessageResponse.Failure(ex.Message));
            }
        }

        [HttpPost("{tenantId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(Guid tenantId, RoleDto model)
        {
            try
            {
                var roleDto = await repository.CreateAsync(tenantId, model);
                return CreatedAtAction(nameof(GetById), new { id = roleDto.Id },
                    DataResponse<RoleDto>.Succeeded(roleDto));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    MessageResponse.Failure(ex.Message));
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, RoleDto model)
        {
            try
            {
                if (id != model.Id)
                {
                    return BadRequest(MessageResponse.Failure("Role Id mismatch"));
                }
                var role = await repository.GetByIdAsync(id);
                if (role == null)
                {
                    return NotFound(MessageResponse.Failure("Role not found"));
                }
                // save
                await repository.UpdateAsync(id, model);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    DataResponse<string>.Failure(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var success = await repository.DeleteAsync(id);
                if (success)
                {
                    return NoContent();
                }
                return NotFound(MessageResponse.Failure("Role not found"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    DataResponse<string>.Failure(ex.Message));
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
