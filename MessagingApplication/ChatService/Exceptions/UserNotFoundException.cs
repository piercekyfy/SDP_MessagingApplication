using Shared.Exceptions;

namespace ChatService.Exceptions
{
    public class ChatNotFoundException : DomainException
    {
        public int Id { get; private set; }

        public ChatNotFoundException(int id) : base ($"Chat ({id}) not found.")
        {
            Id = id;
        }
    }
}
