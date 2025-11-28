
namespace MessageService.Models
{
    public class Chat
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; }
        public List<ChatUser> Users { get; set; } = new List<ChatUser>();
        public DateTimeOffset CreatedOn { get; set; }

        public Chat(string name, List<ChatUser> users)
        {
            Name = name;
            Users = users;
        }

        public Chat(string name, string ownerUniqueName)
        {
            Name = name;
            Users.Add(new ChatUser(ownerUniqueName, ChatUser.AllPermissions));
        }
    }
}
