using MessageService.Exceptions;
using MessageService.Models;
using MessageService.Repositories;
using Shared.Exceptions;

namespace MessageService.Services
{
    public class MessagesService : IMessagesService
    {
        private readonly IMessageRepository messageRepository;
        private readonly IChatRepository chatRepository;
        private readonly IUserRepository userRepository;
        

        public MessagesService(IMessageRepository messageRepository, IChatRepository chatRepository, IUserRepository userRepository) 
        { 
            this.messageRepository = messageRepository;
            this.chatRepository = chatRepository;
            this.userRepository = userRepository;
        }

        public async Task<List<Message>> GetAllByChatAsync(string chatId)
        {
            if(!await chatRepository.ExistsAsync(chatId))
                throw new ChatNotFoundException(chatId);

            return await messageRepository.GetAllByChatAsync(chatId);
        }

        public async Task SendAsync(Message message)
        {
            // User exists?
            if (!await userRepository.ExistsAsync(message.SenderUniqueName))
                throw new DomainException($"Message Sender ({message.SenderUniqueName}) does not exist.");

            // Chat exists?
            Chat chat = await chatRepository.GetAsync(message.ChatId);
            if (chat == null)
                throw new ChatNotFoundException(message.ChatId);

            // User is in chat?
            ChatUser? chatUser = chat.Users.FirstOrDefault(u => u.UniqueName == message.SenderUniqueName);
            if (chatUser == null)
                throw new InvalidChatUserException(message.SenderUniqueName, message.ChatId);

            // User has CanSend permission?
            if(!chatUser.Permissions.Contains(ChatUserPrivilege.CanSend) && !chatUser.Permissions.Contains(ChatUserPrivilege.IsAdmin))
                throw new ChatUserPermissionException(message.SenderUniqueName, message.ChatId, ChatUserPrivilege.CanSend);

            await messageRepository.CreateAsync(message);
        }
    }
}
