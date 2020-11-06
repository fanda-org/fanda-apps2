using System;

namespace Fanda.Core.Base
{
    public interface IMessageResponse
    {
        string Message { get; set; }
        bool Success { get; set; }
        string ErrorMessage { get; set; }
        ValidationErrors Errors { get; set; }
    }

    public interface IDataResponse<TModel> : IMessageResponse
    {
        TModel Data { get; set; }
    }

    public interface IPagedResponse<TModel> : IDataResponse<TModel>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        int ItemsCount { get; set; }
        int PageCount { get; }
        public int FirstRowOnPage { get; }
        public int LastRowOnPage { get; }
    }

    // public class Response : IResponse
    // {
    //     public string Message { get; set; }
    //     public bool Success { get; set; }
    //     public string ErrorMessage { get; set; }
    //     public ValidationResultModel Errors { get; set; }

    //     public static Response Succeeded(string message = null) => new Response
    //     {
    //         Success = true,
    //         Message = message
    //     };
    //     public static Response Failure(string errorMessage) => new Response
    //     {
    //         Success = false,
    //         ErrorMessage = errorMessage
    //     };
    //     public static Response Failure(ValidationResultModel errors, string errorMessage = null) => new Response
    //     {
    //         Success = false,
    //         Errors = errors,
    //         ErrorMessage = errorMessage
    //     };
    //     public static Response Failure(ModelStateDictionary modelState) => new Response
    //     {
    //         Success = false,
    //         Errors = new ValidationResultModel(modelState)
    //     };
    // }

    public class MessageResponse : IMessageResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public ValidationErrors Errors { get; set; }

        public static MessageResponse Succeeded(string message = null)
        {
            return new MessageResponse { Success = true, Message = message };
        }

        public static MessageResponse Failure(string errorMessage)
        {
            return new MessageResponse { Success = false, ErrorMessage = errorMessage };
        }

        public static MessageResponse Failure(ValidationErrors errors, string errorMessage = null)
        {
            return new MessageResponse { Success = false, Errors = errors, ErrorMessage = errorMessage };
        }

        //public static MessageResponse Failure(ModelStateDictionary modelState, string errorMessage = null)
        //    => new MessageResponse
        //    {
        //        Success = false,
        //        Errors = new ValidationResultModel(modelState),
        //        ErrorMessage = errorMessage
        //    };
    }

    public class DataResponse<TModel> : MessageResponse, IDataResponse<TModel>
    {
        public TModel Data { get; set; }

        public static DataResponse<TModel> Succeeded(TModel data, string message = null)
        {
            return new DataResponse<TModel> { Success = true, Message = message, Data = data };
        }
    }

    public class PagedResponse<TModel> : DataResponse<TModel>, IPagedResponse<TModel>
    {
        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int ItemsCount { get; set; }

        public int PageCount => ItemsCount < PageSize ? 1 : (int)((double)ItemsCount / PageSize);

        public int FirstRowOnPage
        {
            get
            {
                if (PageIndex > PageCount)
                {
                    return 0;
                }

                return Math.Min(ItemsCount, ((PageIndex - 1) * PageSize) + 1);
            }
        }

        //=> Math.Min((int)(((PageNumber - 1) * PageSize) + 1), (int)LastRowOnPage);
        public int LastRowOnPage
        {
            get
            {
                if (PageIndex > PageCount)
                {
                    return 0;
                }

                return Math.Min(ItemsCount, (FirstRowOnPage + PageSize) - 1);
            }
        }

        //=> Math.Min((int)PageNumber * (int)PageSize, (int)ItemsCount);

        public static PagedResponse<TModel> Succeeded(TModel data, int itemsCount, int pageIndex, int pageSize,
            string message = null)
        {
            return new PagedResponse<TModel>
            {
                Success = true,
                Data = data,
                ItemsCount = itemsCount,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Message = message
            };
        }
    }

    //public class DataResponse : DataResponse<object> { }
}