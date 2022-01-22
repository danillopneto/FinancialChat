using Jobsity.Challenge.FinancialChat.Domain.Entities;

namespace Jobsity.Challenge.FinancialChat.Infra.Repositories
{
    public class ChatRoomRepository
    {
        private readonly List<ChatRoom> rooms = new();

        public void AddUser(string roomName, User user)
        {
            var room = rooms.FirstOrDefault(r => r.Name == roomName);
            if (room is null)
            {
                rooms.Add(new ChatRoom(roomName, user));
            }
            else
            {
                room.Users.Add(user);
            }
        }

        public List<ChatRoom> GetAllRooms() => rooms;
    }
}