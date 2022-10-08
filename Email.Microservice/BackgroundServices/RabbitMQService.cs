using Email.Microservice.Application.Services;
using Email.Microservice.Model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Email.Microservice.BackgroundServices
{
    public class RabbitMQService : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private IServiceScopeFactory serviceScopeFactory;
        private IConfiguration _configuration;

        public RabbitMQService(IServiceScopeFactory _serviceScopeFactory, IConfiguration configuration)
        {
            serviceScopeFactory = _serviceScopeFactory;
            _configuration = configuration;
            InitRabbitMQ();
        }

        private void InitRabbitMQ()
        {

            var RabbitMQServer = _configuration["RabbitMQ:RabbitURL"];
            var RabbitMQUserName = _configuration["RabbitMQ:Username"];
            var RabbutMQPassword = _configuration["RabbitMQ:Password"];

            var factory = new ConnectionFactory()
            { HostName = RabbitMQServer, UserName = RabbitMQUserName, Password = RabbutMQPassword };

            // create connection
            _connection = factory.CreateConnection();

            // create channel
            _channel = _connection.CreateModel();

            //Direct Exchange Details like name and type of exchange
            _channel.ExchangeDeclare(_configuration["RabbitMqSettings:ExchangeName"], _configuration["RabbitMqSettings:ExchhangeType"]);

            //Declare Queue with Name and a few property related to Queue like durabality of msg, auto delete and many more
            _channel.QueueDeclare(queue: _configuration["RabbitMqSettings:QueueName"],
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);


            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            _channel.QueueBind(queue: _configuration["RabbitMqSettings:QueueName"], exchange: _configuration["RabbitMqSettings:ExchangeName"], routingKey: _configuration["RabbitMqSettings:RouteKey"]);

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                // received message
                var content = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());

                // acknowledge the received message
                _channel.BasicAck(ea.DeliveryTag, false);

                //Deserilized Message
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var emailDetails = JsonConvert.DeserializeObject<EmailModel>(message);

                //Stored Offer Details into the Database
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();
                    emailService.SendEmail(emailDetails);
                }

            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            _channel.BasicConsume(_configuration["RabbitMqSettings:QueueName"], false, consumer);
            return Task.CompletedTask;
        }

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e) { }
        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerRegistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerShutdown(object sender, ShutdownEventArgs e) { }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
