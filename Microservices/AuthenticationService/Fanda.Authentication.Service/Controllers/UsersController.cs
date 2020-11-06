using Fanda.Authentication.Repository;
using Fanda.Authentication.Repository.Dto;
using Fanda.Core;
using Fanda.Core.Base;
using Fanda.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Fanda.Authentication.Service.Controllers
{
    public class UsersController : BaseController
    {
        private const string ModuleName = "User";
        private readonly IUserRepository _repository;

        public UsersController(IUserRepository repository)
        {
            _repository = repository;
        }

        // users?tenantId=5
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(DataResponse<List<UserListDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromQuery][Required] Guid tenantId)
        {
            try
            {
                var queryString = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var query = new Query(queryString["pageIndex"], queryString["pageSize"])
                {
                    Filter = queryString["filter"],
                    FilterArgs = queryString["filterArgs"]?.Split(','),
                    //Search = queryString["search"],
                    Sort = queryString["sort"]
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
        public async Task<IActionResult> GetById([Required][FromRoute] Guid id /*, [FromQuery] bool include*/)
        {
            try
            {
                var user = await _repository.GetByIdAsync(id /*, include*/);
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
                var userDto = await _repository.CreateAsync(tenantId, model);
                return CreatedAtAction(nameof(GetById), new { id = model.Id },
                    DataResponse<UserDto>.Succeeded(userDto));
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
        public async Task<IActionResult> Update(Guid id, UserDto model)
        {
            try
            {
                await _repository.UpdateAsync(id, model);
                return NoContent();
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
                if (id == Guid.Empty)
                {
                    return BadRequest(MessageResponse.Failure($"{ModuleName} id is required"));
                }

                bool success = await _repository.DeleteAsync(id);
                if (success)
                {
                    return NoContent();
                }

                return NotFound(MessageResponse.Failure($"{ModuleName} not found"));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex, ModuleName);
            }
        }

        [HttpPatch("activate/{id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Active([Required][FromRoute] Guid id, [Required] Activate activate)
        {
            try
            {
                bool success = await _repository.ActivateAsync(id, activate.Active);
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

        //[HttpGet("exists/{id}")]
        //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
        //[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        //[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        //[ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        //public async Task<IActionResult> Exists([Required, FromRoute] Guid id, ExistsDto exists)
        //{
        //    try
        //    {
        //        bool success = await _repository.ExistsAsync(new UserKeyData
        //        {
        //            Id = id,
        //            Field = exists.Field,
        //            Value = exists.Value,
        //            TenantId = exists.ParentId
        //        });
        //        if (success)
        //        {
        //            return Ok(MessageResponse.Succeeded("Found"));
        //        }
        //        return NotFound(MessageResponse.Failure("Not found"));
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex, ModuleName);
        //    }
        //}
    }
}