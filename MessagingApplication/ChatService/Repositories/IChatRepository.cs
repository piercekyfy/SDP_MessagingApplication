using ChatService.Models;

namespace ChatService.Repositories
{
    public interface IChatRepository
    {
        Task<List<Chat>> GetAllAsync();
        Task CreateAsync(Chat chat);
        Task UpdateAsync(int chatId, string chatName);
    }
}
