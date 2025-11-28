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
        public bool Deleted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset DeletedAt { get; set; }

        public User(string uniqueName, string displayName, string email)
        {
            UniqueName = uniqueName;
            DisplayName = displayName;
            Email = email;
            Deleted = false;
        }
    }
}