using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Exceptions;
using UserService.DTOs;
using UserService.Exceptions;
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
        public async Task<IActionResult> CreateOne([FromBody] User user)
        {
            await usersService.CreateAsync(user);
            
            return CreatedAtAction(nameof(Get), new {uniqueName = user.UniqueName}, user);
        }

        [HttpPatch("{uniqueName}")]
        public async Task<IActionResult> UpdateOne(string uniqueName, [FromBody] UpdateUserRequest request)
        {
            User? updated;
            try
            {
                updated = await usersService.UpdateAsync(uniqueName, request);
            } catch (UserNotFoundException)
            {
                return NotFound(uniqueName);
            } catch (DomainException ex)
            {
                return BadRequest(ex.DisplayMessage);
            } catch (Exception)
            {
                return StatusCode(500);
            }

            return Ok(updated);
        }

        [HttpDelete("{uniqueName}")]
        public async Task<IActionResult> DeleteOne(string uniqueName)
        {
            try
            {
                await usersService.DeleteAsync(uniqueName);
            } catch (UserNotFoundException)
            {
                return NotFound(uniqueName);
            } catch (DomainException ex)
            {
                return BadRequest(ex.DisplayMessage);
            } catch (Exception)
            {
                return StatusCode(500);
            }

            return Ok(uniqueName);
        }
    }
}
