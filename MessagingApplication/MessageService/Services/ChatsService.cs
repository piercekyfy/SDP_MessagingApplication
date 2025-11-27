using MessageService.Exceptions;
using MessageService.Models;
using MessageService.Repositories;
using Shared.Exceptions;

namespace MessageService.Services
{
    public class ChatsService : IChatsService
    {
        private readonly IChatRepository charRepository;
        private readonly IUserRepository userRepository;

        public ChatsService(IChatRepository chatRepository, IUserRepository userRepository)
        {
            this.charRepository = chatRepository;
            this.userRepository = userRepository;
        }

        public async Task<List<Chat>> GetAllAsync()
        {
            return await charRepository.GetAllAsync();
        }

        public async Task<Chat> GetAsync(string chatId)
        {
            return await charRepository.GetAsync(chatId);
        }

        public async Task CreateAsync(Chat chat)
        {
            // A chat must be created with a single user.
            if (chat.Users.Count <= 0 || chat.Users.Count > 1)
                throw new DomainException("A chat must be created with a single user");

            if (!await userRepository.ExistsAsync(chat.Users[0].UniqueName)) 
                throw new UserNotFoundException(chat.Users[0].UniqueName);

            await charRepository.CreateAsync(chat);
        }
    }
}
