using MessageService.Models;

namespace MessageService.Repositories
{
    public interface IUserRepository
    {
        public Task<User> GetUserAsync(string uniqueName);
        public Task CreateUserAsync(User user);
        public Task UpdateUserAsync(string uniqueName, string? displayName);
        public Task<bool> ExistsAsync(string uniqueName);
    }
}
