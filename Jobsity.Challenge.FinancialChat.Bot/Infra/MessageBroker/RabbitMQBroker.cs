using Jobsity.Challenge.FinancialChat.Bot.Infra.Configurations;
using Jobsity.Challenge.FinancialChat.Bot.Interfaces.MessageBroker;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Jobsity.Challenge.FinancialChat.Bot.Infra.MessageBroker
{
    public class RabbitMQBroker : IMessageBroker
    {
        public void Publish<T>(T messageData, MessageBrokerSettings messageBroker)
        {
            var factory = new ConnectionFactory() { HostName = messageBroker.HostName };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: messageBroker.Queue,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var json = JsonConvert.SerializeObject(messageData);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: messageBroker.RoutingKey,
                                 basicProperties: null,
                                 body: body);
        }
    }
}