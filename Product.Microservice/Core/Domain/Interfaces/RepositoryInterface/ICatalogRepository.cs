using Product.Microservice.Core;
using Product.Microservice.Core.Domain.Result;
using Product.Microservice.Infrastructure.Entities;

namespace Product.Microservice.Interfaces.RepositoryInterface
{
    public interface ICatalogRepository
    {
        Task<int> AddCatalogAsync(Catalog catalog);
        Task<int> EditCatalogAsync(Catalog catalog);
        Task<int> DeleteCatalogAsync(int id);
        Task<Catalog> GetCatalogAsync(int id);
        Task<PagedList<Catalog>> GetCatalogsAsync(PagingInfo pagingInfo);

    }
}
