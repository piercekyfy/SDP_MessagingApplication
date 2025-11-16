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
        private readonly IUsersExchangeService usersExchangeService;

        public UsersController(IUsersService usersService, IUsersExchangeService usersExchangeService)
        {
            this.usersService = usersService;
            this.usersExchangeService = usersExchangeService;
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
            await usersExchangeService.PublishUserCreated(user);
            return CreatedAtAction(nameof(Get), new {uniqueName = user.UniqueName}, user);
        }
    }
}
