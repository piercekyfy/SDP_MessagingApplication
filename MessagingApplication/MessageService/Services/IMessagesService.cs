using MessageService.Models;

namespace MessageService.Services
{
    public interface IMessagesService
    {
        public Task<List<Message>> GetAllByChatAsync(string chatId);
        public Task SendAsync(Message message);
    }
}
