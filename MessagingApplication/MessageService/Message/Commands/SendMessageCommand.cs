using Shared.Middleware.CQRS;
using System.Windows.Input;

namespace MessageService.Message.Commands
{
    public class SendMessageCommand : ICommand<string>
    {
        public int ChatId { get; set; }
        public string SenderUniqueName { get; set; }
        public string? TextContent { get; set; }
        public string? QuotedId { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();

        public SendMessageCommand(int chatId, string senderUniqueName)
        {
            ChatId = chatId;
            SenderUniqueName = senderUniqueName;
        }
    }
}
