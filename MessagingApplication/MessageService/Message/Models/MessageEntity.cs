namespace MessageService.Message.Models
{
    public class MessageEntity
    {
        public string Id { get; set; } = default!;
        public string SenderUniqueName { get; set; }
        public int ChatId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string? TextContent { get; set; }
        public string? QuotedId { get; set; }
        public Dictionary<string, List<string>> Reactions { get; set; } = new Dictionary<string, List<string>>(); // reaction, [users]
        public List<string> ImageUrls { get; set; } = new List<string>();
        public bool Deleted { get; set; }
        public DateTimeOffset DeletedAt { get; set; }

        public MessageEntity(string senderUniqueName, int chatId)
        {
            SenderUniqueName = senderUniqueName;
            ChatId = chatId;
            Deleted = false;
        }
    }
}
