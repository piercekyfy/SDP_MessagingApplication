using MessageService.Configurations;
using MessageService.User.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;

namespace MessageService.User.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<UserModel> collection;

        public UserRepository(IOptions<MongoDBConfiguration> configuration)
        {
            // Map Model
            if (!BsonClassMap.IsClassMapRegistered(typeof(UserModel)))
            {
                BsonClassMap.RegisterClassMap<UserModel>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdMember(c => c.UniqueName);
                });
            }

            var connectionString = new MongoUrl(configuration.Value.ConnectionString);
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(connectionString.DatabaseName);
            collection = database.GetCollection<UserModel>(configuration.Value.Collections.Users);
        }

        public async Task<List<UserModel>> GetManyAsync(IEnumerable<string> uniqueNames)
        {
            return await collection.Find(Builders<UserModel>.Filter.In(u => u.UniqueName, uniqueNames)).ToListAsync();
        }

        public async Task<UserModel> GetAsync(string uniqueName)
        {
            return await collection.Find(u => u.UniqueName == uniqueName).FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsAsync(string uniqueName)
        {
            return await collection.Find(u => u.UniqueName == uniqueName).Limit(1).AnyAsync();
        }

        public async Task CreateAsync(UserModel user)
        {
            await collection.InsertOneAsync(user);
        }

        public async Task UpdateAsync(string uniqueName, string? displayName)
        {
            await collection.UpdateOneAsync(u => u.UniqueName == uniqueName, Builders<UserModel>.Update.Set(u => u.DisplayName, displayName));
        }

        public async Task DeleteAsync(string uniqueName)
        {
            await collection.UpdateOneAsync(u => u.UniqueName == uniqueName, Builders<UserModel>.Update.Set(u => u.Deleted, true));
        }
    }
}
