using Jobsity.Challenge.FinancialChat.Domain.Entities;
using Jobsity.Challenge.FinancialChat.Infra.Configurations;
using Jobsity.Challenge.FinancialChat.Infra.Contexts;
using Jobsity.Challenge.FinancialChat.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Jobsity.Challenge.FinancialChat.Infra.Repositories
{
    public class ChatRoomRepository : IChatRoomRepository
    {
        private readonly ChatContext _chatContext;

        private readonly DataAppSettings _dataAppSettings;

        public ChatRoomRepository(
                                  ChatContext chatContext,
                                  DataAppSettings dataAppSettings)
        {
            _chatContext = chatContext ?? throw new ArgumentNullException(nameof(chatContext));
            _dataAppSettings = dataAppSettings ?? throw new ArgumentNullException(nameof(dataAppSettings));
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

        public async Task<IEnumerable<ChatRoom>> GetAllRooms() =>
            await _chatContext.Rooms.Include(m => m.Messages.OrderBy(t => t.Timestamp))
                                    .ThenInclude(s => s.Sender)
                                    .AsNoTracking()
                                    .Include(u => u.Users)
                                    .AsNoTracking()
                                    .ToListAsync();

        public async Task<ChatRoom> GetRoomById(Guid id) =>
            await _chatContext.Rooms.Where(r => r.Id == id)
                                    .Include(m => m.Messages)
                                    .Include(u => u.Users)
                                    .FirstOrDefaultAsync();
        
        public async Task<ChatRoom> GetRoomByName(string roomName) => 
            await _chatContext.Rooms.Where(r => r.Name == roomName)
                                    .Include(u => u.Users)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync();

        public async Task<ChatRoom> GetRoomByNameAndUser(string groupName, Guid userId) =>
            await _chatContext.Rooms.Include(r => r.Users)
                                    .Where(r => r.Name == groupName && r.Users.Any(u => u.Id == userId))
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync();

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

        public async Task SaveNewMessageAsync(ChatMessage chatMessage)
        {
            var room = await _chatContext.Rooms.Include(m => m.Messages.OrderBy(t => t.Timestamp))
                                               .FirstOrDefaultAsync(r => r.Id == chatMessage.Destination);
            while (room.Messages.Count >= _dataAppSettings.LimitOfMessagesPerRoom)
            {
                room.Messages.RemoveAt(0);
            }

            room.Messages.Add(chatMessage);
            _chatContext.Rooms.Update(room);

            await _chatContext.SaveChangesAsync();
        }
    }
}