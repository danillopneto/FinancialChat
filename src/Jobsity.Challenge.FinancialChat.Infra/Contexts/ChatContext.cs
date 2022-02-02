using Jobsity.Challenge.FinancialChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jobsity.Challenge.FinancialChat.Infra.Contexts
{
    public class ChatContext : DbContext
    {
        public DbSet<ChatMessage> ChatMessages { get; set; }

        public DbSet<ChatRoom> Rooms { get; set; }

        public DbSet<User> Users { get; set; }

        public ChatContext(DbContextOptions<ChatContext> options)
          : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(c => c.ChatRooms)
                .WithMany(e => e.Users);

            modelBuilder.Entity<ChatMessage>()
                .HasOne(c => c.Sender);
        }
    }
}