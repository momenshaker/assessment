
using Microsoft.EntityFrameworkCore;
using Product.Microservice.Core;
using Product.Microservice.Core.Domain.Result;
using Product.Microservice.Extensions;
using Product.Microservice.Infrastructure.Entities;
using Product.Microservice.Interfaces.PresistenceInterface;
using Product.Microservice.Interfaces.RepositoryInterface;

namespace Product.Microservice.Repositories
{
    public class CatalogRepository : ICatalogRepository, IDisposable
    {
        private IApplicationDbContext _applicationDbContext;

        public CatalogRepository(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<int> AddCatalogAsync(Catalog CatalogItem)
        {
            await _applicationDbContext.Catalogs.AddAsync(CatalogItem);
            await _applicationDbContext.SaveChangesAsync();
            return CatalogItem.Id;
        }

        public async Task<int> DeleteCatalogAsync(int id)
        {
            var CatalogItem = await _applicationDbContext.Catalogs.FindAsync(id);
            _applicationDbContext.Catalogs.Remove(CatalogItem);
            await _applicationDbContext.SaveChangesAsync();
            return id;
        }

        public async Task<int> EditCatalogAsync(Catalog CatalogItem)
        {
            _applicationDbContext.Entry(CatalogItem).State = EntityState.Modified;
            await _applicationDbContext.SaveChangesAsync();
            return CatalogItem.Id;
        }

        public async Task<Catalog> GetCatalogAsync(int id)
        {
            var CatalogItem = await _applicationDbContext.Catalogs.FindAsync(id);
            return CatalogItem;
        }

        public async Task<PagedList<Catalog>> GetCatalogsAsync(PagingInfo pagingInfo)
        {
            var query = _applicationDbContext.Catalogs.AsQueryable();
            return await query.ToPagedListAsync(pagingInfo);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _applicationDbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
