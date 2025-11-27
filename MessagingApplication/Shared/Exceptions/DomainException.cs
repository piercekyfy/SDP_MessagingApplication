namespace Shared.Exceptions
{
    public class DomainException : Exception
    {
        public string DisplayMessage { get; set; } = string.Empty;

        public DomainException(string message) : base(message) { }
    }
}
