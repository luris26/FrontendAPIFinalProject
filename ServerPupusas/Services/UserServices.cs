using FrontendAPIFinalProject.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServerPupusas.Models;

namespace FrontendAPIFinalProject.Services
{
    public class UserService : IUserService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

        public UserService(IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var context = await _dbFactory.CreateDbContextAsync();
            var users = await context.Users.ToListAsync();
            return users;
        }

        async Task<IEnumerable<User>> IUserService.GetAllUsers()
        {
            var context = await _dbFactory.CreateDbContextAsync();
            var users = await context.Users.ToListAsync();
            return users;
        }
    }
}
