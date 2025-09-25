using MessageService.Models;
using MongoDB.Driver;

namespace MessageService.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IMongoCollection<Message> collection;

        public MessageRepository(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoDB:ConnectionString"]);
            var database = client.GetDatabase(configuration["MongoDB:Database"]);
            collection = database.GetCollection<Message>("Messages");
        }

        
        public async Task<List<Message>> GetAllAsync()
        {
            return await collection.Find(_ => true).ToListAsync();
        }

        public async Task<Message> GetByIdAsync(string id)
        {
            return await collection.Find(m => m.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Message message)
        {
            message.Timestamp = DateTime.UtcNow;
            await collection.InsertOneAsync(message);
        }

    }
}
