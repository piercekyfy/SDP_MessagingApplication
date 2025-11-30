using ChatService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ChatService.Configurations;

namespace ChatService.Contexts
{
    public class ChatsContext : DbContext
    {
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatUser> ChatUsers { get; set; }
        public DbSet<ChatUserPrivilege> ChatUserPrivileges { get; set; }

        private string? connectionString;

        public ChatsContext(IOptions<PostgresSQLConfiguration> configuration)
        {
            connectionString = configuration.Value.ConnectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>()
                .HasMany(c => c.Users)
                .WithOne()
                .HasForeignKey(cu => cu.ChatId);

            modelBuilder.Entity<ChatUser>(entity => {
                entity.Property(cu => cu.UserUniqueName).HasColumnName("user_uname");
                entity.HasMany<ChatUserPrivilege>().WithOne().HasForeignKey(cup => cup.ChatId);
            });
        }
    }
}
