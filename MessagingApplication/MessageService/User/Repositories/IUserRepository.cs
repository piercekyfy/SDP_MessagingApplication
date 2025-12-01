using MessageService.User.Models;

namespace MessageService.User.Repositories
{
    public interface IUserRepository
    {
        public Task<List<UserModel>> GetManyAsync(IEnumerable<string> uniqueNames);
        public Task<UserModel> GetAsync(string uniqueName);
        public Task<bool> ExistsAsync(string uniqueName);
        
        public Task CreateAsync(UserModel user);
        public Task UpdateAsync(string uniqueName, string? displayName);
        public Task DeleteAsync(string uniqueName);
        
    }
}
