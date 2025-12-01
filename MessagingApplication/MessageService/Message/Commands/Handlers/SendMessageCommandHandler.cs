using MessageService.Chat.Models;
using MessageService.Chat.Repositories;
using MessageService.Exceptions;
using MessageService.Message.Models;
using MessageService.Message.Models.Builders;
using MessageService.Message.Repositories;
using MessageService.User.Repositories;
using Shared.Exceptions;
using Shared.Middleware.CQRS;
using Shared.Models.Chat;

namespace MessageService.Message.Commands.Handlers
{
    public class SendMessageCommandHandler : ICommandHandler<SendMessageCommand, string>
    {
        private readonly IMessageRepository messageRepository;
        private readonly IChatRepository chatRepository;
        private readonly IUserRepository userRepository;
        public SendMessageCommandHandler(IMessageRepository messageRepository, IChatRepository chatRepository, IUserRepository userRepository)
        {
            this.messageRepository = messageRepository;
            this.chatRepository = chatRepository;
            this.userRepository = userRepository;
        }

        public async Task<string> Execute(SendMessageCommand command)
        {
            ChatModel chat = await chatRepository.GetAsync(command.ChatId);
            if (chat == null)
                throw new ChatNotFoundException(command.ChatId);

            if (await userRepository.GetAsync(command.SenderUniqueName) == null)
                throw new UserNotFoundException(command.SenderUniqueName);

            // User is in chat?
            ChatUserModel? chatUser = chat.Users.Where(u => u.UniqueName == command.SenderUniqueName).FirstOrDefault();
            if (chatUser == null)
                throw new InvalidChatUserException(command.SenderUniqueName, command.ChatId);

            // User has CanSend permission?
            if (!chatUser.Privileges.Contains(ChatUserPrivilege.CanSend) && !chatUser.Privileges.Contains(ChatUserPrivilege.IsAdmin))
                throw new ChatUserPermissionException(command.SenderUniqueName, command.ChatId, ChatUserPrivilege.CanSend);

            // Build Message
            MessageBuilder builder = new MessageBuilder(command.SenderUniqueName, command.ChatId).AddTextContent(command.TextContent);

            if (command.QuotedId != null)
            {
                MessageEntity quoted = await messageRepository.GetAsync(command.QuotedId);

                if (quoted == null)
                    throw new InvalidMessageContentException(nameof(quoted)) { DisplayMessage = "Quoted message does not exist." };

                if (quoted.ChatId != command.ChatId) // Check if the User is in and has forward privilege in quoted chat channel
                {
                    ChatModel origin = await chatRepository.GetAsync(quoted.ChatId);

                    ChatUserModel? originUser = origin.Users.FirstOrDefault(u => u.UniqueName == command.SenderUniqueName);

                    if (originUser == null)
                        throw new InvalidChatUserException(command.SenderUniqueName, origin.Id);

                    if (!originUser.Privileges.Contains(ChatUserPrivilege.CanForward))
                        throw new ChatUserPermissionException(command.SenderUniqueName, origin.Id, ChatUserPrivilege.CanForward);
                }

                builder.Quote(command.QuotedId);
            }

            // Images valid?
            using var client = new HttpClient();
            foreach (string imageUrl in command.ImageUrls)
            {
                try
                {
                    var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, imageUrl));
                    if (response.Content == null || response.Content.Headers == null || response.Content.Headers.ContentType == null || response.Content.Headers.ContentType.MediaType == null
                        || !response.IsSuccessStatusCode || !response.Content.Headers.ContentType.MediaType.StartsWith("image"))
                        throw new InvalidMessageContentException(nameof(imageUrl)) { DisplayMessage = $"Invalid Image: {imageUrl}" };
                }
                catch (Exception)
                {
                    throw new InvalidMessageContentException(nameof(imageUrl)) { DisplayMessage = $"Invalid Url: {imageUrl}" };
                }
                builder.AttachImage(imageUrl);
            }

            MessageEntity message = builder.Build();

            await messageRepository.CreateAsync(message);

            return message.Id;
        }
    }
}
