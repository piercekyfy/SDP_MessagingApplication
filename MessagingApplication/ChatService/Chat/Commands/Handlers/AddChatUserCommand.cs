using ChatService.Chat.Models;
using ChatService.Chat.WorkUnits;
using ChatService.Exceptions;
using ChatService.User.Repositories;
using Shared.Middleware.CQRS;
using Shared.Models.Chat;

namespace ChatService.Chat.Commands.Handlers
{
    public class AddChatUserCommandHandler : ICommandHandler<AddChatUserCommand, NoResult>
    {
        private readonly IChatWorkUnit workUnit;
        private readonly IUserRepository userRepository;

        public AddChatUserCommandHandler(IChatWorkUnit workUnit, IUserRepository userRepository)
        {
            this.workUnit = workUnit;
            this.userRepository = userRepository;
        }

        public async Task<NoResult> Execute(AddChatUserCommand command)
        {
            if (await userRepository.GetByUniqueNameAsync(command.UniqueName) == null)
                throw new UserNotFoundException(command.UniqueName) { DisplayMessage = $"User ({command.UniqueName}) does not exist." };

            if (await workUnit.ChatRepository.GetByIdAsync(command.ChatId) == null)
                throw new ChatNotFoundException(command.ChatId) { DisplayMessage = $"Chat ({command.ChatId}) does not exist." };

            await workUnit.BeginAsync();
            try
            {
                ChatUserEntity user = new ChatUserEntity(command.ChatId, command.UniqueName);

                await workUnit.ChatRepository.AddUserAsync(user);
                await workUnit.ChatRepository.SetUserPrivilegesAsync(user.ChatId, user.UserUniqueName, command.Privileges.Select(p => (ChatUserPrivilege)p).ToArray(), true);
            }
            catch (Exception)
            {
                await workUnit.RollbackAsync();
                throw;
            }
            await workUnit.CommitAsync();

            return default;
        }
    }
}
