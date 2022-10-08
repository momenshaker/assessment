using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Product.Microservice.Helpers;
using Product.Microservice.Interfaces.PresistenceInterface;
using Product.Microservice.Interfaces.RepositoryInterface;
using Product.Microservice.Interfaces.ServicesInterface;
using Product.Microservice.Persistence;
using Product.Microservice.Repositories;
using Product.Microservice.Services;

namespace Product.Microservice.Extensions
{
    public static class ServiceCollectionExtension
    {  /// <summary>
       /// Inject Services 
       /// </summary>
       /// <param name="services"></param>
       /// <returns></returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<IApplicationDbContext, ApplicationDbContext>
      (options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")), 128);

            services.AddScoped<ICatalogService, CatalogService>();
            services.AddScoped<ICatalogRepository, CatalogRepository>();
            var mappingConfig = new MapperConfiguration(mappingConfig =>
            {
                mappingConfig.AddProfile(new MapperProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }
}
