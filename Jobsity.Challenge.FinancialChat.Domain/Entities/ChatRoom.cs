namespace Jobsity.Challenge.FinancialChat.Domain.Entities
{
    public class ChatRoom
    {
        public ChatRoom(string name, User user)
        {
            Id = Guid.NewGuid();
            Name = name;
            Users = new List<User> { user };
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<User> Users { get; set; }
    }
}