using Shared.Exceptions;

namespace MessageService.Exceptions
{
    public class InvalidMessageContentException : DomainException
    {
        public string? OffendingProperty { get; set; }
        public InvalidMessageContentException(string? offendingProperty) : base($"Message contains {(offendingProperty == null ? "no content" : $"invalid content ({offendingProperty})")}.")
        {
            OffendingProperty = offendingProperty;
        }
    }
}
