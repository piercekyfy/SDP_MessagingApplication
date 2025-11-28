namespace MessageService.Models
{
    public class Message
    {
        public string Id { get; set; } = default!;
        public string SenderUniqueName { get; set; }
        public string ChatId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string? TextContent { get; set; }
        public string? QuotedId { get; set; }
        public Dictionary<string, List<string>> Reactions { get; set; } = new Dictionary<string, List<string>>(); // reaction, [users]
        public List<string> ImageUrls { get; set; } = new List<string>();
        public bool Deleted { get; set; }
        public DateTimeOffset DeletedAt { get; set; }

        public Message(string senderUniqueName, string chatId)
        {
            SenderUniqueName = senderUniqueName;
            ChatId = chatId;
            Deleted = false;
        }
    }
}
