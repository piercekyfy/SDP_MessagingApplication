namespace MessageService.Models
{
    public class Message
    {
        public string? Id { get; set; }
        public string SenderUniqueName { get; set; }
        public string ChatId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Content { get; set; }

        public Message(string senderUniqueName, string chatId, string content)
        {
            SenderUniqueName = senderUniqueName;
            ChatId = chatId;
            Content = content;
        }
    }
}
