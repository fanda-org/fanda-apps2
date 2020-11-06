using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Fanda.Accounting.Repository;
using Fanda.Accounting.Repository.Dto;
using Fanda.Core;
using Fanda.Core.Base;
using Fanda.Core.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Fanda.Accounting.Service.Controllers
{
    public class OrgUsersController : BaseController
    {
        private const string _moduleName = "Users";
        private readonly IOrgUserRepository _repository;

        public OrgUsersController(IOrgUserRepository repository)
        {
            _repository = repository;
        }

        // organizations?superId=5
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.OK)] // typeof(DataResponse<List<TListModel>>)
        public async Task<IActionResult> GetAll([FromQuery] Guid superId)
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

                var response = await _repository.GetAll(superId, query);
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
        public async Task<IActionResult> GetById([Required] [FromRoute] Guid id)
        {
            try
            {
                var dto = await _repository.GetByIdAsync(id);
                if (dto == null)
                {
                    return NotFound(MessageResponse.Failure($"{_moduleName} id '{id}' not found"));
                }

                return Ok(DataResponse<OrgUserDto>.Succeeded(dto));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex, _moduleName);
            }
        }

        [HttpPost("{superId}")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.Created)] // typeof(DataResponse<TModel>)
        public async Task<IActionResult> Create(Guid superId, OrgUserDto model)
        {
            try
            {
                var dto = await _repository.CreateAsync(superId, model);
                return CreatedAtAction(nameof(GetById), new {id = dto.Id},
                    DataResponse<OrgUserDto>.Succeeded(dto));
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
        public async Task<IActionResult> Update(Guid id, OrgUserDto model)
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
                if (id == Guid.Empty)
                {
                    return BadRequest(MessageResponse.Failure($"{_moduleName} id is missing"));
                }

                bool success = await _repository.DeleteAsync(id);
                if (success)
                {
                    return NoContent();
                }

                return NotFound(MessageResponse.Failure($"{_moduleName} not found"));
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
        public async Task<IActionResult> Active([Required] [FromRoute] Guid id,
            [Required] [FromQuery] Activate activate)
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
    }
}