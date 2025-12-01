using MessageService.Configurations;
using MessageService.Message.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;

namespace MessageService.Message.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IMongoCollection<MessageEntity> collection;

        public MessageRepository(IOptions<MongoDBConfiguration> configuration)
        {
            // Map Model
            if (!BsonClassMap.IsClassMapRegistered(typeof(MessageEntity)))
            {
                BsonClassMap.RegisterClassMap<MessageEntity>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdMember(c => c.Id)
                        .SetIdGenerator(StringObjectIdGenerator.Instance);
                });
            }

            var connectionString = new MongoUrl(configuration.Value.ConnectionString);
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(connectionString.DatabaseName);
            collection = database.GetCollection<MessageEntity>(configuration.Value.Collections.Messages);

            var indexes = Builders<MessageEntity>.IndexKeys
                .Ascending(m => m.ChatId)
                .Descending(m => m.Timestamp); // Index by chat id, order by timestamp descending.
            collection.Indexes.CreateOne(new CreateIndexModel<MessageEntity>(indexes));
        }


        public async Task<List<MessageEntity>> GetAllByChatAsync(int chatId)
        {
            return await collection.Find(m => m.ChatId == chatId).SortByDescending(m => m.Timestamp).ToListAsync();
        }

        public async Task<List<MessageEntity>> GetManyAsync(IEnumerable<string> ids)
        {
            return await collection.Find(Builders<MessageEntity>.Filter.In(m => m.Id, ids)).ToListAsync();
        }

        public async Task<MessageEntity> GetAsync(string id)
        {
            return await collection.Find(m => m.Id == id).FirstOrDefaultAsync();
        }

        public async Task<MessageEntity> CreateAsync(MessageEntity message)
        {
            message.Timestamp = DateTime.UtcNow;
            await collection.InsertOneAsync(message);
            return message;
        }

    }
}
