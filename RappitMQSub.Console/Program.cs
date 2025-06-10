// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Welcom to the ticketing service");

// Configure the configuration to connect with rabbitMQ

var factory = new ConnectionFactory
{
    HostName= "localhost",
    Port= 5672,
    UserName= "user",
    Password= "password",
    VirtualHost= "/"
};

var conn = factory.CreateConnection();

using var channel = conn.CreateModel();

channel.QueueDeclare("bookings", durable: true, exclusive: false, autoDelete: false, arguments: null);

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, eventArgs) =>
{
    // getting my bytes array
    var body = eventArgs.Body.ToArray();

    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine($" new message is initiated - {message}");
    
};

channel.BasicConsume("bookings", true, consumer);

Console.ReadKey();