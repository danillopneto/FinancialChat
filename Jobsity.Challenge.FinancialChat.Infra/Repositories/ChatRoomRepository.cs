using Jobsity.Challenge.FinancialChat.Domain.Entities;
using Jobsity.Challenge.FinancialChat.Interfaces.Repositories;

namespace Jobsity.Challenge.FinancialChat.Infra.Repositories
{
    public class ChatRoomRepository : IChatRoomRepository
    {
        private readonly List<ChatRoom> _rooms = new();

        public async Task<ChatRoom> AddUser(string roomName, User user)
        {
            var room = _rooms.FirstOrDefault(r => r.Name == roomName);
            if (room is null)
            {
                room = new ChatRoom(roomName, user);
                _rooms.Add(room);
            }
            else if (!room.Users.Any(x => x.Id == user.Id))
            {
                room.Users.Add(user);
            }

            return room;
        }

        public async Task<IEnumerable<ChatRoom>> GetAllRooms() => _rooms;

        public async Task<ChatRoom> GetRoomById(Guid id)
        {
            return _rooms.FirstOrDefault(r => r.Id == id);
        }

        public async Task<ChatRoom> GetRoomByName(string groupName)
        {
            return _rooms.FirstOrDefault(r => r.Name == groupName);
        }

        public async Task<bool> HasUser(string groupName, User user)
        {
            var room = await GetRoomByName(groupName);
            return room is not null &&
                   room.Users is not null &&
                   room.Users.Any(u => u.Id == user.Id);
        }

        public async Task RemoveUser(string roomName, User user)
        {
            var room = _rooms.FirstOrDefault(r => r.Name == roomName);
            if (room is not null)
            {
                room.Users.Remove(user);
            }
        }
    }
}