namespace Jobsity.Challenge.FinancialChat.Domain.Entities
{
    public class User
    {
        public string ConnectionId { get; set; }

        public DateTime DtConnection { get; set; }

        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}