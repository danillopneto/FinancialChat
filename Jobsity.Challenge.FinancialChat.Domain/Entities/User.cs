namespace Jobsity.Challenge.FinancialChat.Domain.Entities
{
    public class User
    {
        public DateTime DtConnection { get; set; }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ConnectionId { get; private set; }

        public void SetConnection(string connectionId)
        {
            ConnectionId = connectionId;
        }
    }
}