using Jobsity.Challenge.FinancialChat.Bot.Infra.Configurations;

namespace Jobsity.Challenge.FinancialChat.Bot.Interfaces.UseCases
{
    public interface IFilterCommandUseCase
    {
        (AllowedCommandSettings?, string) Filter(string command);
    }
}