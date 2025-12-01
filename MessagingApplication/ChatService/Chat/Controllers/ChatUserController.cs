using ChatService.Chat.Commands;
using ChatService.Chat.DTOs;
using ChatService.Chat.Queries;
using Microsoft.AspNetCore.Mvc;
using Shared.Middleware.CQRS;
using Shared.Models.Chat;

namespace ChatService.Chat.Controllers
{
    [ApiController]
    [Route("api/v1/chats/{chatId}")]
    public class ChatUserController : ControllerBase
    {
        private readonly IQueryDispatcher queryDispatcher;
        private readonly ICommandDispatcher commandDispatcher;

        public ChatUserController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this.queryDispatcher = queryDispatcher;
            this.commandDispatcher = commandDispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int chatId)
        {
            List<GetChatUserResponse> users = await queryDispatcher.DispatchAsync<GetAllChatUsersQuery, List<GetChatUserResponse>>(new GetAllChatUsersQuery(chatId));
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOne(int chatId, [FromBody] AddUserRequest request)
        {
            if (request.UniqueName == null)
                return BadRequest();

            await commandDispatcher.DispatchAsync<AddChatUserCommand, NoResult>(
                new AddChatUserCommand(
                    chatId, 
                    request.UniqueName, 
                    request.Privileges != null ? new(request.Privileges) : new() { (int)ChatUserPrivilege.CanSend, (int)ChatUserPrivilege.CanForward }
                ));
            return Ok();
        }
    }
}
