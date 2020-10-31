using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mime;

namespace Fanda.Core.Base
{
    [Route("api/[controller]")]
    //[EnableCors("_MyAllowedOrigins")]
    //[Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    //[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any,
    //    VaryByQueryKeys = new[] { "pageIndex", "pageSize", "sort", "filter", "filterArgs" })]
    public class BaseController : ControllerBase
    {
        #region Non action methods

        [NonAction]
        private static InternalServerErrorResult InternalServerError(IMessageResponse value)   // [ActionResultObjectValueAttribute]
        {
            return new InternalServerErrorResult(value);
        }

        [NonAction]
        public virtual IActionResult ExceptionResult(Exception ex, string modelName)
        {
            if (ex is BadRequestException)
            {
                //return BadRequest(MessageResponse.Failure($"Invalid {modelName.ToLower()} id"));
                var valErr = (ex as BadRequestException).ValidationErrors;
                if (valErr == null)
                {
                    return BadRequest(MessageResponse.Failure(ex.Message));
                }
                else
                {
                    return BadRequest(MessageResponse.Failure(valErr));
                }
            }
            else if (ex is NotFoundException)
            {
                //return NotFound(MessageResponse.Failure($"{modelName} not found"));
                //var valErr = (ex as NotFoundException).ValidationErrors;
                //if (valErr == null)
                //{
                return NotFound(MessageResponse.Failure(ex.Message));
                //}
                //else
                //{
                //    return BadRequest(MessageResponse.Failure(valErr));
                //}
            }
            else if (ex is ArgumentNullException || ex is ArgumentException)
            {
                return BadRequest(MessageResponse.Failure(ex.Message)); // $"Invalid {modelName.ToLower()} id"
            }
            else
            {
                return InternalServerError(MessageResponse.Failure(ex.Message));
            }
        }

        #endregion Non action methods

        //protected async Task<IActionResult> HandleComputationFailure<T>(Task<T> f)
        //{
        //    try
        //    {
        //        var result = await f.ConfigureAwait(false);
        //        return Ok(Response<T>.Succeeded(result));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, Response<T>.Failure(ex.Message));
        //    }
        //}

        //protected async Task<IActionResult> HandleComputationFailureWithConditions<T>(Task<T> f, Func<T, bool> succeeded)
        //{
        //    try
        //    {
        //        var result = await f.ConfigureAwait(false);

        //        if (succeeded(result))
        //            return Ok(Response<T>.Succeeded(result));
        //        else
        //            return BadRequest(Response<T>.Failure("Internal error occurred"));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, Response<T>.Failure(ex.Message));
        //    }
        //}
    }
}
