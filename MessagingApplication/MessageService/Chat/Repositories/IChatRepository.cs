using MessageService.Chat.Models;

namespace MessageService.Chat.Repositories
{
    public interface IChatRepository
    {
        Task<bool> ExistsAsync(int chatId);
        Task<List<ChatModel>> GetAllAsync();
        Task<List<ChatModel>> GetManyAsync(IEnumerable<int> ids);
        Task<ChatModel> GetAsync(int chatId);
        Task CreateAsync(ChatModel chat);
        Task AddUserAsync(int chatId, ChatUserModel user);
    }
}
