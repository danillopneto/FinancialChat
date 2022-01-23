namespace Jobsity.Challenge.FinancialChat.Bot.Models
{
    public class CommandDto
    {
        public Guid Destination { get; set; }

        public string Message { get; set; }

        public Guid SenderId { get; set; }
    }
}
