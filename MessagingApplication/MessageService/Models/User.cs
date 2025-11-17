namespace MessageService.Models
{
    public class User
    {
        public string UniqueName { get; set; }

        public User(string uniqueName)
        {
            UniqueName = uniqueName;
        }
    }
}
