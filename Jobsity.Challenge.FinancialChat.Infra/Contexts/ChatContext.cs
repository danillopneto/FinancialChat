using Jobsity.Challenge.FinancialChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jobsity.Challenge.FinancialChat.Infra.Contexts
{
    public class ChatContext : DbContext
    {
        public ChatContext(DbContextOptions<ChatContext> options)
          : base(options)
        { }

        public DbSet<User> Users { get; set; }

        public DbSet<ChatRoom> Rooms { get; set; }
    }
}