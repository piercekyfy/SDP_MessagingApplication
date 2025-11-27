using MessageService.Models;

namespace MessageService.Repositories
{
    public interface IMessageRepository
    {
        public Task<List<Message>> GetAllByChatAsync(string chatId);
        public Task CreateAsync(Message message);
    }
}
