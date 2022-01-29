using Jobsity.Challenge.FinancialChat.Domain.Entities;
using Jobsity.Challenge.FinancialChat.Infra.Contexts;
using Jobsity.Challenge.FinancialChat.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Jobsity.Challenge.FinancialChat.Infra.Repositories
{
    public class UserConnectionRepository : IUserConnectionRepository
    {
        private readonly ChatContext _chatContext;

        public UserConnectionRepository(ChatContext chatContext)
        {
            _chatContext = chatContext ?? throw new ArgumentNullException(nameof(chatContext));
        }

        public async Task<List<User>> GetAllUser() => await _chatContext.Users.ToListAsync();

        public async Task<User> GetUser(Guid id)
        {
            return await _chatContext.Users.FindAsync(id);
        }

        public async Task<User> GetUserByConnectionId(string connectionId)
        {
            return (from con in _chatContext.Users where con.ConnectionId == connectionId select con).AsNoTracking().FirstOrDefault();
        }

        public async Task Save(User user)
        {
            var currentUser = await _chatContext.Users.Where(x => x.Name == user.Name).FirstOrDefaultAsync();
            if (currentUser is null)
            {
                await _chatContext.Users.AddAsync(user);
            }
            else
            {
                currentUser.ConnectionId = user.ConnectionId;
                _chatContext.Users.Update(currentUser);
                user.Id = currentUser.Id;
            }

            await _chatContext.SaveChangesAsync();
        }
    }
}