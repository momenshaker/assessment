using Product.Microservice.Core.Domain.Result;

namespace Product.Microservice.Core.Domain.Interfaces
{
    public interface IResult<T>
    {
        string Version { get; set; }
        List<string>? ErrorMessages { get; set; }
        PagingInfo? PagingInfo { get; set; }
        DateTime TimeStamp { get; set; }
    }
}
