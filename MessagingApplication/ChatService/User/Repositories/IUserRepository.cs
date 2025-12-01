using ChatService.User.Models;

namespace ChatService.User.Repositories
{
    public interface IUserRepository
    {
        Task<UserModel?> GetByUniqueNameAsync(string uniqueName);
        Task CreateAsync(UserModel user);
        Task DeleteAsync(string uniqueName);
    }
}
