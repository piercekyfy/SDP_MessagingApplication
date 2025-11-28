using MessageService.Models;
using MessageService.Repositories;
using Shared.Messaging.Models.User;
using System.Text.Json;

namespace MessageService.Services.Events
{
    public class UserEventHandler : IUserEventHandler
    {
        private readonly IUserRepository userRepository;

        public UserEventHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task HandleUserCreatedAsync(UserUpdated ev)
        {
            await userRepository.CreateAsync(new User(ev.UniqueName, ev.DisplayName));
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