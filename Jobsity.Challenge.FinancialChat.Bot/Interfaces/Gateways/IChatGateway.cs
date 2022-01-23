using Jobsity.Challenge.FinancialChat.Bot.Domain.DataContracts.Requests;

namespace Jobsity.Challenge.FinancialChat.Bot.Interfaces.Gateways
{
    public interface IChatGateway
    {
        Task SendMessageAsyc(CommandMessageRequest request, CancellationToken cancellationToken);
    }
}
