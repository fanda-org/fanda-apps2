using Fanda.Core;
using Fanda.Core.Base;
using Fanda.Core.Extensions;
using FandaAuth.Service;
using FandaAuth.Service.Dto;
using Microsoft.AspNetCore.Authorization;
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
    public class TenantsController : FandaControllerBase<ITenantRepository, TenantDto, TenantListDto>
    {
        private const string ModuleName = "Tenant";
        //private readonly ITenantRepository repository;

        public TenantsController(ITenantRepository repository) : base(repository, ModuleName)
        {
            //this.repository = repository;
        }

        //[HttpGet]
        //[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        //[ProducesResponseType(typeof(DataResponse<List<TenantListDto>>), (int)HttpStatusCode.OK)]
        //public async Task<IActionResult> GetAll()
        //{
        //    try
        //    {
        //        NameValueCollection queryString = HttpUtility.ParseQueryString(Request.QueryString.Value);
        //        var query = new Query(queryString["page"], queryString["pageSize"])
        //        {
        //            Filter = queryString["filter"],
        //            FilterArgs = queryString["filterArgs"]?.Split(','),
        //            Search = queryString["search"],
        //            Sort = queryString["sort"],
        //        };

        //        var response = await repository.GetData(Guid.Empty, query);
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex, ModuleName);
        //    }
        //}

        //[HttpGet("{id}")]
        //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        //[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        //[ProducesResponseType(typeof(DataResponse<TenantDto>), (int)HttpStatusCode.OK)]
        //public async Task<IActionResult> GetById([Required, FromRoute] Guid id/*, [FromQuery] bool include*/)
        //{
        //    try
        //    {
        //        var tenant = await repository.GetByIdAsync(id/*, include*/);
        //        if (tenant == null)
        //        {
        //            return NotFound(MessageResponse.Failure($"{ModuleName} id '{id}' not found"));
        //        }
        //        return Ok(DataResponse<TenantDto>.Succeeded(tenant));
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex, ModuleName);
        //    }
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        //[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        //[ProducesResponseType(typeof(DataResponse<TenantDto>), (int)HttpStatusCode.Created)]
        //public async Task<IActionResult> Create(TenantDto model)
        //{
        //    try
        //    {
        //        #region Validation

        //        var validationResult = await repository.ValidateAsync(model);

        //        #endregion Validation

        //        if (validationResult.IsValid)
        //        {
        //            var tenant = await repository.CreateAsync(model);
        //            return CreatedAtAction(nameof(GetById), new { id = tenant.Id },
        //                DataResponse<TenantDto>.Succeeded(tenant));
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
        //[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        //[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        //[ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        //public async Task<IActionResult> Update(Guid id, TenantDto model)
        //{
        //    try
        //    {
        //        if (id != model.Id)
        //        {
        //            return BadRequest(MessageResponse.Failure($"{ModuleName} id mismatch"));
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
        //[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        //[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        //[ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        //public async Task<IActionResult> Delete(Guid id)
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
