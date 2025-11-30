namespace ChatService.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        private readonly List<ChatUser> users = new List<ChatUser>();
        public IReadOnlyList<ChatUser> Users => users.AsReadOnly();

        public Chat(string name, DateTimeOffset createdAt)
        {
            Name = name;
            CreatedAt = createdAt;
        }
    }
}
