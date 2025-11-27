using MessageService.Models;

namespace MessageService.Services
{
    public interface IChatsService
    {
        Task<List<Chat>> GetAllAsync();
        public Task<Chat> GetAsync(string chatId);
        public Task CreateAsync(Chat chat);
    }
}
