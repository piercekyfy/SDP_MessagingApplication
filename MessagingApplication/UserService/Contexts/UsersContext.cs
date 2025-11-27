using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UserService.Configurations;
using UserService.Models;

namespace UserService.Contexts
{
    public class UsersContext : DbContext
    {
        public DbSet<User> Users { get; set; }

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
    }
}
