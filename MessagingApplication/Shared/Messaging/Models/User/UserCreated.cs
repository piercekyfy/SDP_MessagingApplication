namespace Shared.Messaging.Models.User
{
    public class UserCreated : MessageModel
    {
        public string UniqueName { get; set; }

        public UserCreated(string uniqueName)
        {
            UniqueName = uniqueName;
        }
    }
}
