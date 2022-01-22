using Jobsity.Challenge.FinancialChat.Domain.Entities;

namespace Jobsity.Challenge.FinancialChat.Infra.Repositories
{
    public class ConnectionsRepository
    {
        private readonly List<User> connections = new();

        public void Save(User user, string connectionId)
        {
            var currentUser = connections.FirstOrDefault(x => x.Name == user.Name);
            if (currentUser is null)
            {
                user.Id = Guid.NewGuid();
                connections.Add(user);
            }
            else
            {
                currentUser.SetConnection(connectionId);
            }
        }

        public List<User> GetAllUser()
        {
            return (from con in connections
                    select con
            ).ToList();
        }

        public User GetUserByConnectionId(string connectionId)
        {
            return (from con in connections where con.ConnectionId == connectionId select con).FirstOrDefault();
        }
    }
}