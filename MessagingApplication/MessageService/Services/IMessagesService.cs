using MessageService.DTOs;
using MessageService.Models;

namespace MessageService.Services
{
    public interface IMessagesService
    {
        public Task<List<GetMessageResponse>> GetAllByChatAsync(string chatId);
        public Task<Message> SendAsync(string chatId, SendMessageRequest request);
    }
}
