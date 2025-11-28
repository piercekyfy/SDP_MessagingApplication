using MessageService.Configurations;
using MessageService.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;

namespace MessageService.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly IMongoCollection<Chat> collection;

        public ChatRepository(IOptions<MongoDBConfiguration> configuration)
        {
            // Map Model
            if (!BsonClassMap.IsClassMapRegistered(typeof(Chat)))
            {
                BsonClassMap.RegisterClassMap<Chat>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdMember(c => c.Id)
                        .SetIdGenerator(StringObjectIdGenerator.Instance);
                });
            }

            var connectionString = new MongoUrl(configuration.Value.ConnectionString);
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(connectionString.DatabaseName);
            collection = database.GetCollection<Chat>(configuration.Value.Collections.Chats);      
        }

        public async Task<List<Chat>> GetAllAsync()
        {
            return await collection.Find(_ => true).ToListAsync();
        }

        public async Task<List<Chat>> GetManyAsync(IEnumerable<string> ids)
        {
            return await collection.Find(Builders<Chat>.Filter.In(c => c.Id, ids)).ToListAsync();
        }

        public async Task<Chat> GetAsync(string chatId)
        {
            return await collection.Find(c => c.Id == chatId).FirstOrDefaultAsync(); 
        }

        public async Task<bool> ExistsAsync(string chatId)
        {
            return await collection.Find(c => c.Id == chatId).Limit(1).AnyAsync();
        }

        public async Task CreateAsync(Chat chat)
        {
            chat.CreatedOn = DateTime.UtcNow;
            await collection.InsertOneAsync(chat);
        }
    }
}
