using UserService.Models;

namespace UserService.Services
{
    public interface IUsersExchangeService
    {
        public Task PublishUserCreated(User user);
    }
}
