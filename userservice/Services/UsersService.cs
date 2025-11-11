using UserService.Models;
using UserService.Repository;

namespace UserService.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUserRepository repository;

        public UsersService(IUserRepository repository) 
        { 
            this.repository = repository;
        }

        public async Task CreateAsync(User user)
        {
            user.DateCreated = DateTime.UtcNow;
            await repository.CreateAsync(user);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<User?> GetByUniqueNameAsync(string uniqueName)
        {
            return await repository.GetByUniqueNameAsync(uniqueName);
        }
    }
}
