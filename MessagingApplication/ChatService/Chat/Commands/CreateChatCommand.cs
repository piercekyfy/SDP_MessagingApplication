using Shared.Middleware.CQRS;

namespace ChatService.Chat.Commands
{
    /// <summary>
    /// Create a chat room, add creator as chat user with all privileges (owner).
    /// </summary>
    public class CreateChatCommand : ICommand<int>
    {
        public string Name { get; set; }
        public string CreatorUniqueName { get; set; }

        public CreateChatCommand(string name, string creatorUniqueName)
        {
            Name = name;
            CreatorUniqueName = creatorUniqueName;
        }
    }
}
