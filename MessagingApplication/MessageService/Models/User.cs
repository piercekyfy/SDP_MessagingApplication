namespace MessageService.Models
{
    public class User
    {

        public string UniqueName { get; set; }
        public string DisplayName { get; set; }

        public User(string uniqueName, string displayName)
        {
            UniqueName = uniqueName;
            DisplayName = displayName;
        }
    }
}
