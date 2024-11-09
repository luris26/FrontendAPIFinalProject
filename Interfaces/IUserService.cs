using System.Collections.Generic;
using System.Threading.Tasks;
using ServerPupusas.Models;

namespace FrontendAPIFinalProject.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User?> GetUserByUsername(string userEmail);
    }
}
