using Jobsity.Challenge.FinancialChat.Domain.Dtos;

namespace Jobsity.Challenge.FinancialChat.Interfaces.UseCases
{
    public interface IDispatchCommandUseCase
    {
        Task DispatchAsync(ChatMessageDto chatMessage);
    }
}