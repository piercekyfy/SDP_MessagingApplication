using MessageService.DTOs;
using MessageService.Exceptions;
using MessageService.Models;
using MessageService.Repositories;
using MessageService.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Exceptions;

namespace MessageService.Controllers
{
    [ApiController]
    [Route("api/v1/chats/{chatId}/messages")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessagesService service;

        public MessagesController(IMessagesService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string chatId)
        {
            List<Message> messages;

            try
            {
                messages = await service.GetAllByChatAsync(chatId);
            } catch (ChatNotFoundException)
            {
                return NotFound(chatId);
            } catch (DomainException)
            {
                return BadRequest();
            } catch (Exception)
            {
                return StatusCode(500);
            }

            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOne([FromBody] CreateMessageRequest request)
        {
            Message message = new Message(request.SenderUniqueName, request.ChatId, request.Content);

            try
            {
                await service.SendAsync(message);
            }
            catch (ChatNotFoundException)
            {
                return NotFound(message.ChatId);
            }
            
            catch (InvalidChatUserException)
            {
                return Forbid();
            }
            catch (ChatUserPermissionException)
            {
                return Forbid();
            }
            catch (DomainException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return Ok(new MessageCreatedResponse(message.Id ?? "", message.Timestamp));
        }
    }
}
