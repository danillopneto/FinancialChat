using Jobsity.Challenge.FinancialChat.Consumer.Infra.Configuration;
using Jobsity.Challenge.FinancialChat.Consumer.Interfaces.MessageBroker;

namespace Jobsity.Challenge.FinancialChat.Consumer
{
    public class Worker : BackgroundService
    {
        private readonly DataAppSettings _dataAppSettings;

        private readonly ILogger<Worker> _logger;

        private readonly IMessageBroker _message;

        public Worker(
                      IMessageBroker message,
                      DataAppSettings dataAppSettings,
                      ILogger<Worker> logger)
        {
            _message = message ?? throw new ArgumentNullException(nameof(message));
            _dataAppSettings = dataAppSettings ?? throw new ArgumentNullException(nameof(dataAppSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                        _message.ProcessMessage(_dataAppSettings.MessageBroker);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                    }
                    finally
                    {
                        await Task.Delay(_dataAppSettings.IntervalWorkerActive, stoppingToken);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error trying to process the message.");
            }
        }
    }
}