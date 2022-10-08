using Product.Microservice.Infrastructure.Entities;
using Product.Microservice.Persistence;

namespace Product.Microservice.Tests
{
    public static class Seed
    {
        public static async void SeedData(ApplicationDbContext applicationDbContext)
        {
            applicationDbContext.AddRange(
                new List<Catalog>{
                    new Catalog
            {
                Id = 1,
                Name = "Product 1",
                Cost = 1.0m,
                Price = 1.2m,
                ImageBase = ""
            },new Catalog
            {
                Id = 2,
                Name = "Product 2",
                Cost = 2.0m,
                Price = 2.2m,
                ImageBase = ""
            },new Catalog
            {
                Id = 3,
                Name = "Product 3",
                Cost = 3.0m,
                Price = 3.2m,
                ImageBase = ""
            } });



            applicationDbContext.SaveChanges();
        }
    }
}
