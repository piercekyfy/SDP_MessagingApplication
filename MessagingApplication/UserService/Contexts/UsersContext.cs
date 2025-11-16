using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Contexts
{
    public class UsersContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        private string? connectionString;

        public UsersContext(IConfiguration configuration)
        {
            connectionString = configuration["PostgresSQL:ConnectionString"];
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention();
        }
    }
}
