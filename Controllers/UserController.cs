using System.Security.Claims;
using FrontendAPIFinalProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerPupusas.Models;

namespace FrontendAPIFinalProject.Controllers
{

    // http://localhost:5073/api/users/users
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;

        public UsersController(IUserService userService, ApplicationDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        [HttpGet("users")]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userService.GetAllUsers();

        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (email == null)
            {
                return Unauthorized("Invalid token");
            }

            var authenticatedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (authenticatedUser == null)
            {
                return Forbid("User not found in the database");
            }

            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }


        // [HttpGet("email")]
        // public async Task<ActionResult<string>> GetUserEmail(HttpContext _httpContext)
        // {
        //     if (_httpContext.User?.Identity?.IsAuthenticated != true)
        //     {
        //         Console.WriteLine("User is not authenticated");
        //         return Unauthorized("User is not authenticated.");
        //     }

        //     string? email = _httpContext.User.FindFirstValue(ClaimTypes.Email);
        //     Console.WriteLine(email);
        //     if (string.IsNullOrEmpty(email))
        //     {
        //         Console.WriteLine("Email claim not found");
        //         return NotFound("Email claim not found.");
        //     }

        //     Console.WriteLine("Authenticated user's email: " + email);
        //     return Ok(email);
        // }
    }
}
