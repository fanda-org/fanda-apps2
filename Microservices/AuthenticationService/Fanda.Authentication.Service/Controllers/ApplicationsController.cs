using Fanda.Authentication.Domain;
using Fanda.Authentication.Repository;
using Fanda.Authentication.Repository.Dto;
using Fanda.Core.Base;

namespace Fanda.Authentication.Service.Controllers
{
    public class ApplicationsController :
        SuperController<IApplicationRepository, Application, ApplicationDto, ApplicationListDto>
    {
        //private const string ModuleName = "Application";
        //private readonly IApplicationRepository repository;

        public ApplicationsController(IApplicationRepository repository) : base(repository)
        {
            //this.repository = repository;
        }

        //[HttpGet]
        //[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        //[ProducesResponseType(typeof(DataResponse<List<ApplicationListDto>>), (int)HttpStatusCode.OK)]
        //public async Task<IActionResult> GetAll()
        //{
        //    try
        //    {
        //        NameValueCollection queryString = HttpUtility.ParseQueryString(Request.QueryString.Value);
        //        var query = new Query(queryString["pageIndex"], queryString["pageSize"])
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
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
        //[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        //[ProducesResponseType(typeof(DataResponse<ApplicationDto>), (int)HttpStatusCode.OK)]
        //public async Task<IActionResult> GetById([Required, FromRoute] Guid id/*, [FromQuery] bool include*/)
        //{
        //    try
        //    {
        //        var app = await repository.GetByIdAsync(id/*, include*/);
        //        if (app == null)
        //        {
        //            return NotFound(MessageResponse.Failure($"{ModuleName} id '{id}' not found"));
        //        }
        //        return Ok(DataResponse<ApplicationDto>.Succeeded(app));
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
        //[ProducesResponseType(typeof(DataResponse<ApplicationDto>), (int)HttpStatusCode.Created)]
        //public async Task<IActionResult> Create(ApplicationDto model)
        //{
        //    try
        //    {
        //        #region Validation

        //        var validationResult = await repository.ValidateAsync(model);

        //        #endregion Validation

        //        if (validationResult.IsValid)
        //        {
        //            var app = await repository.CreateAsync(model);
        //            return CreatedAtAction(nameof(GetById), new { id = app.Id },
        //                DataResponse<ApplicationDto>.Succeeded(app));
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
        //public async Task<IActionResult> Update(Guid id, ApplicationDto model)
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

        // [HttpPost("map")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        // public async Task<IActionResult> Map([FromBody] AppResourceDto appResource)
        // {
        //     try
        //     {
        //         bool success = await repository.MapResource(appResource);
        //         if (success)
        //         {
        //             return Ok(DataResponse.Succeeded("Mapped app-resource successfully"));
        //         }
        //         return NotFound(DataResponse.Failure($"App-resource not found"));
        //     }
        //     catch (Exception ex)
        //     {
        //         if (ex is BadRequestException || ex is ArgumentNullException || ex is ArgumentException)
        //         {
        //             return BadRequest(DataResponse.Failure("Invalid app-resource id"));
        //         }
        //         else if (ex is NotFoundException)
        //         {
        //             return NotFound(DataResponse.Failure("App-resource not found"));
        //         }
        //         else
        //         {
        //             return InternalServerError(DataResponse.Failure(ex.Message));
        //         }
        //     }
        // }

        // [HttpPost("unmap")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        // public async Task<IActionResult> Unmap([FromBody] AppResourceDto appResource)
        // {
        //     try
        //     {
        //         bool success = await repository.UnmapResource(appResource);
        //         if (success)
        //         {
        //             return Ok(DataResponse.Succeeded("Unmapped app-resource successfully"));
        //         }
        //         return NotFound(DataResponse.Failure($"App-resource not found"));
        //     }
        //     catch (Exception ex)
        //     {
        //         if (ex is BadRequestException || ex is ArgumentNullException || ex is ArgumentException)
        //         {
        //             return BadRequest(DataResponse.Failure("Invalid app-resource id"));
        //         }
        //         else if (ex is NotFoundException)
        //         {
        //             return NotFound(DataResponse.Failure("App-resource not found"));
        //         }
        //         else
        //         {
        //             return InternalServerError(DataResponse.Failure(ex.Message));
        //         }
        //     }
        // }
    }
}
