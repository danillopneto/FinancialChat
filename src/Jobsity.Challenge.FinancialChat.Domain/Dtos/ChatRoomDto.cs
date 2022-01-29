namespace Jobsity.Challenge.FinancialChat.Domain.Dtos
{
    public class ChatRoomDto
    {
        public Guid Id { get; set; }

        public List<ChatMessageDto> Messages { get; set; }

        public string Name { get; set; }

        public List<UserDto> Users { get; set; }
    }
}