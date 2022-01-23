using Jobsity.Challenge.FinancialChat.Domain.DataContracts.Requests;
using Jobsity.Challenge.FinancialChat.Domain.Dtos;
using Jobsity.Challenge.FinancialChat.Interfaces.Gateways;
using Jobsity.Challenge.FinancialChat.Interfaces.UseCases;

namespace Jobsity.Challenge.FinancialChat.UseCases.UseCases
{
    public class DispatchCommandUseCase : IDispatchCommandUseCase
    {
        private readonly IBotGateway _botGateway;

        public DispatchCommandUseCase(IBotGateway botGateway)
        {
            _botGateway = botGateway ?? throw new ArgumentNullException(nameof(botGateway));
        }

        public async Task DispatchAsync(ChatMessageDto chatMessage)
        {
            var request = new CommandRequest
            {
                Destination = chatMessage.Destination,
                Message = chatMessage.Message,
                SenderId = chatMessage.Sender.Id
            };
            await _botGateway.PublishCommandAsync(request);
        }
    }
}