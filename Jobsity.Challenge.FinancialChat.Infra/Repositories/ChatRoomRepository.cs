using Jobsity.Challenge.FinancialChat.Domain.Entities;
using Jobsity.Challenge.FinancialChat.Infra.Contexts;
using Jobsity.Challenge.FinancialChat.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Jobsity.Challenge.FinancialChat.Infra.Repositories
{
    public class ChatRoomRepository : IChatRoomRepository
    {
        private readonly ChatContext _chatContext;

        public ChatRoomRepository(ChatContext chatContext)
        {
            _chatContext = chatContext ?? throw new ArgumentNullException(nameof(chatContext));
        }

        public async Task<ChatRoom> AddUser(string roomName, User user)
        {
            var room = await _chatContext.Rooms.Include(r => r.Users)
                                               .FirstOrDefaultAsync(r => r.Name == roomName);
            if (room is null)
            {
                room = new ChatRoom(roomName, user);
                _chatContext.Rooms.Add(room);
            }
            else if (!room.Users.Any(x => x.Id == user.Id))
            {
                room.Users.Add(user);
                _chatContext.Rooms.Update(room);
            }

            await _chatContext.SaveChangesAsync();

            return room;
        }

        public async Task<IEnumerable<ChatRoom>> GetAllRooms() => await _chatContext.Rooms.AsNoTracking().Include(r => r.Users).ToListAsync();

        public async Task<ChatRoom> GetRoomById(Guid id)
        {
            return await _chatContext.Rooms.FindAsync(id);
        }

        public async Task<ChatRoom> GetRoomByName(string roomName)
        {
            return await _chatContext.Rooms.Include(r => r.Users)
                                           .Where(r => r.Name == roomName)
                                           .AsNoTracking()
                                           .FirstOrDefaultAsync();
        }

        public async Task<ChatRoom> GetRoomByNameAndUser(string groupName, Guid userId)
        {
            return await _chatContext.Rooms.Include(r => r.Users)
                                           .Where(r => r.Name == groupName && r.Users.Any(u => u.Id == userId))
                                           .AsNoTracking()
                                           .FirstOrDefaultAsync();
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
            var room = await GetRoomByName(roomName);
            if (room is not null)
            {
                room.Users.Remove(user);
            }

            await _chatContext.SaveChangesAsync();
        }
    }
}