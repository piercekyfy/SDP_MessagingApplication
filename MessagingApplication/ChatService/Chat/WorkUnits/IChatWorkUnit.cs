using ChatService.Chat.Contexts;
using ChatService.Chat.Repositories;
using Shared.Middleware;

namespace ChatService.Chat.WorkUnits
{
    public interface IChatWorkUnit : IWorkUnit
    {
        public IChatRepository ChatRepository { get; }
    }
}
