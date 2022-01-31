using Jobsity.Challenge.FinancialChat.Bot.Domain.Dtos;
using Jobsity.Challenge.FinancialChat.Bot.Infra.Configurations;

namespace Jobsity.Challenge.FinancialChat.Bot.Interfaces.UseCases
{
    public interface IProcessCommandUseCase
    {
        Task ProcessAsync(CommandDto command, AllowedCommandSettings allowedCommand, CancellationToken cancellationToken);
    }
}