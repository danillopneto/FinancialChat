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
            MapUser(modelBuilder);

            MapChatMessage(modelBuilder);
        }

        private static void MapChatMessage(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatMessage>()
                            .HasOne(c => c.Sender);
        }

        private static void MapUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                            .HasMany(c => c.ChatRooms)
                            .WithMany(e => e.Users);
        }
    }
}