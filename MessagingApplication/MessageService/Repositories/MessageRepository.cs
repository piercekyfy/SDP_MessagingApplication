using MessageService.Configurations;
using MessageService.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;

namespace MessageService.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IMongoCollection<Message> collection;

        public MessageRepository(IOptions<MongoDBConfiguration> configuration)
        {
            // Map Model
            if (!BsonClassMap.IsClassMapRegistered(typeof(Message)))
            {
                BsonClassMap.RegisterClassMap<Message>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdMember(c => c.Id)
                        .SetIdGenerator(StringObjectIdGenerator.Instance);
                });
            }

            var connectionString = new MongoUrl(configuration.Value.ConnectionString);
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(connectionString.DatabaseName);
            collection = database.GetCollection<Message>(configuration.Value.Collections.Messages);

            var indexes = Builders<Message>.IndexKeys
                .Ascending(m => m.ChatId)
                .Descending(m => m.Timestamp); // Index by chat id, order by timestamp descending.
            collection.Indexes.CreateOne(new CreateIndexModel<Message>(indexes));
        }


        public async Task<List<Message>> GetAllByChatAsync(string chatId)
        {
            return await collection.Find(m => m.ChatId == chatId).SortByDescending(m => m.Timestamp).ToListAsync();
        }

        public async Task<List<Message>> GetManyAsync(IEnumerable<string> ids)
        {
            return await collection.Find(Builders<Message>.Filter.In(m => m.Id, ids)).ToListAsync();
        }

        public async Task<Message> GetAsync(string id)
        {
            return await collection.Find(m => m.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Message> CreateAsync(Message message)
        {
            message.Timestamp = DateTime.UtcNow;
            await collection.InsertOneAsync(message);
            return message;
        }

    }
}
