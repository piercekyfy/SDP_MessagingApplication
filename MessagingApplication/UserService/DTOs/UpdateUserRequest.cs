using UserService.Models;

namespace UserService.DTOs
{
    public class UpdateUserRequest
    {
        public string? DisplayName { get; set; }
        public string? Email { get; set; }

        public void Apply(User user)
        {
            if(!string.IsNullOrEmpty(DisplayName))
                user.DisplayName = DisplayName;
            if (!string.IsNullOrEmpty(Email))
                user.Email = Email;
        }
    }
}
