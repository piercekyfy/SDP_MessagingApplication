using MessageService.Models;
using MessageService.Repositories;
using MessageService.Services;
using Microsoft.AspNetCore.Mvc;

namespace MessageService.Controllers
{
    [ApiController]
    [Route("api/v1/messages")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessagesService service;

        public MessagesController(IMessagesService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var message = await service.GetByIdAsync(id);
            if (message == null) return NotFound();
            return Ok(message);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Message message)
        {
            await service.CreateAsync(message);
            return CreatedAtAction(nameof(Get), new { id = message.Id }, message);
        }
    }
}
