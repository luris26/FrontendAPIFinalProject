
using ServerPupusas.ModelDTOs;
using ServerPupusas.Models;

namespace FrontendAPIFinalProject.Services
{
    public interface IMenuService
    {
        Task<IEnumerable<Menu>> GetAllMenuItems();
        Task<IEnumerable<Menu>> AddNewItemToMenu(MenuDTO newMenuItem);
        Task<bool> UpdateMenuItem(Menu menuItem);
        Task<bool> DeleteMenuItem(int id);
        Task<Menu?> GetMenuItemById(int id);
    }
}
