using MessageService.DTOs;
using MessageService.Exceptions;
using MessageService.Models;
using MessageService.Models.Builders;
using MessageService.Repositories;
using Shared.Exceptions;
using System;
using System.Security.Cryptography;

namespace MessageService.Services
{
    public class MessagesService : IMessagesService
    {
        private readonly IMessageRepository messageRepository;
        private readonly IChatRepository chatRepository;
        private readonly IUserRepository userRepository;
        

        public MessagesService(IMessageRepository messageRepository, IChatRepository chatRepository, IUserRepository userRepository) 
        { 
            this.messageRepository = messageRepository;
            this.chatRepository = chatRepository;
            this.userRepository = userRepository;
        }

        public async Task<List<GetMessageResponse>> GetAllByChatAsync(string chatId)
        {
            Chat chat = await chatRepository.GetAsync(chatId);
            if(chat == null)
                throw new ChatNotFoundException(chatId);
            
            var messages = await messageRepository.GetAllByChatAsync(chatId);

            var quotedMessages = await messageRepository.GetManyAsync(messages.Where(m => m.QuotedId != null).Select(m => m.QuotedId!).Distinct());

            var relatedUsersIds =
                messages.Select(m => m.SenderUniqueName)
                .Concat(
                    quotedMessages.Select(m => m.SenderUniqueName)
                ).Concat(
                    messages.SelectMany(m => m.Reactions.Values).SelectMany(r => r)
                ).Where(s => !string.IsNullOrEmpty(s)).Distinct(); // There is a lot of merit to only fetching reaction details when they're inspected by the user.

            var users = (await userRepository.GetManyAsync(relatedUsersIds)).ToDictionary(u => u.UniqueName);
            var quotedMessagesDict = quotedMessages.ToDictionary(m => m.Id);

            List<GetMessageResponse> responses = new List<GetMessageResponse>();

            foreach(Message message in messages)
            {
                users.TryGetValue(message.SenderUniqueName, out var user);
                GetMessageResponse response = (user == null ? new GetMessageResponse(message, chat) : new GetMessageResponse(message, user, chat));

                if (message.QuotedId != null && quotedMessagesDict.TryGetValue(message.QuotedId, out var quotedMessage))
                {
                    Chat quotedChat = await chatRepository.GetAsync(quotedMessage.ChatId);
                    if(quotedChat != null)
                    {
                        users.TryGetValue(quotedMessage.SenderUniqueName, out var quotedUser);
                        GetMessageResponse quotedResponse = (user == null ? new GetMessageResponse(quotedMessage, quotedChat) : new GetMessageResponse(quotedMessage, quotedUser, quotedChat));
                        response.QuotedMessage = quotedResponse;
                    }
                }

                foreach(KeyValuePair<string, List<string>> kvp in message.Reactions)
                {
                    List<GetMessageResponseUser> sources = new List<GetMessageResponseUser>();
                    foreach (string uniqueName in kvp.Value)
                        if (users.TryGetValue(uniqueName, out var source))
                            sources.Add(new GetMessageResponseUser(source.UniqueName, source.DisplayName));
                    response.Reactions.Add(kvp.Key, sources);
                }

                response.ImageUrls = message.ImageUrls;

                responses.Add(response);
            }

            return responses;
        }

        public async Task<Message> SendAsync(string chatId, SendMessageRequest request)
        {
            // Chat exists?
            Chat chat = await chatRepository.GetAsync(chatId);
            if (chat == null)
                throw new ChatNotFoundException(chatId);

            // User exists?
            if (!await userRepository.ExistsAsync(request.SenderUniqueName))
                throw new DomainException($"Message Sender ({request.SenderUniqueName}) does not exist.");

            // User is in chat?
            ChatUser? chatUser = chat.Users.FirstOrDefault(u => u.UniqueName == request.SenderUniqueName);
            if (chatUser == null)
                throw new InvalidChatUserException(request.SenderUniqueName, chatId);

            // User has CanSend permission?
            if(!chatUser.Permissions.Contains(ChatUserPrivilege.CanSend) && !chatUser.Permissions.Contains(ChatUserPrivilege.IsAdmin))
                throw new ChatUserPermissionException(request.SenderUniqueName, chatId, ChatUserPrivilege.CanSend);

            // Build Message
            MessageBuilder builder = new MessageBuilder(request.SenderUniqueName, chatId).AddTextContent(request.TextContent);

            if(request.QuotedId != null)
            {
                Message quoted = await messageRepository.GetAsync(request.QuotedId);

                if (quoted == null)
                    throw new InvalidMessageContentException(nameof(quoted)) { DisplayMessage = "Quoted message does not exist." };
                
                if(quoted.ChatId != chatId) // Check if the User is in and has forward privilege in quoted chat channel
                {
                    Chat origin = await chatRepository.GetAsync(quoted.ChatId);

                    ChatUser? originUser = origin.Users.FirstOrDefault(u => u.UniqueName == request.SenderUniqueName);

                    if (originUser == null)
                        throw new InvalidChatUserException(request.SenderUniqueName, origin.Id);

                    if (!originUser.Permissions.Contains(ChatUserPrivilege.CanForward))
                        throw new ChatUserPermissionException(request.SenderUniqueName, origin.Id, ChatUserPrivilege.CanForward);
                }

                builder.Quote(request.QuotedId);
            }

            foreach(string imageUrl in request.ImageUrls)
            {
                try
                {
                    using var client = new HttpClient();
                    var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, imageUrl));
                    if (response.Content == null || response.Content.Headers == null || response.Content.Headers.ContentType == null || response.Content.Headers.ContentType.MediaType == null
                        || !response.IsSuccessStatusCode || !response.Content.Headers.ContentType.MediaType.StartsWith("image"))
                        throw new InvalidMessageContentException(nameof(imageUrl)) { DisplayMessage = $"Invalid Image: {imageUrl}" };
                } catch (Exception)
                {
                    throw new InvalidMessageContentException(nameof(imageUrl)) { DisplayMessage = $"Invalid Url: {imageUrl}" };
                }
                builder.AttachImage(imageUrl);
            }

            Message message = builder.Build();

            return await messageRepository.CreateAsync(message);
        }
    }
}
