using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

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

        public string Email { get; set; }
        public DateTime DateCreated { get; set; }

        public User(string uniqueName, string email, DateTime dateCreated)
        {
            UniqueName = uniqueName;
            Email = email;
            DateCreated = dateCreated;
        }
    }
}