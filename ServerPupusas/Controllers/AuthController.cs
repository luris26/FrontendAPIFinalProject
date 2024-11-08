using Microsoft.AspNetCore.Mvc;
using FrontendAPIFinalProject.Services;
using ServerPupusas.Models;
using Microsoft.EntityFrameworkCore;

namespace FrontendAPIFinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly ApplicationDbContext _context;

        public AuthController(JwtService jwtService, ApplicationDbContext context)
        {
            _jwtService = jwtService;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            //compare to the database :)
            Console.WriteLine(user?.Email);
            if (user != null && user.PasswordHash == model.Password)
            {
                var token = _jwtService.GenerateToken(model.Email);
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid credentials");
        }
    }

    public class UserLoginModel
    {
        // public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
    }
}
