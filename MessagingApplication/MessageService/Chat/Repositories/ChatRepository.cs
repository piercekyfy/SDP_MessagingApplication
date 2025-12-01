using MessageService.Chat.Models;
using MessageService.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;

namespace MessageService.Chat.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly IMongoCollection<ChatModel> collection;

        public ChatRepository(IOptions<MongoDBConfiguration> configuration)
        {
            // Map Model
            if (!BsonClassMap.IsClassMapRegistered(typeof(ChatUserModel)))
            {
                BsonClassMap.RegisterClassMap<ChatUserModel>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdMember(c => c.UniqueName);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(ChatModel)))
            {
                BsonClassMap.RegisterClassMap<ChatModel>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdMember(c => c.Id);
                });
            }

            var connectionString = new MongoUrl(configuration.Value.ConnectionString);
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(connectionString.DatabaseName);
            collection = database.GetCollection<ChatModel>(configuration.Value.Collections.Chats);     
        }

        public async Task<List<ChatModel>> GetAllAsync()
        {
            return await collection.Find(_ => true).ToListAsync();
        }

        public async Task<List<ChatModel>> GetManyAsync(IEnumerable<int> ids)
        {
            return await collection.Find(Builders<ChatModel>.Filter.In(c => c.Id, ids)).ToListAsync();
        }

        public async Task<ChatModel> GetAsync(int chatId)
        {
            return await collection.Find(c => c.Id == chatId).FirstOrDefaultAsync(); 
        }

        public async Task<bool> ExistsAsync(int chatId)
        {
            return await collection.Find(c => c.Id == chatId).Limit(1).AnyAsync();
        }

        public async Task CreateAsync(ChatModel chat)
        {
            await collection.InsertOneAsync(chat);
        }

        public async Task AddUserAsync(int chatId, ChatUserModel user)
        {
            await collection.UpdateOneAsync(
                c => c.Id == chatId,
                Builders<ChatModel>.Update.AddToSet(c => c.Users, user)
            );
        }
    }
}
