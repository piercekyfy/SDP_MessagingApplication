using UserService.Models;

namespace UserService.Services
{
    public interface IUserExchangeService
    {
        public Task PublishUserCreatedAsync(User user);
        public Task PublishUserUpdatedAsync(User user);
    }
}
