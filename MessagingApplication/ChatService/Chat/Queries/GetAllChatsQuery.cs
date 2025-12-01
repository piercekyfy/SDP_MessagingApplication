using ChatService.Chat.DTOs;
using Shared.Middleware.CQRS;

namespace ChatService.Chat.Queries
{
    public class GetAllChatsQuery : IQuery<List<GetChatResponse>> {}
}
