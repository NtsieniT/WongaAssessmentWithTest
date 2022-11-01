using Microsoft.VisualStudio.TestTools.UnitTesting;
using RabbitMQ.Client;
using System;
using System.Text;
using FluentAssertions;

namespace WongaTests
{
    [TestClass]
    public class UnitTest1
    {
        private const string QueueName = "wonga-queue";
        private const string ExchangeName = "testing.topic";
        private const string RoutingKey = "test";
        private IConnection _connection;
        private IModel _model;

        [TestInitialize]
        public void Setup()
        {
            _connection = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            }.CreateConnection();

            _model = _connection.CreateModel();
        }

        [TestCleanup]
        public void CloseConnection()
        {
            _connection.Close();
        }

        [TestMethod]
        public void Publishes()
        {
            _model.ExchangeDeclare(ExchangeName, ExchangeType.Topic);
            _model.QueueDeclare(QueueName, false, false, false, null);
            byte[] messageBodyBytes = Encoding.UTF8.GetBytes("Hello, world!");
            _model.BasicPublish(ExchangeName, "", null, messageBodyBytes);
        }

        [TestMethod]
        public void ConsumeWithAutoAck()
        {
            _model.ExchangeDeclare(ExchangeName, ExchangeType.Topic);
            _model.QueueDeclare(QueueName, false, false, false, null);
            _model.QueueBind(QueueName, ExchangeName, RoutingKey, null);

            BasicGetResult result = _model.BasicGet(QueueName, true);
            string content = Encoding.UTF8.GetString(result.Body.ToArray());
            content.Should().Be("Hello, world!");

        }
    }
}
