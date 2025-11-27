using Shared.Exceptions;

namespace UserService.Exceptions
{
    public class UserNotFoundException : DomainException
    {
        public string UniqueName { get; private set; }

        public UserNotFoundException(string uniqueName) : base($"User ({uniqueName}) not found.")
        {
            UniqueName = uniqueName;
        }
    }
}
