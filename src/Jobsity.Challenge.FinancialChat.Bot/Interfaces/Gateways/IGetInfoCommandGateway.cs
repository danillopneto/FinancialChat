using Jobsity.Challenge.FinancialChat.Bot.Infra.Configurations;

namespace Jobsity.Challenge.FinancialChat.Bot.Interfaces.Gateways
{
    public interface IGetInfoCommandGateway
    {
        Task<string> GetInfoAsync(AllowedCommandSettings settings, string commandParameter);
    }
}