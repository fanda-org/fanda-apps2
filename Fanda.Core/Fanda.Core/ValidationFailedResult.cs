using Fanda.Core.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Fanda.Core
{
    public class ValidationFailedResult : ObjectResult
    {
        public ValidationFailedResult(ModelStateDictionary modelState)
            : base(DataResponse<string>.Failure(new ValidationErrors(modelState), "Validation failed"))
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }

    public class InternalServerErrorResult : ObjectResult
    {
        public InternalServerErrorResult(IMessageResponse value) :
            base(value)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
