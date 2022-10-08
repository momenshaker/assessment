using Product.Microservice.Core.Domain.Result;
using Product.Microservice.Dto;

namespace Product.Microservice.Interfaces.ServicesInterface
{
    public interface ICatalogService
    {
        Task<Result<int>> AddCatalogAsync(CatalogDto catalogDto);
        Task<Result<int>> EditCatalogAsync(CatalogDto catalogDto);
        Task<Result<int>> DeleteCatalogAsync(int id);
        Task<Result<CatalogDto>> GetCatalogAsync(int id);
        Task<Result<List<CatalogDto>>> GetCatalogsAsync(PagingInfo pagingInfo);

    }
}
