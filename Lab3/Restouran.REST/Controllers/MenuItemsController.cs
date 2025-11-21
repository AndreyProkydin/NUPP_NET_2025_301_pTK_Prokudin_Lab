using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restouran.Infrastructure;
using Restouran.Infrastructure.Models;
using Restouran.REST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restouran.REST.Controllers
{

    [ApiController]
    [Route("api/[controller]")] 
    public class MenuItemsController : ControllerBase
    {

        private readonly IRepository<MenuItem> _menuItemRepository;
        private readonly IRepository<Tag> _tagRepository;
        private readonly ICrudServiceAsync<MenuItem> _menuItemService;

        public MenuItemsController(
            IRepository<MenuItem> menuItemRepository,
            IRepository<Tag> tagRepository,
            ICrudServiceAsync<MenuItem> menuItemservice)
        {
            _menuItemRepository = menuItemRepository;
            _tagRepository = tagRepository;
            _menuItemService = menuItemservice;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItemModel>>> GetMenuItems()
        {
            var menuItems = await _menuItemRepository.GetAll()
                .Include(mi => mi.MenuItemTags) 
                .ThenInclude(mit => mit.Tag) 
                .ToListAsync();

            var menuItemModels = menuItems.Select(mi => new MenuItemModel
            {
                IdItem = mi.IdItem,
                Name = mi.Name,
                Description = mi.Description,
                Price = mi.Price,
                Tags = mi.MenuItemTags.Select(mit => new TagModel
                {
                    Id = mit.Tag.Id,
                    Name = mit.Tag.Name
                }).ToList()
            });

            return Ok(menuItemModels); 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItemModel>> GetMenuItem(int id)
        {
            var menuItem = await _menuItemRepository.GetAll()
                .Include(mi => mi.MenuItemTags)
                .ThenInclude(mit => mit.Tag)
                .FirstOrDefaultAsync(mi => mi.IdItem == id);

            if (menuItem == null)
            {
                return NotFound();
            }

            var menuItemModel = new MenuItemModel
            {
                IdItem = menuItem.IdItem,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                Tags = menuItem.MenuItemTags.Select(mit => new TagModel
                {
                    Id = mit.Tag.Id,
                    Name = mit.Tag.Name
                }).ToList()
            };

            return Ok(menuItemModel);
        }

        [HttpPost]
        public async Task<ActionResult<MenuItemModel>> PostMenuItem(MenuItemRequestModel requestModel)
        {
            var newMenuItem = new MenuItem
            {
                Name = requestModel.Name,
                Description = requestModel.Description,
                Price = requestModel.Price
            };

            var tags = await _tagRepository.GetAll()
                .Where(t => requestModel.TagIds.Contains(t.Id))
                .ToListAsync();

            foreach (var tag in tags)
            {
                newMenuItem.MenuItemTags.Add(new MenuItemTag { Tag = tag });
            }

            await _menuItemService.CreateAsync(newMenuItem);

            var createdModel = new MenuItemModel
            {
                IdItem = newMenuItem.IdItem,
                Name = newMenuItem.Name,
                Description = newMenuItem.Description,
                Price = newMenuItem.Price,
                Tags = tags.Select(t => new TagModel { Id = t.Id, Name = t.Name }).ToList()
            };

            return CreatedAtAction(nameof(GetMenuItem), new { id = createdModel.IdItem }, createdModel); 
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMenuItem(int id, MenuItemRequestModel requestModel)
        {
            var menuItemToUpdate = await _menuItemRepository.GetAll()
                .Include(mi => mi.MenuItemTags)
                .FirstOrDefaultAsync(mi => mi.IdItem == id);

            if (menuItemToUpdate == null)
            {
                return NotFound(); 
            }

            menuItemToUpdate.Name = requestModel.Name;
            menuItemToUpdate.Description = requestModel.Description;
            menuItemToUpdate.Price = requestModel.Price;

            menuItemToUpdate.MenuItemTags.Clear();

            var tags = await _tagRepository.GetAll()
                .Where(t => requestModel.TagIds.Contains(t.Id))
                .ToListAsync();

            foreach (var tag in tags)
            {
                menuItemToUpdate.MenuItemTags.Add(new MenuItemTag { Tag = tag });
            }

            await _menuItemService.UpdateAsync(menuItemToUpdate);

            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var menuItemToDelete = await _menuItemRepository.GetByIdAsync(id);
            if (menuItemToDelete == null)
            {
                return NotFound(); 
            }

            await _menuItemService.RemoveAsync(menuItemToDelete);

            return NoContent(); 
        }
    }
}
