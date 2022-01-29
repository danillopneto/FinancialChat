using Jobsity.Challenge.FinancialChat.Domain.Dtos;

namespace Jobsity.Challenge.FinancialChat.Interfaces.UseCases
{
    public interface IGetUserUseCase
    {
        Task<UserDto> GetUserByConnectionId(string connectionId);

        Task<UserDto> GetUserById(Guid id);
    }
}