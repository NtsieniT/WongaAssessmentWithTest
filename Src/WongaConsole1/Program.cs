using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace WongaConsole1
{
    static class Program
    {
        static void Main(string[] args)
        {

            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };

            using var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();
            channel.QueueDeclare("wonga-queue", 
                durable: true, 
                exclusive: false, 
                autoDelete: false, 
                null);
            var name = Console.ReadLine();
            var message = $"Hello my name is, {name}";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish("", "wonga-queue", null, body);
        }
    }
}
