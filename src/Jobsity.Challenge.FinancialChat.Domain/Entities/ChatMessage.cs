using System.ComponentModel.DataAnnotations;

namespace Jobsity.Challenge.FinancialChat.Domain.Entities
{
    public class ChatMessage
    {
        [Key]
        public Guid Id { get; set; }

        public Guid Destination { get; set; }

        public string Message { get; set; }

        public User Sender { get; set; }

        public DateTime Timestamp { get; set; }
    }
}