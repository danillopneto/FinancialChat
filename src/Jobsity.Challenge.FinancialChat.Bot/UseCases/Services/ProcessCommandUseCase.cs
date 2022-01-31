using Jobsity.Challenge.FinancialChat.Bot.Domain.DataContracts.Requests;
using Jobsity.Challenge.FinancialChat.Bot.Domain.Dtos;
using Jobsity.Challenge.FinancialChat.Bot.Infra.Configurations;
using Jobsity.Challenge.FinancialChat.Bot.Interfaces.Domain;
using Jobsity.Challenge.FinancialChat.Bot.Interfaces.Gateways;
using Jobsity.Challenge.FinancialChat.Bot.Interfaces.MessageBroker;
using Jobsity.Challenge.FinancialChat.Bot.Interfaces.UseCases;
using Jobsity.Challenge.FinancialChat.Core.Infra.Configurations;

namespace Jobsity.Challenge.FinancialChat.Bot.UseCases.Services
{
    public class ProcessCommandUseCase : IProcessCommandUseCase
    {
        private readonly IGetInfoCommandGateway _getInfoCommandGateway;

        private readonly ILogger<ProcessCommandUseCase> _logger;

        private readonly IMessageBroker _messageBroker;

        public ProcessCommandUseCase(
                                     IGetInfoCommandGateway getInfoCommandGateway,
                                     IMessageBroker messageBroker,
                                     ILogger<ProcessCommandUseCase> logger)
        {
            _getInfoCommandGateway = getInfoCommandGateway ?? throw new ArgumentNullException(nameof(getInfoCommandGateway));
            _messageBroker = messageBroker ?? throw new ArgumentNullException(nameof(messageBroker));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ProcessAsync(CommandDto command, AllowedCommandSettings allowedCommand, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _getInfoCommandGateway.GetInfoAsync(allowedCommand, command.CommandParameter);
                var converterType = Type.GetType(allowedCommand.Converter);
                if (converterType is not null)
                {
                    var commandConverter = Activator.CreateInstance(converterType) as ICommandConverter;
                    if (commandConverter is not null)
                    {
                        QueueMessage(allowedCommand.MessageBroker, commandConverter.Convert(result), command);
                        return;
                    }
                }

                throw new InvalidCastException("It wasn't able to convert the command.");
            }
            catch (Exception ex)
            {
                var message = string.Format("Error processing command {0} with parameter {1}", allowedCommand.Command, command.CommandParameter);
                _logger.LogError(ex, message);
                QueueMessage(allowedCommand.MessageBroker, message, command);
            }
        }

        private void QueueMessage(MessageBrokerSettings messageBroker, string message, CommandDto command)
        {
            var request = new CommandMessageRequest
            {
                Destination = command.Destination,
                Message = message,
                SenderId = command.SenderId
            };

            _messageBroker.Publish(request, messageBroker);
        }
    }
}