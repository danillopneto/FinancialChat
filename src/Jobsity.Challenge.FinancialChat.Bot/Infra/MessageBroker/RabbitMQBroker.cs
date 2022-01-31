using Jobsity.Challenge.FinancialChat.Bot.Interfaces.MessageBroker;
using Jobsity.Challenge.FinancialChat.Core.Infra.Configurations;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Jobsity.Challenge.FinancialChat.Bot.Infra.MessageBroker
{
    public class RabbitMQBroker : IMessageBroker
    {
        private readonly ILogger<RabbitMQBroker> _logger;

        public RabbitMQBroker(ILogger<RabbitMQBroker> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Publish<T>(T message, MessageBrokerSettings messageBroker)
        {
            try
            {
                var factory = new ConnectionFactory() { Uri = new Uri(messageBroker.ConnectionString) };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();
                channel.QueueDeclare(
                                     queue: messageBroker.Queue,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);

                _logger.LogInformation("Publishing {Message} into {Queue} - {RoutingKey}", json, messageBroker.Queue, messageBroker.RoutingKey);

                channel.BasicPublish(
                                     exchange: string.Empty,
                                     routingKey: messageBroker.RoutingKey,
                                     basicProperties: null,
                                     body: body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error trying to publish message into {Hostname}", messageBroker.ConnectionString);
                throw;
            }
        }
    }
}