namespace Jobsity.Challenge.FinancialChat.Domain.Entities
{
    public class ChatMessage
    {
        public ChatMessage()
        {
        }

        public ChatMessage(Guid destination, string message)
        {
            Destination = destination;
            Message = message;
        }

        public ChatMessage(Guid destination, User sender, string message)
        {
            Destination = destination;
            Sender = sender;
            Message = message;
        }

        public Guid Destination { get; set; }

        public User Sender { get; set; }

        public string Message { get; set; }
    }
}