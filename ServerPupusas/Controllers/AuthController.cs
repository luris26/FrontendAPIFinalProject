using Microsoft.AspNetCore.Mvc;
using FrontendAPIFinalProject.Services;

namespace FrontendAPIFinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginModel model)
        {
            //compare to the database :)
            if (model.Username == "luris" && model.Password == "luris")
            {
                var token = _jwtService.GenerateToken(model.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid credentials");
        }
    }

    public class UserLoginModel
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
