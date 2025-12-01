using MessageService.Chat.Repositories;
using MessageService.Exceptions;
using MessageService.Message.DTOs;
using MessageService.Message.Repositories;
using MessageService.User.Repositories;
using Shared.Middleware.CQRS;
using System;

namespace MessageService.Message.Queries.Handlers
{
    public class GetAllMessagesByChatQueryHandler : IQueryHandler<GetAllMessagesByChatQuery, List<GetMessageResponse>>
    {
        private readonly IMessageRepository messageRepository;
        private readonly IChatRepository chatRepository;

        public GetAllMessagesByChatQueryHandler(IMessageRepository messageRepository, IChatRepository chatRepository)
        {
            this.messageRepository = messageRepository;
            this.chatRepository = chatRepository;
        }

        public async Task<List<GetMessageResponse>> Execute(GetAllMessagesByChatQuery query)
        {
            if (await chatRepository.GetAsync(query.ChatId) == null)
                throw new ChatNotFoundException(query.ChatId) { DisplayMessage = $"Chat ({query.ChatId}) does not exist." };

            return (await messageRepository.GetAllByChatAsync(query.ChatId)).Select(
                m => new GetMessageResponse(m.Id, m.SenderUniqueName, m.ChatId, m.Timestamp) { TextContent = m.TextContent, QuotedId = m.QuotedId, Reactions = m.Reactions, ImageUrls = m.ImageUrls })
                .ToList();
        }
    }
}
