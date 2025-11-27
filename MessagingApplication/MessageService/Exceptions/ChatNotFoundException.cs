using Shared.Exceptions;

namespace MessageService.Exceptions
{
    public class ChatNotFoundException : DomainException
    {
        public string ChatId { get; private set; }

        public ChatNotFoundException(string chatId) : base ($"Chat ({chatId}) not found.")
        {
            ChatId = chatId;
        }
    }
}
