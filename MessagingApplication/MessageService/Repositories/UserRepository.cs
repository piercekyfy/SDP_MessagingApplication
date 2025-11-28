using MessageService.Configurations;
using MessageService.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;

namespace MessageService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> collection;

        public UserRepository(IOptions<MongoDBConfiguration> configuration)
        {
            // Map Model
            if (!BsonClassMap.IsClassMapRegistered(typeof(User)))
            {
                BsonClassMap.RegisterClassMap<User>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdMember(c => c.UniqueName);
                });
            }

            var connectionString = new MongoUrl(configuration.Value.ConnectionString);
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(connectionString.DatabaseName);
            collection = database.GetCollection<User>(configuration.Value.Collections.Users);
        }

        public async Task<List<User>> GetManyAsync(IEnumerable<string> uniqueNames)
        {
            return await collection.Find(Builders<User>.Filter.In(u => u.UniqueName, uniqueNames)).ToListAsync();
        }

        public async Task<User> GetAsync(string uniqueName)
        {
            return await collection.Find(u => u.UniqueName == uniqueName).FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsAsync(string uniqueName)
        {
            return await collection.Find(u => u.UniqueName == uniqueName).Limit(1).AnyAsync();
        }

        public async Task CreateAsync(User user)
        {
            await collection.InsertOneAsync(user);
        }

        public async Task UpdateAsync(string uniqueName, string? displayName)
        {
            await collection.UpdateOneAsync(u => u.UniqueName == uniqueName, Builders<User>.Update.Set(u => u.DisplayName, displayName));
        }

        public async Task DeleteAsync(string uniqueName)
        {
            await collection.UpdateOneAsync(u => u.UniqueName == uniqueName, Builders<User>.Update.Set(u => u.Deleted, true));
        }
    }
}
