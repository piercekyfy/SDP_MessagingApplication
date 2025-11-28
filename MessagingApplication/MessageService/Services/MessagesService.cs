using MessageService.Assemblers;
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
            var relatedMessages = await messageRepository.GetManyAsync(messages.Where(m => m.QuotedId != null).Select(m => m.QuotedId!).Distinct());
            var relatedChats = await chatRepository.GetManyAsync(relatedMessages.Select(m => m.ChatId).Append(chat.Id).Distinct());

            var relatedUserIds =
                messages.Select(m => m.SenderUniqueName)
                .Concat(
                    relatedMessages.Select(m => m.SenderUniqueName)
                ).Concat(
                    messages.SelectMany(m => m.Reactions.Values).SelectMany(r => r)
                ).Where(s => !string.IsNullOrEmpty(s)).Distinct();

            var relatedUsers = await userRepository.GetManyAsync(relatedUserIds);

            var assembler = new GetMessageResponseAssembler();

            return assembler.AssembleMany(messages, relatedChats, relatedUsers, relatedMessages);
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

            using var client = new HttpClient();
            foreach (string imageUrl in request.ImageUrls)
            {
                try
                {
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
