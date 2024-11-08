using System.Security.Claims;
using FrontendAPIFinalProject.Services;
using Microsoft.AspNetCore.Mvc;
using ServerPupusas.Models;

namespace FrontendAPIFinalProject.Controllers
{

    // http://localhost:5073/api/users/users
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("users")]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userService.GetAllUsers();

        }

        [HttpGet("email")]
        public async Task<ActionResult<string>> GetUserEmail(HttpContext _httpContext)
        {
            if (_httpContext.User?.Identity?.IsAuthenticated != true)
            {
                Console.WriteLine("User is not authenticated");
                return Unauthorized("User is not authenticated.");
            }

            string? email = _httpContext.User.FindFirstValue(ClaimTypes.Email);
            Console.WriteLine(email);
            if (string.IsNullOrEmpty(email))
            {
                Console.WriteLine("Email claim not found");
                return NotFound("Email claim not found.");
            }

            Console.WriteLine("Authenticated user's email: " + email);
            return Ok(email);
        }
    }
}
