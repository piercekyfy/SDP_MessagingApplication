using MessageService.Message.Models;

namespace MessageService.Message.Models.Builders
{
    public class MessageBuilder
    {
        private string senderUniqueName;
        private int chatId;
        private string? textContent;
        private string? quotedId;
        private Dictionary<string, List<string>> reactionIds { get; set; } = new Dictionary<string, List<string>>();
        private List<string> imageUrls { get; set; } = new List<string>();

        public MessageBuilder(string senderUniqueName, int chatId)
        {
            this.senderUniqueName = senderUniqueName;
            this.chatId = chatId;
            textContent = string.Empty;
        }

        public bool HasContent()
        {
            return textContent != null || imageUrls.Count > 0;
        }

        public MessageEntity Build()
        {
            var model = new MessageEntity(senderUniqueName, chatId) { TextContent = textContent, QuotedId = quotedId, Reactions = reactionIds, ImageUrls = imageUrls };
            model.Timestamp = DateTimeOffset.UtcNow;
            return model;
        }

        public void Reset(string senderUniqueName, string chatId)
        {
            textContent = string.Empty;
            quotedId = null;
            reactionIds.Clear();
            imageUrls.Clear();
        }

        public MessageBuilder AddTextContent(string? content)
        {
            textContent = content;
            return this;
        }

        public MessageBuilder Quote(string messageId)
        {
            quotedId = messageId;
            return this;
        }

        public MessageBuilder React(string reactionId, string sourceUniqueName)
        {
            if (reactionIds.TryGetValue(reactionId, out var sources))
                if (!sources.Contains(sourceUniqueName)) sources.Add(sourceUniqueName);
            else
                reactionIds[reactionId] = new List<string>() { sourceUniqueName };

            return this;
        }

        public MessageBuilder AttachImage(string url)
        {
            if(!imageUrls.Contains(url))
                imageUrls.Add(url);
            return this;
        }
    }
}
