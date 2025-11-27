using Shared.Exceptions;

namespace MessageService.Exceptions
{
    public class InvalidChatUserException : DomainException
    {
        public string UniqueName { get; private set; }
        public string ChatId { get; private set; }

        public InvalidChatUserException(string uniqueName, string chatId) : base($"User ({uniqueName}) is not in Chat ({chatId}).")
        {
            UniqueName = uniqueName;
            ChatId = chatId;
        }
    }
}
