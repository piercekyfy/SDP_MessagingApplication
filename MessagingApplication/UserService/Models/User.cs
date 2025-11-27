using Microsoft.EntityFrameworkCore;
namespace UserService.Models
{
    [PrimaryKey(nameof(UniqueName))]
    public class User
    {
        private string uniqueName = "";
        public string UniqueName
        {
            get { return uniqueName; }
            set
            {
                uniqueName = value.ToLower();
            }
        }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public DateTimeOffset DateCreated { get; set; }

        public User(string uniqueName, string displayName, string email, DateTimeOffset dateCreated)
        {
            UniqueName = uniqueName;
            DisplayName = displayName;
            Email = email;
            DateCreated = dateCreated;
        }
    }
}