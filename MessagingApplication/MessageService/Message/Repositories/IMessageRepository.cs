using MessageService.Message.Models;

namespace MessageService.Message.Repositories
{
    public interface IMessageRepository
    {
        public Task<List<MessageEntity>> GetAllByChatAsync(int chatId);
        public Task<List<MessageEntity>> GetManyAsync(IEnumerable<string> ids);
        public Task<MessageEntity> GetAsync(string id);
        public Task<MessageEntity> CreateAsync(MessageEntity message);
    }
}
