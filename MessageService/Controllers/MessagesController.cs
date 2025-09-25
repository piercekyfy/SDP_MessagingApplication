using MessageService.Models;
using MessageService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MessageService.Controllers
{
    [ApiController]
    [Route("api/v1/messages")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageRepository repository;

        public MessagesController(IMessageRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await repository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var message = await repository.GetByIdAsync(id);
            if (message == null) return NotFound();
            return Ok(message);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Message message)
        {
            await repository.CreateAsync(message);
            return CreatedAtAction(nameof(Get), new { id = message.Id }, message);
        }
    }
}
