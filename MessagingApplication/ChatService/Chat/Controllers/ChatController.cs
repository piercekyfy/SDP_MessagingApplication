using ChatService.Chat.Commands;
using ChatService.Chat.DTOs;
using ChatService.Chat.Queries;
using Microsoft.AspNetCore.Mvc;
using Shared.Middleware.CQRS;

namespace ChatService.Chat.Controllers
{
    [ApiController]
    [Route("api/v1/chats")]
    public class ChatController : ControllerBase
    {
        private readonly IQueryDispatcher queryDispatcher;
        private readonly ICommandDispatcher commandDispatcher;

        public ChatController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this.queryDispatcher = queryDispatcher;
            this.commandDispatcher = commandDispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<GetChatResponse> chats = await queryDispatcher.DispatchAsync<GetAllChatsQuery, List<GetChatResponse>>(new GetAllChatsQuery());
            return Ok(chats);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOne([FromBody] CreateChatCommand command)
        {
            int chatId = await commandDispatcher.DispatchAsync<CreateChatCommand, int>(command);
            return Ok(chatId);
        }
    }
}
