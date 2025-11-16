using MessageService.Models;

namespace MessageService.Repositories
{
    public interface IMessageRepository
    {
        Task<List<Message>> GetAllAsync();
        Task<Message> GetByIdAsync(string id);
        Task CreateAsync(Message message);
    }
}
