using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Product.Microservice.Infrastructure.Entities;
using Product.Microservice.Persistence;

namespace Product.Microservice.Extensions
{
    public static class ApplicationBuilderExtension
    { /// <summary>
      /// Prepare database by adding rows and initiate migration if there is any migration not applied
      /// </summary>
      /// <param name="app"></param>
      /// <param name="jsonData"></param>
      /// <returns></returns>
        public static async Task<IApplicationBuilder> PrepareDatabase(this IApplicationBuilder app, string jsonData)
        {

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();
                var CatalogData = JsonConvert.DeserializeObject<List<Catalog>>(jsonData);


                if (!context.Catalogs.Any())
                {
                    foreach (var client in CatalogData)
                    {
                        context.Catalogs.Add(client);
                    }
                    context.SaveChanges();
                }

                return app;
            }
        }
    }
}
