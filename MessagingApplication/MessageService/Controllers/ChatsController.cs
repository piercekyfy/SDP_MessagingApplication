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
    [Route("api/v1/chats")]
    public class ChatsController : ControllerBase
    {
        private readonly IChatsService service;

        public ChatsController(IChatsService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await service.GetAllAsync());
        }

        [HttpGet("{chatId}")]
        public async Task<IActionResult> Get(string chatId)
        {
            Chat chat;

            try
            {
                chat = await service.GetAsync(chatId);
            } catch (ChatNotFoundException)
            {
                return NotFound(chatId);
            }
            catch (DomainException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return Ok(chat);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOne([FromBody] CreateChatRequest request)
        {
            Chat chat = new Chat(request.Name, request.OwnerUniqueName);
            await service.CreateAsync(chat);

            return CreatedAtAction(nameof(Get), new { chatId = chat.Id }, chat);
        }
    }
}
