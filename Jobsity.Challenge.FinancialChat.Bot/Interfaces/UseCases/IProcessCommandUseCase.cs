using Jobsity.Challenge.FinancialChat.Bot.Infra.Configurations;
using Jobsity.Challenge.FinancialChat.Bot.Domain.Dtos;

namespace Jobsity.Challenge.FinancialChat.Bot.Interfaces.UseCases
{
    public interface IProcessCommandUseCase
    {
        Task ProcessAsync(CommandDto command, AllowedCommandSettings allowedCommand, CancellationToken cancellationToken);
    }
}