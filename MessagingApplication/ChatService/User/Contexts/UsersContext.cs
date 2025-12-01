using ChatService.Configurations;
using ChatService.User.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ChatService.User.Contexts
{
    public class UsersContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }

        private string? connectionString;

        public UsersContext(IOptions<PostgresSQLConfiguration> configuration)
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
            modelBuilder.Entity<UserModel>().ToTable("users");
            modelBuilder.Entity<UserModel>().HasKey(u => u.UniqueName);
        }
    }
}
