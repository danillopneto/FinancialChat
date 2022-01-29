using Jobsity.Challenge.FinancialChat.Domain.Dtos;

namespace Jobsity.Challenge.FinancialChat.Interfaces.UseCases
{
    public interface ISaveMessageIntoRoomUseCase
    {
        Task SaveAsync(ChatMessageDto chatMessageDto);
    }
}