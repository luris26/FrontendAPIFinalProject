using Microsoft.AspNetCore.Mvc;
using FrontendAPIFinalProject.Services;
using ServerPupusas.Models;
using ServerPupusas.ModelDTOs;

namespace ServerPupusas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Menu>>> GetAllMenuItems()
        {
            var menuItems = await _menuService.GetAllMenuItems();
            return Ok(menuItems);
        }

        [HttpPost("add")]
        public async Task<ActionResult<Menu>> AddMenuItem([FromBody] MenuDTO newMenu)
        {
            if (newMenu == null)
            {
                return BadRequest("Invalid menu item data.");
            }

            var menuItem = new Menu
            {
                Name = newMenu.Name,
                Description = newMenu.Description,
                Category = newMenu.Category,
                Price = newMenu.Price,
                Availability = newMenu.Availability ?? true,
                CreatedAt = DateTime.UtcNow
            };

            var updatedMenu = await _menuService.AddNewItemToMenu(newMenu);

            return CreatedAtAction(nameof(GetAllMenuItems), new { id = menuItem.MenuId }, updatedMenu);
        }
        [HttpPut("update/{id}")]
        public async Task<ActionResult<Menu>> UpdateMenuItem(int id, [FromBody] MenuDTO updatedMenu)
        {
            if (updatedMenu == null)
            {
                return BadRequest("Invalid menu item data.");
            }

            var menuItem = await _menuService.GetMenuItemById(id);
            if (menuItem == null)
            {
                return NotFound($"Menu item with ID {id} not found.");
            }

            menuItem.Name = updatedMenu.Name;
            menuItem.Description = updatedMenu.Description;
            menuItem.Category = updatedMenu.Category;
            menuItem.Price = updatedMenu.Price;
            menuItem.Availability = updatedMenu.Availability ?? menuItem.Availability;

            if (!ValidCategories.Categories.Contains(menuItem.Category))
            {
                return BadRequest($"La categoría '{menuItem.Category}' no es válida. Las categorías válidas son: {string.Join(", ", ValidCategories.Categories)}.");
            }

            var result = await _menuService.UpdateMenuItem(menuItem);

            if (!result)
            {
                return StatusCode(500, "Error updating menu item.");
            }

            return Ok(menuItem);
        }
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteMenuItem(int id)
        {
            var menuItem = await _menuService.GetMenuItemById(id);
            if (menuItem == null)
            {
                return NotFound($"Menu item with ID {id} not found.");
            }

            var result = await _menuService.DeleteMenuItem(id);

            if (!result)
            {
                return StatusCode(500, "Error deleting menu item.");
            }

            return NoContent();
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Menu>> GetMenuItemById(int id)
        {
            var menuItem = await _menuService.GetMenuItemById(id);
            if (menuItem == null)
            {
                return NotFound($"Menu item with ID {id} not found.");
            }

            return Ok(menuItem);
        }
    }
}
