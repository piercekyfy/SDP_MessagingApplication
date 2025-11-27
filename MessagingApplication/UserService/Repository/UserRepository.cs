using Microsoft.EntityFrameworkCore;
using UserService.Contexts;
using UserService.Models;

namespace UserService.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UsersContext context;

        public UserRepository(UsersContext context)
        {
            this.context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<User?> GetByUniqueNameAsync(string uniqueName)
        {
            return await context.Users.FirstOrDefaultAsync(user => user.UniqueName == uniqueName.ToLower());
        }

        public async Task CreateAsync(User user)
        {
            user.DateCreated = DateTime.UtcNow;
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }
    }
}
