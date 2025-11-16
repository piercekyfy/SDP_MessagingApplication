namespace MessageService.Models
{
    public class Message
    {
        public string Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Content { get; set; }

        public Message(string id, string content)
        {
            Id = id;
            Content = content;
        }
    }
}
