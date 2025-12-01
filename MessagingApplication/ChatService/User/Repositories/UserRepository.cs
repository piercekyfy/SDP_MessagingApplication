using Microsoft.EntityFrameworkCore;
using ChatService.User.Models;
using ChatService.User.Contexts;

namespace ChatService.User.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UsersContext context;

        public UserRepository(UsersContext context)
        {
            this.context = context;
        }

        public async Task<List<UserModel>> GetAllAsync()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<UserModel?> GetByUniqueNameAsync(string uniqueName)
        {
            return await context.Users.FirstOrDefaultAsync(user => user.UniqueName == uniqueName.ToLower());
        }

        public async Task CreateAsync(UserModel user)
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string uniqueName)
        {
            await context.Users.Where(u => u.UniqueName == uniqueName).ExecuteDeleteAsync();
            await context.SaveChangesAsync();
        }
    }
}
