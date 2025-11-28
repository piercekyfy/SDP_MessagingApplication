using Shared.Messaging.Models.User;

namespace MessageService.Services.Events
{
    public interface IUserEventHandler
    {
        Task HandleUserCreatedAsync(UserUpdated ev);
        Task HandleUserUpdatedAsync(UserUpdated ev);
        Task HandleUserDeletedAsync(UserDeleted ev);
    }
}
