using Microsoft.EntityFrameworkCore;

namespace Product.Microservice.Core
{
    public class PagedList<T> : List<T>
    {
        public PagedList(IQueryable<T> source, int index, int pageSize)
        {



        }
        public PagedList(List<T> source, int index, int pageSize)
        {



        }
        public PagedList(List<T> source)
        {

            this.AddRange(source);

        }
        public static async Task<PagedList<T>> InitiatePage(IQueryable<T> source, int index, int pageSize)
        {

            PagedList<T> PagedList = new PagedList<T>(await source.Skip(index * pageSize)
                .Take(pageSize)
                .ToListAsync(), index, pageSize);
            return PagedList;
        }

    }
}
