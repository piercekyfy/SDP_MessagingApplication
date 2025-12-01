using ChatService.Chat.DTOs;
using ChatService.Chat.Repositories;
using Shared.Middleware.CQRS;

namespace ChatService.Chat.Queries.Handlers
{
    public class GetAllChatUsersQueryHandler : IQueryHandler<GetAllChatUsersQuery, List<GetChatUserResponse>>
    {
        private readonly IChatRepository chatRepository;

        public GetAllChatUsersQueryHandler(IChatRepository chatRepository)
        {
            this.chatRepository = chatRepository;
        }

        public async Task<List<GetChatUserResponse>> Execute(GetAllChatUsersQuery query)
        {
            return (
                await chatRepository.GetAllChatUsersAsync(query.ChatId))
                    .Select(
                cu => new GetChatUserResponse(
                        cu.UserUniqueName, 
                        cu.Privileges.Select(p => (int)p.Privilege).ToArray(), 
                        cu.JoinedAt))
                    .ToList();
        }
    }
}
