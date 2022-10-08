using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Product.Microservice.Core.Domain.Result;
using Product.Microservice.Helpers;
using Product.Microservice.Interfaces.RepositoryInterface;
using Product.Microservice.Interfaces.ServicesInterface;
using Product.Microservice.Persistence;
using Product.Microservice.Repositories;
using Product.Microservice.Services;
using Xunit;

namespace Product.Microservice.Tests
{
    public class CatalogTest : IDisposable

    {
        protected ICatalogRepository CatalogRepository;
        protected ICatalogService CatalogService;
        protected ApplicationDbContext _applicationDbContext;
        private IConfiguration Configuration;
        public CatalogTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).Options;
            _applicationDbContext = new ApplicationDbContext(options);
            Seed.SeedData(_applicationDbContext);
            var mapperProfile = new MapperProfile();
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(mapperProfile));
            IMapper mockMapper = new Mapper(mapperConfiguration);
            CatalogRepository = new CatalogRepository(_applicationDbContext);
            var config = new ConfigurationBuilder()
    .AddJsonFile("AppSetting.json")
    .Build();
            Configuration = config;
            CatalogService = new CatalogService(CatalogRepository, mockMapper, Configuration);
        }

        public void Dispose()
        {
            _applicationDbContext.Dispose();

        }
        [Fact]
        public async void GetCatalogById()
        {
            var result = await CatalogService.GetCatalogAsync(1);
            Assert.NotNull(result.Data);
        }
        [Fact]
        public async void AddCatalog()
        {
            var result = await CatalogService.AddCatalogAsync(new Dto.CatalogDto() { name = "Test", cost = 1.1m, price = 1.2m, imageBase = "" });
            var equalResult = new Result<int>(4);
            Assert.Equal(equalResult.Data, result.Data);
        }
        [Fact]
        public async void EditCatalog()
        {
            var result = await CatalogService.EditCatalogAsync(new Dto.CatalogDto() { name = "Test", cost = 1.1m, id = 2, price = 1.2m, imageBase = "" });
            var equalResult = new Result<int>(2);
            Assert.Equal(equalResult.Data, result.Data);
        }
        [Fact]
        public async void RemoveCatalog()
        {
            var result = await CatalogService.DeleteCatalogAsync(1);
            var equalResult = new Result<int>(1);
            Assert.Equal(equalResult.Data, result.Data);
        }
    }
}