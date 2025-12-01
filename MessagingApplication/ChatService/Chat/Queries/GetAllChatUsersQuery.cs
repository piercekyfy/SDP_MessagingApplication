using ChatService.Chat.DTOs;
using Shared.Middleware.CQRS;

namespace ChatService.Chat.Queries
{
    public class GetAllChatUsersQuery : IQuery<List<GetChatUserResponse>>
    {
        public int ChatId { get; set; }

        public GetAllChatUsersQuery(int chatId)
        {
            ChatId = chatId;
        }
    }
}
