using Jobsity.Challenge.FinancialChat.Domain.Dtos;

namespace Jobsity.Challenge.FinancialChat.Interfaces.UseCases
{
    public interface ISaveUserUseCase
    {
        Task<UserDto> SaveUser(NewUserDto newUserDto, string connectionId);
    }
}