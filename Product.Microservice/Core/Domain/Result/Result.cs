using Product.Microservice.Core.Domain.Interfaces;
using Product.Microservice.Core.Enums;

namespace Product.Microservice.Core.Domain.Result
{
    public class Result<T> : IResult<T>
    {
        public string Version { get; set; } = "1.0";
        public StatusCode StatusCode { get; set; }
        public List<string>? ErrorMessages { get; set; }
        public T Data { get; set; }
        public PagingInfo? PagingInfo { get; set; }
        public string? SuccessMessage { get; set; } = null;
        public DateTime TimeStamp { get; set; } = DateTime.Now;

        public Result()
        {


        }
        public Result(T data)
        {
            Data = data;


        }
        public Result(T data, PagingInfo pagingInfo)
        {

            Data = data;
            PagingInfo = pagingInfo;
        }

        public void Success(string successMessage)
        {

            StatusCode = StatusCode.Success;
            SuccessMessage = successMessage;
        }
    }
}
