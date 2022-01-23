using Jobsity.Challenge.FinancialChat.Bot.Domain.DataContracts.Requests;
using Jobsity.Challenge.FinancialChat.Bot.Domain.Dtos;
using Jobsity.Challenge.FinancialChat.Bot.Infra.Configurations;
using Jobsity.Challenge.FinancialChat.Bot.Interfaces.Domain;
using Jobsity.Challenge.FinancialChat.Bot.Interfaces.Gateways;
using Jobsity.Challenge.FinancialChat.Bot.Interfaces.UseCases;

namespace Jobsity.Challenge.FinancialChat.Bot.UseCases.Services
{
    public class ProcessCommandUseCase : IProcessCommandUseCase
    {
        private readonly IChatGateway _chatGateway;

        private readonly IGetInfoCommandGateway _getInfoCommandGateway;

        private readonly ILogger<ProcessCommandUseCase> _logger;

        public ProcessCommandUseCase(
                                     IGetInfoCommandGateway getInfoCommandGateway,
                                     IChatGateway chatGateway,
                                     ILogger<ProcessCommandUseCase> logger)
        {
            _getInfoCommandGateway = getInfoCommandGateway ?? throw new ArgumentNullException(nameof(getInfoCommandGateway));
            _chatGateway = chatGateway ?? throw new ArgumentNullException(nameof(chatGateway));
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
                        await PostMessage(commandConverter.Convert(result), command, cancellationToken);
                        return;
                    }
                }

                throw new InvalidCastException("It wasn't able to convert the command.");
            }
            catch (Exception ex)
            {
                var message = string.Format("Error processing command {0} with parameter {1}", allowedCommand.Command, command.CommandParameter);
                _logger.LogError(ex, message);
                await PostMessage(message, command, cancellationToken);
            }
        }

        private async Task PostMessage(string message, CommandDto command, CancellationToken cancellationToken)
        {
            var request = new CommandMessageRequest
            {
                Destination = command.Destination,
                Message = message,
                SenderId = command.SenderId
            };

            await _chatGateway.SendMessageAsyc(request, cancellationToken);
        }
    }
}