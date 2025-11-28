using MessageService.Models;

namespace MessageService.Repositories
{
    public interface IUserRepository
    {
        public Task<List<User>> GetManyAsync(IEnumerable<string> uniqueNames);
        public Task<User> GetAsync(string uniqueName);
        public Task<bool> ExistsAsync(string uniqueName);
        
        public Task CreateAsync(User user);
        public Task UpdateAsync(string uniqueName, string? displayName);
        public Task DeleteAsync(string uniqueName);
        
    }
}
