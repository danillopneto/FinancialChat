using Jobsity.Challenge.FinancialChat.Consumer.Domain.DataContracts.Requests;
using Jobsity.Challenge.FinancialChat.Consumer.Infra.Configuration;
using Jobsity.Challenge.FinancialChat.Consumer.Interfaces.Gateways;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Jobsity.Challenge.FinancialChat.Consumer
{
    public class Worker : BackgroundService
    {
        private readonly IChatGateway _chatGateway;

        private readonly DataAppSettings _dataAppSettings;

        private readonly ILogger<Worker> _logger;

        public Worker(
                      DataAppSettings dataAppSettings,
                      IChatGateway chatGateway,
                      ILogger<Worker> logger)
        {
            _dataAppSettings = dataAppSettings ?? throw new ArgumentNullException(nameof(dataAppSettings));
            _chatGateway = chatGateway ?? throw new ArgumentNullException(nameof(chatGateway));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var factory = new ConnectionFactory() { Uri = new Uri(_dataAppSettings.MessageBroker.ConnectionString), DispatchConsumersAsync = true };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();
                channel.QueueDeclare(
                                     queue: _dataAppSettings.MessageBroker.Queue,
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

                    channel.BasicAck(ea.DeliveryTag, false);
                };

                channel.BasicConsume(
                                     queue: _dataAppSettings.MessageBroker.Queue,
                                     autoAck: false,
                                     consumer: consumer);

                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation($"Worker active in: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    await Task.Delay(_dataAppSettings.IntervalWorkerActive, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error trying to process the message.");
            }
        }
    }
}