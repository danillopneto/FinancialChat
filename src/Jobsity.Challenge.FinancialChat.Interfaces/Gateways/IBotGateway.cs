using Jobsity.Challenge.FinancialChat.Domain.DataContracts.Requests;

namespace Jobsity.Challenge.FinancialChat.Interfaces.Gateways
{
    public interface IBotGateway
    {
        Task PublishCommandAsync(CommandRequest command);
    }
}