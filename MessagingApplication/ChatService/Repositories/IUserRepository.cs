using ChatService.Models;

namespace ChatService.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByUniqueNameAsync(string uniqueName);
        Task CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(string uniqueName);
    }
}
