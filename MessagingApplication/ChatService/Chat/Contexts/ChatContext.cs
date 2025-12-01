using ChatService.Chat.Models;
using ChatService.Configurations;
using ChatService.User.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ChatService.Chat.Contexts
{
    public class ChatContext : DbContext
    {
        public DbSet<ChatEntity> Chats { get; set; }
        public DbSet<ChatUserEntity> ChatUsers { get; set; }
        public DbSet<ChatUserPrivilegeModel> ChatUserPrivileges { get; set; }


        private readonly string? connectionString;

        public ChatContext(IOptions<PostgresSQLConfiguration> configuration)
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
            modelBuilder.Entity<ChatEntity>().ToTable("chats");
            modelBuilder.Entity<ChatUserEntity>().ToTable("chat_users");
            modelBuilder.Entity<ChatUserPrivilegeModel>().ToTable("chat_user_privileges");

            modelBuilder.Entity<ChatEntity>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.HasMany(c => c.Users)
                      .WithOne()
                      .HasForeignKey(cu => cu.ChatId);
            });

            modelBuilder.Entity<ChatUserEntity>(entity =>
            {
                entity.HasKey(cu => new { cu.ChatId, cu.UserUniqueName });

                entity.Property(cu => cu.UserUniqueName)
                      .HasColumnName("user_uname");

                entity.HasMany(cu => cu.Privileges)
                      .WithOne()
                      .HasForeignKey(p => new { p.ChatId, p.UserUniqueName }); 
            });

            modelBuilder.Entity<ChatUserPrivilegeModel>(entity =>
            {
                entity.HasKey(p => new { p.ChatId, p.UserUniqueName, p.Privilege });

                entity.Property(p => p.UserUniqueName)
                    .HasColumnName("user_uname");
            });
        }
    }
}
