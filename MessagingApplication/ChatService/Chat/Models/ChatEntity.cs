using ChatService.Chat.Models;

namespace ChatService.Chat.Models
{
    public class ChatEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        private readonly List<ChatUserEntity> users = new List<ChatUserEntity>();
        public IReadOnlyList<ChatUserEntity> Users => users.AsReadOnly();

        public ChatEntity(string name)
        {
            Name = name;
        }
    }
}
