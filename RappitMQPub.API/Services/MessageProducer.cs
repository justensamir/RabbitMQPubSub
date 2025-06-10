using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;


namespace RabbitMQPub.API.Services
{
    public class MessageProducer : IMessageProducer
    {
        public void SendingMessage<T>(T message)
        {
            // Configure the configuration to connect with rabbitMQ
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var cfg = config.GetSection("RabbitMQ");

            var factory = new ConnectionFactory
            {
                HostName = cfg["HostName"],
                Port = int.Parse(cfg["Port"]),
                UserName = cfg["UserName"],
                Password = cfg["Password"],
                VirtualHost = cfg["VirtualHost"]
            };

            var conn = factory.CreateConnection();

            using var channel = conn.CreateModel();

            channel.QueueDeclare("bookings", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var jsonString = JsonSerializer.Serialize(message);

            var body = Encoding.UTF8.GetBytes(jsonString);

            channel.BasicPublish("", "bookings", body: body);
        }
    }
}
