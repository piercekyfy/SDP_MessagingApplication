using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Models;
using UserService.Services;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await usersService.GetAllAsync());
        }

        [HttpGet("{uniqueName}")]
        public async Task<IActionResult> Get(string uniqueName)
        {
            return Ok(await usersService.GetByUniqueNameAsync(uniqueName));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            await usersService.CreateAsync(user);
            return CreatedAtAction(nameof(Get), new {uniqueName = user.UniqueName}, user);
        }
    }
}
