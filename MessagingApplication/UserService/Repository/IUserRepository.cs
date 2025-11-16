using UserService.Models;

namespace UserService.Repository
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByUniqueNameAsync(string uniqueName);
        Task CreateAsync(User user);
    }
}
