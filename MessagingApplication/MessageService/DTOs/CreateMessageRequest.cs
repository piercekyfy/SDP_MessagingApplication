namespace MessageService.DTOs
{
    public class CreateMessageRequest
    {
        public string SenderUniqueName { get; set; }
        public string ChatId { get; set; }
        public string Content { get; set; }

        public CreateMessageRequest(string senderUniqueName, string chatId, string content)
        {
            SenderUniqueName = senderUniqueName;
            ChatId = chatId;
            Content = content;
        }
    }
}
