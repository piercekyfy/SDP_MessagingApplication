using Shared.Messaging.Models;

namespace ChatService.Chat.Events
{
    public class ChatCreated : MessagingModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ChatCreated(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
