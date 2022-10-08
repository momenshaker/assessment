using AutoMapper;
using Newtonsoft.Json;
using Product.Microservice.Core.Domain.Result;
using Product.Microservice.Core.Enums;
using Product.Microservice.Dto;
using Product.Microservice.Infrastructure.Entities;
using Product.Microservice.Interfaces.RepositoryInterface;
using Product.Microservice.Interfaces.ServicesInterface;
using Product.Microservice.Repositories;
using RabbitMQ.Client;
using System.Text;

namespace Product.Microservice.Services
{
    public class CatalogService : ICatalogService
    {
        private IConfiguration _configuration;
        private ICatalogRepository _CatalogsRepository;
        private IMapper _mapper;
        public CatalogService(ICatalogRepository CatalogsRepository, IMapper mapper, IConfiguration configuration)
        {
            _CatalogsRepository = CatalogsRepository;
            _mapper = mapper;
            _configuration = configuration;
        }
        /// <summary>
        /// Using this function you can add new Catalog
        /// </summary>
        /// <param name="CatalogItem"></param>
        /// <returns></returns>
        public async Task<Result<int>> AddCatalogAsync(CatalogDto CatalogItem)
        {

            var mappedCatalog = _mapper.Map<Catalog>(CatalogItem);
            var savedValue = await _CatalogsRepository.AddCatalogAsync(mappedCatalog);
            SendEmail(new EmailDto()
            {
                Subject = "New Catalog Item Added",
                EmailAddress = "momen_shaker@windowslive.com",
                Body = "New Catalog Item Added"

            });
            var result = new Result<int>()
            {
                Data = savedValue,
                StatusCode = StatusCode.Success,
                SuccessMessage = savedValue != 0 ? "Catalog Added Successfully" : "Catalog Not Added"
            };
            return result;

        }
        /// <summary>
        /// using this function you delete Catalog
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result<int>> DeleteCatalogAsync(int id)
        {
            var deletedValue = await _CatalogsRepository.DeleteCatalogAsync(id);

            var result = new Result<int>()
            {
                Data = deletedValue,
                StatusCode = StatusCode.Success,
                SuccessMessage = "Catalog Updated Successfully"
            };
            return result;
        }
        /// <summary>
        /// using this function you can edit Catalog
        /// </summary>
        /// <param name="CatalogItem"></param>
        /// <returns></returns>

        public async Task<Result<int>> EditCatalogAsync(CatalogDto CatalogItem)
        {

            var mappedCatalog = _mapper.Map<Catalog>(CatalogItem);
            var savedValue = await _CatalogsRepository.EditCatalogAsync(mappedCatalog);
            var result = new Result<int>()
            {
                Data = savedValue,
                StatusCode = StatusCode.Success,
                SuccessMessage = savedValue != 0 ? "Catalog Updated Successfully" : "Catalog Not Updated"
            };

            return result;
        }
        /// <summary>
        /// using this function you can get Catalog information by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result<CatalogDto>> GetCatalogAsync(int id)
        {
            var CatalogRow = await _CatalogsRepository.GetCatalogAsync(id);
            var mappedItem = _mapper.Map<CatalogDto>(CatalogRow);
            var result = new Result<CatalogDto>(mappedItem)
            {
                Data = mappedItem,
                StatusCode = StatusCode.Success,
                SuccessMessage = mappedItem != null ? "Catalog Retrived Successfully" : "Catalog Not Found"
            };
            return result;
        }
        /// <summary>
        /// using this function you can retrive a list of Catalogs with filters, sorting and pagination 
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="searhText"></param>
        /// <param name="order"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>

        public async Task<Result<List<CatalogDto>>> GetCatalogsAsync(PagingInfo pagingInfo)
        {
            var retrivedData = await _CatalogsRepository.GetCatalogsAsync(pagingInfo);
            var mappedList = _mapper.Map<List<CatalogDto>>(retrivedData);
            var result = new Result<List<CatalogDto>>(mappedList)
            {
                Data = mappedList,
                StatusCode = StatusCode.Success,
                SuccessMessage = mappedList != null ? "Catalogs List Retrived Successfully" : "Catalogs Not Found",
                PagingInfo = pagingInfo
            };
            return result;
        }

        private bool SendEmail(EmailDto emailIitem)
        {
            var RabbitMQServer = _configuration["RabbitMQ:RabbitURL"];
            var RabbitMQUserName = _configuration["RabbitMQ:Username"];
            var RabbutMQPassword = _configuration["RabbitMQ:Password"];

            try
            {
                var factory = new ConnectionFactory()
                { HostName = RabbitMQServer, UserName = RabbitMQUserName, Password = RabbutMQPassword };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    //Direct Exchange Details like name and type of exchange
                    channel.ExchangeDeclare(_configuration["RabbitMqSettings:ExchangeName"], _configuration["RabbitMqSettings:ExchhangeType"]);

                    //Declare Queue with Name and a few property related to Queue like durabality of msg, auto delete and many more
                    channel.QueueDeclare(queue: _configuration["RabbitMqSettings:QueueName"],
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    channel.QueueBind(queue: _configuration["RabbitMqSettings:QueueName"], exchange: _configuration["RabbitMqSettings:ExchangeName"], routingKey: _configuration["RabbitMqSettings:RouteKey"]);
                    string productDetail = JsonConvert.SerializeObject(emailIitem);
                    var body = Encoding.UTF8.GetBytes(productDetail);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    channel.BasicPublish(exchange: _configuration["RabbitMqSettings:ExchangeName"],
                                         routingKey: _configuration["RabbitMqSettings:RouteKey"],
                                         basicProperties: properties,
                                         body: body);

                    return true;
                }
            }

            catch (Exception)
            {
            }
            return false;
        }
    }
}