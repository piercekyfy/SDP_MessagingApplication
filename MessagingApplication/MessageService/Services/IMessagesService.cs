using MessageService.Models;

namespace MessageService.Services
{
    public interface IMessagesService
    {
        Task<List<Message>> GetAllAsync();
        Task<Message> GetByIdAsync(string id);
        Task CreateAsync(Message message);
    }
}
