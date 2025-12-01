using Shared.Middleware.CQRS;
using Shared.Models;

namespace ChatService.Chat.Commands
{
    public class AddChatUserCommand : ICommand<NoResult>
    {
        public int ChatId { get; set; }
        public string UniqueName { get; set; }
        public HashSet<int> Privileges { get; set; }

        public AddChatUserCommand(int chatId, string uniqueName, HashSet<int> privileges)
        {
            ChatId = chatId;
            UniqueName = uniqueName;
            Privileges = privileges;
        }
    }
}
