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
            List<GetMessageResponse> messages;

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
                throw;
            }

            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOne(string chatId, [FromBody] SendMessageRequest request)
        {
            Message result;
            try
            {
                result = await service.SendAsync(chatId, request);
            }
            catch (ChatNotFoundException)
            {
                return NotFound(chatId);
            }
            
            catch (InvalidChatUserException)
            {
                return Forbid();
            }
            catch (ChatUserPermissionException)
            {
                return Forbid();
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

            return Ok(new MessageCreatedResponse(result.Id ?? "", result.Timestamp));
        }
    }
}
