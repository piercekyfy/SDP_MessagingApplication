using MessageService.Models;
using MessageService.Repositories;

namespace MessageService.Services
{
    public class MessagesService : IMessagesService
    {
        private readonly IMessageRepository repository;

        public MessagesService(IMessageRepository repository) 
        { 
            this.repository = repository;
        }

        public async Task<List<Message>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }
    
        public async Task<Message> GetByIdAsync(string id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task CreateAsync(Message message)
        {
            message.Timestamp = DateTime.UtcNow;
            await repository.CreateAsync(message);
        }
    }
}
