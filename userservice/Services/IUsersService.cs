using UserService.Models;

namespace UserService.Services
{
    public interface IUsersService
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByUniqueNameAsync(string uniqueName);
        Task CreateAsync(User user);
    }
}
