using System.ComponentModel.DataAnnotations;

namespace Jobsity.Challenge.FinancialChat.Domain.Entities
{
    public class ChatRoom
    {
        public ChatRoom()
        {
        }

        public ChatRoom(string name, User user)
        {
            Id = Guid.NewGuid();
            Name = name;
            Users = new List<User> { user };
        }

        [Key]
        public Guid Id { get; set; }

        public List<ChatMessage> Messages { get; set; }

        public string Name { get; set; }

        public List<User> Users { get; set; }
    }
}