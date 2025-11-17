using MessageService.Models;
using MongoDB.Driver;

namespace MessageService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> collection;

        public UserRepository(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoDB:ConnectionString"]);
            var database = client.GetDatabase(configuration["MongoDB:Database"]);
            collection = database.GetCollection<User>(configuration["Collections:Users"]);
        }

        public async Task CreateUserAsync(User user)
        {
            await collection.InsertOneAsync(user);
        }

        public async Task<User> GetUserAsync(string uniqueName)
        {
            return await collection.Find(u => u.UniqueName == uniqueName).FirstOrDefaultAsync();
        }
    }
}
