namespace Jobsity.Challenge.FinancialChat.Domain.Dtos
{
    public class UserDto
    {
        public string ConnectionId { get; set; }

        public DateTime DtConnection { get; set; }

        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}