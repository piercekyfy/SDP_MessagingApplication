using MessageService.Exceptions;
using MessageService.Message.Commands;
using MessageService.Message.DTOs;
using MessageService.Message.Models;
using MessageService.Message.Queries;
using Microsoft.AspNetCore.Mvc;
using Shared.Exceptions;
using Shared.Middleware.CQRS;

namespace MessageService.Message.Controllers
{
    [ApiController]
    [Route("api/v1/chats/{chatId}/messages")]
    public class MessagesController : ControllerBase
    {
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IQueryDispatcher queryDispatcher;

        public MessagesController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            this.commandDispatcher = commandDispatcher;
            this.queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int chatId)
        {
            List<GetMessageResponse> messages;

            try
            {
                messages = await queryDispatcher.DispatchAsync<GetAllMessagesByChatQuery, List<GetMessageResponse>>(new GetAllMessagesByChatQuery(chatId));
            } catch (ChatNotFoundException)
            {
                return NotFound(chatId);
            } catch (DomainException)
            {
                return BadRequest();
            } catch (Exception)
            {
                throw;
            }

            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOne(int chatId, [FromBody] SendMessageRequest request)
        {
            string result;
            try
            {
                result = await commandDispatcher.DispatchAsync<SendMessageCommand, string>(new SendMessageCommand(chatId, request.SenderUniqueName)
                {
                    TextContent = request.TextContent,
                    QuotedId = request.QuotedId,
                    ImageUrls = request.ImageUrls
                });
            }
            catch (ChatNotFoundException)
            {
                return NotFound(chatId);
            }
            catch (UserNotFoundException)
            {
                return NotFound(request.SenderUniqueName);
            }
            catch (InvalidChatUserException ex)
            {
                return BadRequest(ex.DisplayMessage);
            }
            catch (ChatUserPermissionException ex)
            {
                return BadRequest(ex.DisplayMessage);
            }
            catch (InvalidMessageContentException ex)
            {
                return BadRequest(ex.DisplayMessage);
            }
            catch (DomainException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return Ok(result);
        }
    }
}
