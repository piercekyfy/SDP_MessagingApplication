using ChatService.Chat.Events;
using ChatService.Chat.Models;
using ChatService.Chat.Publishers;
using ChatService.Chat.WorkUnits;
using ChatService.Exceptions;
using ChatService.User.Repositories;
using Shared.Middleware.CQRS;

namespace ChatService.Chat.Commands.Handlers
{
    public class CreateChatCommandHandler : ICommandHandler<CreateChatCommand, int>
    {
        private readonly IChatWorkUnit workUnit;
        private readonly IUserRepository userRepository;
        private readonly ChatMessagePublisher publisher;

        public CreateChatCommandHandler(IChatWorkUnit workUnit, IUserRepository userRepository, ChatMessagePublisher publisher)
        {
            this.workUnit = workUnit;
            this.userRepository = userRepository;
            this.publisher = publisher;
        }

        public async Task<int> Execute(CreateChatCommand command)
        {
            if (await userRepository.GetByUniqueNameAsync(command.CreatorUniqueName) == null)
                throw new UserNotFoundException(command.CreatorUniqueName) { DisplayMessage = $"Chat creator ({command.CreatorUniqueName}) does not exist."};

            ChatEntity chat = new ChatEntity(command.Name);
            chat.CreatedAt = DateTimeOffset.UtcNow;

            await workUnit.BeginAsync();
            try
            {
                await workUnit.ChatRepository.CreateChatAsync(chat);
                await publisher.PublishCreatedAsync(new ChatCreated(chat.Id, chat.Name));

                ChatUserEntity user = new ChatUserEntity(chat.Id, command.CreatorUniqueName);
                user.JoinedAt = DateTimeOffset.UtcNow;

                await workUnit.ChatRepository.AddUserAsync(user);
                await workUnit.ChatRepository.SetUserPrivilegesAsync(chat.Id, user.UserUniqueName, ChatUserPrivilegeModel.All, true);
                await publisher.PublishJoinedAsync(new UserJoinedChat(chat.Id, user.UserUniqueName, ChatUserPrivilegeModel.All.Select(p => (int)p).ToArray(), user.JoinedAt));
            } catch (Exception)
            {
                await workUnit.RollbackAsync();
                throw;
            }
            await workUnit.CommitAsync();

            return chat.Id;
        }
    }
}
