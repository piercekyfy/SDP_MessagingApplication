using MessageService.Chat.Models;
using MessageService.Message.Models;
using MessageService.User.Models;

namespace MessageService.Message.DTOs
{
    public class GetMessageResponse
    {
        public string? Id { get; set; }
        public string SenderUniqueName { get; set; }
        public int ChatId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string? TextContent { get; set; }
        public string? QuotedId { get; set; }
        public Dictionary<string, List<string>> Reactions { get; set; } = new Dictionary<string, List<string>>();
        public List<string> ImageUrls { get; set; } = new List<string>();


        public GetMessageResponse(string id, string senderUniqueName, int chatId, DateTimeOffset timestamp)
        {
            Id = id;
            SenderUniqueName = senderUniqueName;
            ChatId = chatId;
            Timestamp = timestamp;
        }
    }
}
