namespace Jobsity.Challenge.FinancialChat.Domain.Entities
{
    public class ChatMessage
    {
        public long Destination { get; set; }

        public User Sender { get; set; }

        public string Message { get; set; }
    }
}