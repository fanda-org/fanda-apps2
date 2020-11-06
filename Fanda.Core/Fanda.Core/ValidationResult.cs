using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace Fanda.Core
{
    public class ValidationError
    {
        public ValidationError()
        {
        }

        public ValidationError(string field, string message)
        {
            Field = field != string.Empty ? field : null;
            Message = message;
        }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }

        public string Message { get; }
    }

    public class ValidationErrors : List<ValidationError>
    {
        //public void Clear() => Errors.Clear();

        public void AddError(string field, string message)
        {
            Add(new ValidationError(field, message));
        }

        public bool IsValid()
        {
            return Count == 0;
        }

        #region Constructors

        public ValidationErrors()
        {
        }

        public ValidationErrors(List<ValidationError> errors)
        {
            AddRange(errors);
        }

        public ValidationErrors(ModelStateDictionary modelState)
        {
            AddRange(modelState.Keys
                .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                .ToList()
            );
        }

        #endregion Constructors
    }

    //public class DtoErrors : Dictionary<string, List<string>>
    //{
    //    public void AddErrors(string field, string errorMessage)
    //    {
    //        if (TryGetValue(field, out List<string> errors))
    //        {
    //            errors.Add(errorMessage);
    //            this[field] = errors;
    //        }
    //        else
    //        {
    //            if (errors == null)
    //            {
    //                errors = new List<string>();
    //            }
    //            errors.Add(errorMessage);
    //            Add(field, errors);
    //        }
    //    }
    //}
}