using ChatService.Chat.Contexts;
using ChatService.Chat.Repositories;

namespace ChatService.Chat.WorkUnits
{
    public class ChatWorkUnit : IChatWorkUnit
    {
        public IChatRepository ChatRepository => chatRepository;
        private readonly ChatRepository chatRepository;
        private readonly ChatContext context;

        public ChatWorkUnit(ChatContext context)
        {
            this.context = context;
            chatRepository = new ChatRepository(context);
        }

        public async Task BeginAsync()
        {
            await context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await context.Database.CommitTransactionAsync();
        }

        public async Task RollbackAsync()
        {
            await context.Database.RollbackTransactionAsync();
        }
    }
}
