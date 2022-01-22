using Jobsity.Challenge.FinancialChat.Domain.Entities;

namespace Jobsity.Challenge.FinancialChat.Infra.Repositories
{
    public class ChatRoomRepository
    {
        private readonly List<ChatRoom> rooms = new();

        public ChatRoom AddUser(string roomName, User user)
        {
            var room = rooms.FirstOrDefault(r => r.Name == roomName);
            if (room is null)
            {
                room = new ChatRoom(roomName, user);
                rooms.Add(room);
            }
            else if (!room.Users.Any(x => x.Id == user.Id))
            {
                room.Users.Add(user);
            }

            return room;
        }

        public List<ChatRoom> GetAllRooms() => rooms;

        public void RemoveUser(string roomName, User user)
        {
            var room = rooms.FirstOrDefault(r => r.Name == roomName);
            if (room is not null)
            {
                room.Users.Remove(user);
            }
        }

        public ChatRoom GetRoomById(Guid id)
        {
            return rooms.FirstOrDefault(r => r.Id == id);
        }

        public ChatRoom GetRoomByName(string groupName)
        {
            return rooms.FirstOrDefault(r => r.Name == groupName);
        }

        public bool? HasUser(string groupName, User user)
        {
            return GetRoomByName(groupName)?.Users?.Any(u => u.Id == user.Id);
        }
    }
}