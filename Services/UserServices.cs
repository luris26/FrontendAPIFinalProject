using FrontendAPIFinalProject.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServerPupusas.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FrontendAPIFinalProject.Services
{
    public class UserService : IUserService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

        public UserService(IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        async Task<IEnumerable<User>> IUserService.GetAllUsers()
        {
            var context = await _dbFactory.CreateDbContextAsync();
            var users = await context.Users.ToListAsync();
            return users;
        }
        public async Task<User?> GetUserByUsername(string userEmail)
        {
            var context = await _dbFactory.CreateDbContextAsync();
            var users = context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            return await users;

        }
    }
}
