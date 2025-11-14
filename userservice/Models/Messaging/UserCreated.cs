namespace UserService.Models.Messaging
{
    public class UserCreated
    {
        public string UniqueName { get; set; }

        public UserCreated(string uniqueName)
        {
            UniqueName = uniqueName;
        }
    }
}
