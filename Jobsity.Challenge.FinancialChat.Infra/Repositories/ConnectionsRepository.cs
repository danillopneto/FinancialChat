using Jobsity.Challenge.FinancialChat.Domain.Entities;

namespace Jobsity.Challenge.FinancialChat.Infra.Repositories
{
    public class ConnectionsRepository
    {
        private readonly Dictionary<string, User> connections = new();

        public void Add(string uniqueID, User user)
        {
            if (!connections.ContainsKey(uniqueID))
                connections.Add(uniqueID, user);
        }

        public List<User> GetAllUser()
        {
            return (from con in connections
                    select con.Value
            ).ToList();
        }

        public User GetUser(string connectionId)
        {
            return (from con in connections where con.Key == connectionId select con.Value).FirstOrDefault();
        }

        public string GetUserId(long id)
        {
            return (from con in connections
                    where con.Value.Key == id
                    select con.Key).FirstOrDefault();
        }
    }
}