using Jobsity.Challenge.FinancialChat.Domain.Entities;

namespace Jobsity.Challenge.FinancialChat.Interfaces.Repositories
{
    public interface IUserConnectionRepository
    {
        Task<List<User>> GetAllUser();

        Task<User> GetUserByConnectionId(string connectionId);

        Task Save(User user);
    }
}