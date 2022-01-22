using Jobsity.Challenge.FinancialChat.Domain.Dtos;

namespace Jobsity.Challenge.FinancialChat.Interfaces.UseCases
{
    public interface IAddToRoomUseCase
    {
        Task<ChatRoomDto?> AddAsync(string roomName, UserDto userDto);
    }
}