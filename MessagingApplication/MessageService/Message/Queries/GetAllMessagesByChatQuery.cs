using MessageService.Message.DTOs;
using Shared.Middleware.CQRS;

namespace MessageService.Message.Queries
{
    public class GetAllMessagesByChatQuery : IQuery<List<GetMessageResponse>>
    {
        public int ChatId { get; set; }

        public GetAllMessagesByChatQuery(int chatId)
        {
            ChatId = chatId;
        }
    }
}
