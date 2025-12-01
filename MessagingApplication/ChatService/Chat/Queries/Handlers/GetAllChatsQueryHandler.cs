using ChatService.Chat.DTOs;
using ChatService.Chat.Repositories;
using Shared.Middleware.CQRS;

namespace ChatService.Chat.Queries.Handlers
{
    public class GetAllChatsQueryHandler : IQueryHandler<GetAllChatsQuery, List<GetChatResponse>>
    {
        private readonly IChatRepository chatRepository;

        public GetAllChatsQueryHandler(IChatRepository chatRepository)
        {
            this.chatRepository = chatRepository;
        }

        public async Task<List<GetChatResponse>> Execute(GetAllChatsQuery query)
        {
            return (await chatRepository.GetAllChatsAsync()).Select(c => new GetChatResponse(c.Id, c.Name)).ToList();
        }
    }
}
