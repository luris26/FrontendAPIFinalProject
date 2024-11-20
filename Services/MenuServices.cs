using FrontendAPIFinalProject.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServerPupusas.Models;
using ServerPupusas.ModelDTOs;

namespace FrontendAPIFinalProject.Services
{
    public class MenuService : IMenuService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

        public MenuService(IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<IEnumerable<Menu>> AddNewItemToMenu(MenuDTO newMenuItem)
        {
            if (newMenuItem == null)
            {
                throw new ArgumentNullException(nameof(newMenuItem), "The menu item cannot be null.");
            }

            using var context = await _dbFactory.CreateDbContextAsync();

            var menuEntity = new Menu
            {
                Name = newMenuItem.Name,
                Description = newMenuItem.Description,
                Category = newMenuItem.Category,
                Price = newMenuItem.Price,
                Availability = newMenuItem.Availability ?? true
            };

            await context.Menus.AddAsync(menuEntity);
            await context.SaveChangesAsync();

            return await context.Menus.ToListAsync();
        }

        public async Task<IEnumerable<Menu>> GetAllMenuItems()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            return await context.Menus.ToListAsync();
        }
        public async Task<bool> UpdateMenuItem(Menu menuItem)
        {
            using var _context = await _dbFactory.CreateDbContextAsync();
            _context.Menus.Update(menuItem);
            var updated = await _context.SaveChangesAsync();
            return updated > 0;
        }
        public async Task<bool> DeleteMenuItem(int id)
        {
            using var _context = await _dbFactory.CreateDbContextAsync();
            var menuItem = await _context.Menus.FindAsync(id);
            if (menuItem == null)
            {
                return false;
            }

            _context.Menus.Remove(menuItem);
            var deleted = await _context.SaveChangesAsync();
            return deleted > 0;
        }
        public async Task<Menu?> GetMenuItemById(int id)
        {
            using var _context = await _dbFactory.CreateDbContextAsync();
            return await _context.Menus.FindAsync(id);
        }
    }
}
