using Shared.Exceptions;
using MessageService.Models;

namespace MessageService.Exceptions
{
    public class ChatUserPermissionException : DomainException
    {
        public string UniqueName { get; private set; }
        public string ChatId { get; private set; }

        public ChatUserPrivilege Expected { get; private set; }

        public ChatUserPermissionException(string uniqueName, string chatId, ChatUserPrivilege expected) : base($"User ({uniqueName}) in Chat ({chatId}) requires privilege ({expected}).")
        {
            UniqueName = uniqueName;
            ChatId = chatId;
            Expected = expected;
        }
    }
}
