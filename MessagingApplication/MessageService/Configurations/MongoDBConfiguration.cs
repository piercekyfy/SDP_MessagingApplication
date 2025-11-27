using Shared.Configurations;

namespace MessageService.Configurations
{
    public class MongoDBCollectionsConfiguration
    {
        public string Chats { get; set; } = default!;
        public string Messages { get; set; } = default!;
        public string Users { get; set; } = default!;
    }
    public class MongoDBConfiguration : DBConfiguration 
    {
        public MongoDBCollectionsConfiguration Collections { get; set; } = default!;
    }
}
