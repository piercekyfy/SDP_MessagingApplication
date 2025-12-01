namespace MessageService.User.Models
{
    public class UserModel
    {

        public string UniqueName { get; set; }
        public string DisplayName { get; set; }
        public bool Deleted { get; set; }

        public UserModel(string uniqueName, string displayName)
        {
            UniqueName = uniqueName;
            DisplayName = displayName;
            Deleted = false;
        }
    }
}
