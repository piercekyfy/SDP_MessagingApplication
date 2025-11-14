using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Infastructure.Messaging;
using UserService.Models;
using UserService.Services;

namespace UserService.Controllers
{
    [ApiController]
    [Route("test")]
    public class Test : ControllerBase
    {
        private readonly IMessageBrokerConnection pub;

        public Test(IMessageBrokerConnection pub)
        {
            this.pub = pub;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(pub);
        }
    }
}
