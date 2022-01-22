using Jobsity.Challenge.FinancialChat.Domain.Entities;

namespace Jobsity.Challenge.FinancialChat.Interfaces.Repositories
{
    public interface IChatRoomRepository
    {
        Task<ChatRoom> AddUser(string roomName, User user);

        Task<IEnumerable<ChatRoom>> GetAllRooms();

        Task<ChatRoom> GetRoomById(Guid id);

        Task<ChatRoom> GetRoomByName(string groupName);

        Task<bool> HasUser(string groupName, User user);

        Task RemoveUser(string roomName, User user);
    }
}