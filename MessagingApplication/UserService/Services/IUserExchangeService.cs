using UserService.Models;

namespace UserService.Services
{
    public interface IUserExchangeService
    {
        public Task PublishCreatedAsync(User user);
        public Task PublishUpdatedAsync(User user);
        public Task PublishDeletedAsync(User user);
    }
}
