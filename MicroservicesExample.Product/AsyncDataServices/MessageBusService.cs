using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace MicroservicesExample.Product.AsyncDataServices
{
    public class MessageBusService : IMessageBusService
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusService(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                Uri = new Uri("amqp://admin:password@rabbitmq:5672")
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public void PublishNewProduct(Data.Entities.Product product)
        {
            var message = JsonSerializer.Serialize(product);

            if (_connection.IsOpen)
            {
                SendMessage(message);
            }
        }

        public void Dispose()
        {
            if(_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "trigger", 
                                  routingKey: "",
                                  basicProperties: null,
                                  body: body);
        }
    }
}
