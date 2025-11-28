using MessageService.Models;

namespace MessageService.DTOs
{
    public struct GetMessageResponseUser
    {
        public string? UniqueName { get; set; }
        public string DisplayName { get; set; }

        public GetMessageResponseUser(string? uniqueName, string displayName)
        {
            UniqueName = uniqueName;
            DisplayName = displayName;
        }
    }

    public struct GetMessageResponseChat
    {
        public string ChatId { get; set; }
        public string Name { get; set; }

        public GetMessageResponseChat(string chatId, string name)
        {
            ChatId = chatId;
            Name = name;
        }
    }

    public class GetMessageResponse
    {
        public string? Id { get; set; }
        public GetMessageResponseUser Sender { get; set; }
        public GetMessageResponseChat Chat { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string? TextContent { get; set; }
        public GetMessageResponse? QuotedMessage { get; set; }
        public Dictionary<string, List<GetMessageResponseUser>> Reactions { get; set; } = new Dictionary<string, List<GetMessageResponseUser>>();
        public List<string> ImageUrls { get; set; } = new List<string>();

        public GetMessageResponse(Message message, User sender, Chat chat)
        {
            if (chat.Id == null)
                throw new ArgumentNullException(nameof(chat.Id));

            Id = message.Id;
            Sender = new GetMessageResponseUser(sender.Deleted ? null : sender.UniqueName, sender.Deleted ? "Deleted User" : sender.DisplayName);
            Chat = new GetMessageResponseChat(chat.Id, chat.Name);
            Timestamp = message.Timestamp;
            TextContent = message.TextContent;
        }

        public GetMessageResponse(Message message, Chat chat)
        {
            if (chat.Id == null)
                throw new ArgumentNullException(nameof(chat.Id));

            Id = message.Id;
            Sender = new GetMessageResponseUser(null, "Deleted User");
            Chat = new GetMessageResponseChat(chat.Id, chat.Name);
            Timestamp = message.Timestamp;
            TextContent = message.TextContent;
        }
    }
}
