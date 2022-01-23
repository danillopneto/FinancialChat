using Jobsity.Challenge.FinancialChat.Consumer.Domain.DataContracts.Requests;
using Jobsity.Challenge.FinancialChat.Consumer.Interfaces.Gateways;
using Jobsity.Challenge.FinancialChat.Consumer.Interfaces.MessageBroker;
using Jobsity.Challenge.FinancialChat.Core.Infra.Configurations;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Jobsity.Challenge.FinancialChat.Consumer.Infra.MessageBroker
{
    public class RabbitMQBroker : IMessageBroker
    {
        private readonly IChatGateway _chatGateway;

        public RabbitMQBroker(IChatGateway chatGateway)
        {
            _chatGateway = chatGateway ?? throw new ArgumentNullException(nameof(chatGateway));
        }

        public void ProcessMessage(MessageBrokerSettings messageBroker)
        {
                var factory = new ConnectionFactory() { Uri = new Uri(messageBroker.ConnectionString), DispatchConsumersAsync = true };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                                 queue: messageBroker.Queue,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var request = JsonConvert.DeserializeObject<CommandMessageRequest>(message);
                await _chatGateway.SendMessageAsyc(request);
            };

            channel.BasicConsume(
                                 queue: messageBroker.Queue,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}