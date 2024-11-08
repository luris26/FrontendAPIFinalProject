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
    }
}
