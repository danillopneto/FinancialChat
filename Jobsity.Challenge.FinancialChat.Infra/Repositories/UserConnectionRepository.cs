using Jobsity.Challenge.FinancialChat.Domain.Entities;
using Jobsity.Challenge.FinancialChat.Interfaces.Repositories;

namespace Jobsity.Challenge.FinancialChat.Infra.Repositories
{
    public class UserConnectionRepository : IUserConnectionRepository
    {
        private readonly List<User> _connections = new();

        public async Task<List<User>> GetAllUser()
        {
            return (from con in _connections
                    select con
            ).ToList();
        }

        public async Task<User> GetUserByConnectionId(string connectionId)
        {
            return (from con in _connections where con.ConnectionId == connectionId select con).FirstOrDefault();
        }

        public async Task Save(User user)
        {
            var currentUser = _connections.FirstOrDefault(x => x.Name == user.Name);
            if (currentUser is null)
            {
                _connections.Add(user);
            }
            else
            {
                _connections.Remove(currentUser);
                var newUser = currentUser with { ConnectionId = user.ConnectionId };
                _connections.Add(newUser);
            }
        }
    }
}