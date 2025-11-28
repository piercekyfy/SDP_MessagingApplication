using MessageService.Models;

namespace MessageService.Repositories
{
    public interface IChatRepository
    {
        Task<bool> ExistsAsync(string chatId);
        Task<List<Chat>> GetAllAsync();
        Task<List<Chat>> GetManyAsync(IEnumerable<string> ids);
        Task<Chat> GetAsync(string chatId);
        Task CreateAsync(Chat chat);
    }
}
