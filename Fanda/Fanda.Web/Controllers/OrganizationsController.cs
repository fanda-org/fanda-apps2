using Fanda.Core;
using Fanda.Core.Base;
using Fanda.Core.Extensions;
using Fanda.Domain;
using Fanda.Service;
using Fanda.Service.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Fanda.Web.Controllers
{
    public class OrganizationsController :
        SubController<IOrganizationRepository, Organization, OrganizationDto, OrgYearListDto>
    {
        //private const string ModuleName = "Organization";
        //private readonly IOrganizationRepository repository;

        public OrganizationsController(IOrganizationRepository repository) : base(repository)
        {
            //this.repository = repository;
        }

        //[HttpGet("all/{userId}")]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        //public async Task<IActionResult> GetAll([Required] Guid userId)
        //{
        //    try
        //    {
        //        NameValueCollection queryString = HttpUtility.ParseQueryString(Request.QueryString.Value);
        //        var query = new Query(queryString["page"], queryString["pageSize"])
        //        {
        //            Filter = queryString["filter"],
        //            FilterArgs = queryString["filterArgs"]?.Split(','),
        //            // Search = queryString["search"],
        //            Sort = queryString["sort"]
        //        };

        //        var response = await repository.GetAll(userId, query);
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex, ModuleName);
        //    }
        //}

        //[HttpGet("{id}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> GetById([Required, FromRoute] Guid id)
        //{
        //    try
        //    {
        //        var org = await repository.GetByIdAsync(id);
        //        if (org != null)
        //        {
        //            return Ok(DataResponse<OrganizationDto>.Succeeded(org));
        //        }
        //        return NotFound(MessageResponse.Failure($"Org id '{id}' not found"));
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex, ModuleName);
        //    }
        //}

        ////[HttpGet("children/{id}")]
        ////[ProducesResponseType(StatusCodes.Status200OK)]
        ////[ProducesResponseType(StatusCodes.Status400BadRequest)]
        ////[ProducesResponseType(StatusCodes.Status404NotFound)]
        ////[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        ////public async Task<IActionResult> Get([Required, FromRoute] Guid id)
        ////{
        ////    try
        ////    {
        ////        var orgChildren = await repository.GetChildrenByIdAsync(id);
        ////        if (orgChildren != null)
        ////        {
        ////            return Ok(DataResponse<OrgChildrenDto>.Succeeded(orgChildren));
        ////        }
        ////        return NotFound(MessageResponse.Failure($"Org id '{id}' not found"));
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        if (ex is BadRequestException || ex is ArgumentNullException || ex is ArgumentException)
        ////        {
        ////            return BadRequest(MessageResponse.Failure("Invalid org id"));
        ////        }
        ////        else if (ex is NotFoundException)
        ////        {
        ////            return NotFound(MessageResponse.Failure("Organization not found"));
        ////        }
        ////        else
        ////        {
        ////            //return StatusCode(StatusCodes.Status500InternalServerError, DataResponse.Failure(ex.Message));
        ////            return InternalServerError(MessageResponse.Failure(ex.Message));
        ////        }
        ////    }
        ////}

        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> Create(OrganizationDto model)
        //{
        //    try
        //    {
        //        #region Validation

        //        var validationResult = await repository.ValidateAsync(model);
        //        // foreach (var error in validationResult)
        //        // {
        //        //     ModelState.AddModelError(error.Field, error.Message);
        //        // }

        //        #endregion Validation

        //        if (validationResult.IsValid) //(ModelState.IsValid)
        //        {
        //            var org = await repository.CreateAsync(model);
        //            //return Ok(DataResponse<OrganizationDto>.Succeeded(org));
        //            return CreatedAtAction(nameof(GetById), new { id = model.Id, include = false },
        //                DataResponse<OrganizationDto>.Succeeded(org));
        //        }
        //        else
        //        {
        //            return BadRequest(MessageResponse.Failure(validationResult));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex, ModuleName);
        //    }
        //}

        //[HttpPut("{id}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> Update([Required] Guid id, OrganizationDto model)
        //{
        //    try
        //    {
        //        if (id != model.Id)
        //        {
        //            return BadRequest(MessageResponse.Failure("Org id mismatch"));
        //        }

        //        #region Validation

        //        var validationResult = await repository.ValidateAsync(model);

        //        #endregion Validation

        //        if (validationResult.IsValid)
        //        {
        //            await repository.UpdateAsync(id, model);
        //            return NoContent();
        //        }
        //        else
        //        {
        //            return BadRequest(MessageResponse.Failure(validationResult));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex, ModuleName);
        //    }
        //}

        //[HttpDelete("{id}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> Delete([Required] Guid id)
        //{
        //    try
        //    {
        //        if (id == null || id == Guid.Empty)
        //        {
        //            return BadRequest(MessageResponse.Failure("Id is missing"));
        //        }
        //        var success = await repository.DeleteAsync(id);
        //        if (success)
        //        {
        //            return NoContent();
        //        }
        //        else
        //        {
        //            return NotFound(MessageResponse.Failure("Organization not found"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex, ModuleName);
        //    }
        //}

        //// [HttpPost("{orgId}/add-user/{userId}")]
        //// [ProducesResponseType(StatusCodes.Status200OK)]
        //// [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //// [ProducesResponseType(StatusCodes.Status404NotFound)]
        //// [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //// public async Task<IActionResult> AddUser([Required, FromRoute] Guid orgId, [Required, FromRoute] Guid userId)
        //// {
        ////     try
        ////     {
        ////         bool success = await repository.MapUserAsync(orgId, userId);
        ////         if (success)
        ////         {
        ////             return Ok(DataResponse.Succeeded("User added successfully"));
        ////         }
        ////         return NotFound(DataResponse.Failure($"Organization/User not found"));
        ////     }
        ////     catch (Exception ex)
        ////     {
        ////         if (ex is BadRequestException || ex is ArgumentNullException)
        ////         {
        ////             return BadRequest(DataResponse.Failure("Invalid org/user id"));
        ////         }
        ////         else if (ex is NotFoundException)
        ////         {
        ////             return NotFound(DataResponse.Failure("Organization/User not found"));
        ////         }
        ////         else
        ////         {
        ////             //return StatusCode(StatusCodes.Status500InternalServerError, DataResponse.Failure(ex.Message));
        ////             return InternalServerError(DataResponse.Failure(ex.Message));
        ////         }
        ////     }
        //// }

        //// [HttpPost("{orgId}/remove-user/{userId}")]
        //// [ProducesResponseType(StatusCodes.Status200OK)]
        //// [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //// [ProducesResponseType(StatusCodes.Status404NotFound)]
        //// [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //// public async Task<IActionResult> RemoveUser([Required, FromRoute] Guid orgId, [Required, FromRoute] Guid userId)
        //// {
        ////     try
        ////     {
        ////         bool success = await repository.UnmapUserAsync(orgId, userId);
        ////         if (success)
        ////         {
        ////             return Ok(DataResponse.Succeeded("User removed successfully"));
        ////         }
        ////         return NotFound(DataResponse.Failure($"Organization/User not found"));
        ////     }
        ////     catch (Exception ex)
        ////     {
        ////         if (ex is BadRequestException || ex is ArgumentNullException)
        ////         {
        ////             return BadRequest(DataResponse.Failure("Invalid org/user id"));
        ////         }
        ////         else if (ex is NotFoundException)
        ////         {
        ////             return NotFound(DataResponse.Failure("Organization/User not found"));
        ////         }
        ////         else
        ////         {
        ////             //return StatusCode(StatusCodes.Status500InternalServerError, DataResponse.Failure(ex.Message));
        ////             return InternalServerError(DataResponse.Failure(ex.Message));
        ////         }
        ////     }
        //// }

        //[HttpPatch("active/{id}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        //        return NotFound(MessageResponse.Failure($"Org id '{id}' not found"));
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex, ModuleName);
        //    }
        //}

        //[HttpGet("exists/{id}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> Exists([Required, FromRoute] Guid id, ExistsDto exists)
        //{
        //    try
        //    {
        //        bool success = await repository.ExistsAsync(new KeyData
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
        //        return ExceptionResult(ex, ModuleName);
        //    }
        //}
    }
}
