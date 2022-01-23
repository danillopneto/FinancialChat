using Jobsity.Challenge.FinancialChat.Consumer.Domain.DataContracts.Requests;

namespace Jobsity.Challenge.FinancialChat.Consumer.Interfaces.Gateways
{
    public interface IChatGateway
    {
        Task SendMessageAsyc(CommandMessageRequest request);
    }
}