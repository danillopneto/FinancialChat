using Jobsity.Challenge.FinancialChat.Bot.Models;

namespace Jobsity.Challenge.FinancialChat.Bot.Interfaces.UseCases
{
    public interface IProcessCommandUseCase
    {
        Task ProcessAsync(CommandDto command, CancellationToken cancellationToken);
    }
}