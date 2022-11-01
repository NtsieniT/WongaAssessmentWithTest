using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace WongaConsole2
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

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (SENDER, e) => {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var index = message.IndexOf(',');
                var name = message.Remove(0, index + 1).Trim();
                Console.WriteLine($"Hello {name}, I am your father!");
            };

            channel.BasicConsume("wonga-queue", true, consumer);
            Console.ReadLine();
        }
    }
}
