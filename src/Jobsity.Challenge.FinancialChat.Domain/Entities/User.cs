using System.ComponentModel.DataAnnotations;

namespace Jobsity.Challenge.FinancialChat.Domain.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public string ConnectionId { get; set; }

        public DateTime DtConnection { get; set; }

        public string Name { get; set; }

        public IEnumerable<ChatRoom> ChatRooms { get; set; }
    }
}