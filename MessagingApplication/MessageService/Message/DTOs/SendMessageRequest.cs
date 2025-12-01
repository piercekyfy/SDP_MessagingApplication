namespace MessageService.Message.DTOs
{
    public class SendMessageRequest
    {
        public string SenderUniqueName { get; set; }
        public string? TextContent { get; set; }
        public string? QuotedId { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();

        public SendMessageRequest(string senderUniqueName)
        {
            SenderUniqueName = senderUniqueName;
        }
    }
}
