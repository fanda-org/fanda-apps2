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
    public class BaseController : ControllerBase
    {
        #region Non action methods

        [NonAction]
        private InternalServerErrorResult InternalServerError(IMessageResponse value)   // [ActionResultObjectValueAttribute]
        {
            return new InternalServerErrorResult(value);
        }

        [NonAction]
        public virtual IActionResult ExceptionResult(Exception ex, string modelName)
        {
            if (ex is BadRequestException || ex is ArgumentNullException || ex is ArgumentException)
            {
                //return BadRequest(MessageResponse.Failure($"Invalid {modelName.ToLower()} id"));
                return BadRequest(MessageResponse.Failure(ex.Message));
            }
            else if (ex is NotFoundException)
            {
                //return NotFound(MessageResponse.Failure($"{modelName} not found"));
                return NotFound(MessageResponse.Failure(ex.Message));
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
