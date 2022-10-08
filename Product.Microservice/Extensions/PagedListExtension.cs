using Microsoft.EntityFrameworkCore;
using Product.Microservice.Core;
using Product.Microservice.Core.Domain.Result;

namespace Product.Microservice.Extensions
{
    public static class PagedListExtension
    {
        /// <summary>
        /// Pagination for IQueryable 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="souuce"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> souuce, PagingInfo pagingInfo)
        {
            pagingInfo.TotalCount = await souuce.CountAsync();
            pagingInfo.PageSize = pagingInfo.PageSize;
            pagingInfo.CurrentPage = pagingInfo.CurrentPage;
            var pagedList = new PagedList<T>(await souuce.Skip(pagingInfo.CurrentPage * pagingInfo.PageSize)
                .Take(pagingInfo.PageSize).ToListAsync());
            return pagedList;

        }
    }
}
