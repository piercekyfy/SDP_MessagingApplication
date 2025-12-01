namespace ChatService.User.Models
{
    public class UserModel
    {
        public string UniqueName { get; set; }

        public UserModel(string uniqueName)
        {
            UniqueName = uniqueName;
        }
    }
}
