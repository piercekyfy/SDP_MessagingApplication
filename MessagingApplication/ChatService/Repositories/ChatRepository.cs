using Microsoft.EntityFrameworkCore;
using ChatService.Contexts;
using ChatService.Models;

namespace ChatService.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ChatsContext context;

        public ChatRepository(ChatsContext context)
        {
            this.context = context;
        }

        public async Task<List<Chat>> GetAllAsync()
        {
            return await context.Chats.ToListAsync();
        }

        public async Task CreateAsync(Chat chat)
        {
            chat.CreatedAt = DateTimeOffset.UtcNow;
            await context.Chats.AddAsync(chat);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int chatId, string chatName)
        {
            Chat? chat = await context.Chats.Where(c => c.Id == chatId).FirstOrDefaultAsync();

            if (chat == null)
                throw new ArgumentException(nameof(chatId));

            chat.Name = chatName;
            context.Chats.Update(chat);
            await context.SaveChangesAsync();
        }
    }
}
