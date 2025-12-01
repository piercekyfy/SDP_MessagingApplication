using ChatService.User.Models;
using ChatService.User.Repositories;
using Shared.Messaging.Models.User;
using System.Text.Json;

namespace MessageService.Services.Events
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
            await userRepository.CreateAsync(new UserModel(ev.UniqueName));
        }

        public async Task HandleUserDeletedAsync(UserDeleted ev)
        {
            await userRepository.DeleteAsync(ev.UniqueName);
        }
    }
}