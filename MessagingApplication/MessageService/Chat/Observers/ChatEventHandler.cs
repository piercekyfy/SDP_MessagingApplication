using ChatService.Chat.Events;
using MessageService.Chat.Repositories;
using Shared.Messaging.Models.User;
using MessageService.Chat.Models;
using Shared.Models.Chat;

namespace MessageService.Chat.Observers
{
    public class ChatEventHandler
    {
        private readonly IChatRepository chatRepository;

        public ChatEventHandler(IChatRepository userRepository)
        {
            this.chatRepository = userRepository;
        }

        public async Task HandleChatCreatedAsync(ChatCreated ev)
        {
            await chatRepository.CreateAsync(new ChatModel(ev.Id, ev.Name));
        }

        public async Task HandleUserJoinedAsync(UserJoinedChat ev)
        {
            await chatRepository.AddUserAsync(ev.ChatId, new ChatUserModel(ev.UniqueName, new(ev.Privileges.Select(p => (ChatUserPrivilege)p).ToArray())));
        }
    }
}