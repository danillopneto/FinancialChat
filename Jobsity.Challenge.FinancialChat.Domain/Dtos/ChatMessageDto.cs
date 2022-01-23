namespace Jobsity.Challenge.FinancialChat.Domain.Dtos
{
    public class ChatMessageDto
    {
        public ChatMessageDto()
        {
            Timestamp = DateTime.Now;
        }

        public ChatMessageDto(Guid destination, string message)
        {
            Destination = destination;
            Message = message;
            Timestamp = DateTime.Now;
        }

        public Guid Destination { get; set; }

        public string Message { get; set; }

        public UserDto Sender { get; set; }

        public DateTime Timestamp { get; set; }

        public bool IsCommand()
        {
            return !string.IsNullOrWhiteSpace(Message) && Message.StartsWith("/");
        }
    }
}