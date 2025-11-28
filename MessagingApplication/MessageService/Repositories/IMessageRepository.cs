using MessageService.Models;

namespace MessageService.Repositories
{
    public interface IMessageRepository
    {
        public Task<List<Message>> GetAllByChatAsync(string chatId);
        public Task<List<Message>> GetManyAsync(IEnumerable<string> ids);
        public Task<Message> GetAsync(string id);
        public Task<Message> CreateAsync(Message message);
    }
}
