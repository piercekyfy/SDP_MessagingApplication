using MessageService.User.Models;
using MessageService.User.Repositories;
using Shared.Messaging.Models.User;

namespace MessageService.User.Observers
{
    public class UserEventHandler
    {
        private readonly IUserRepository userRepository;

        public UserEventHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task HandleUserCreatedAsync(UserUpdated ev)
        {
            await userRepository.CreateAsync(new UserModel(ev.UniqueName, ev.DisplayName));
        }

        public async Task HandleUserUpdatedAsync(UserUpdated ev)
        {
            await userRepository.UpdateAsync(ev.UniqueName, ev.DisplayName);
        }

        public async Task HandleUserDeletedAsync(UserDeleted ev)
        {
            await userRepository.DeleteAsync(ev.UniqueName);
        }
    }
}