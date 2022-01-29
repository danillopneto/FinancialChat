using Jobsity.Challenge.FinancialChat.Domain.Dtos;

namespace Jobsity.Challenge.FinancialChat.Interfaces.UseCases
{
    public interface IGetRoomUseCase
    {
        Task<IEnumerable<ChatRoomDto>> GetAll();

        Task<ChatRoomDto> GetByNameAndUser(string groupName, Guid userId);
    }
}