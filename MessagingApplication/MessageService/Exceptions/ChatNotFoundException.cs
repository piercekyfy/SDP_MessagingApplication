using Shared.Exceptions;

namespace MessageService.Exceptions
{
    public class ChatNotFoundException : DomainException
    {
        public int ChatId { get; private set; }

        public ChatNotFoundException(int chatId) : base ($"Chat ({chatId}) not found.")
        {
            ChatId = chatId;
        }
    }
}
