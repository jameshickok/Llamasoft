namespace BakeryExercise.Controllers
{
    using BakeryExercise.Controllers.DTO;
    using BakeryExercise.EntityFramework;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("/api/menu/{menuId}/items")]
    public class MenuItemController : Controller
    {
        private readonly BakeryContext bakeryContext;

        public MenuItemController(BakeryContext bakeryContext)
        {
            this.bakeryContext = bakeryContext;
        }

        [HttpGet]
        public Task<ActionResult> GetMenuItems(Guid menuId)
        {
            return Task.FromResult<ActionResult>(
                this.Ok(this.bakeryContext.MenuItems
                    .Where(mi => mi.Menu.MenuId == menuId)
                    .Select(mi => new MenuItemDto
                    {
                        MenuItemId = mi.MenuItemId,
                        MenuId = mi.Menu.MenuId,
                        Name = mi.Name,
                        Price = mi.Price
                    })));
        }

        [HttpGet("{menuItemId}")]
        public async Task<ActionResult> GetMenuItemAsync(Guid menuItemId)
        {
            return await Task.FromResult<ActionResult>(
                this.Ok(this.bakeryContext.MenuItems
                    .Where(m => m.MenuItemId == menuItemId)
                    .Select(m => new MenuItemDto
                    {
                        MenuItemId = m.MenuItemId,
                        MenuId = m.Menu.MenuId,
                        Name = m.Name,
                        Price = m.Price
                    }).FirstOrDefaultAsync()));
        }

        [HttpPost]
        [Authorize(Policy = "IsAdmin")]
        public async Task<ActionResult> AddMenuItemAsync(Guid menuId, string name, double price)
        {
            var MenuForMenuItem = await this.bakeryContext
                .Menus
                .FindAsync(menuId);

            if (MenuForMenuItem == null)
            {
                return this.NotFound(menuId);
            }

            var NewMenuItem = new MenuItem
            {
                MenuItemId = Guid.NewGuid(),
                Menu = MenuForMenuItem,
                Name = name,
                Price = price
            };

            await this.bakeryContext
                .MenuItems
                .AddAsync(NewMenuItem);

            await this.bakeryContext
                .SaveChangesAsync();

            return this.CreatedAtAction("AddMenuItemAsync", new MenuItemDto
            {
                MenuItemId = NewMenuItem.MenuItemId,
                MenuId = menuId,
                Name = name,
                Price = price
            });
        }

        [HttpPut]
        [Authorize(Policy = "IsAdmin")]
        public async Task<ActionResult> UpdateMenuItemAsync(Guid menuItemId, double price)
        {
            var MenuItemToUpdate = await this.bakeryContext
                .MenuItems
                .FindAsync(menuItemId);

            if (MenuItemToUpdate == null)
            {
                return this.NotFound(menuItemId);
            }

            MenuItemToUpdate.Price = price;

            this.bakeryContext
                .MenuItems
                .Update(MenuItemToUpdate);

            await this.bakeryContext
                .SaveChangesAsync();

            return this.Ok(MenuItemToUpdate);
        }

        [HttpDelete]
        [Authorize(Policy = "IsAdmin")]
        public async Task<ActionResult> DeleteMenuItemAsync(Guid menuItemId)
        {
            var PortionToDelete = await this.bakeryContext
                .Portions
                .FindAsync(menuItemId);

            if (PortionToDelete == null)
            {
                return this.NotFound(menuItemId);
            }
            else
            {
                this.bakeryContext
                .Portions
                .Remove(PortionToDelete);

                await this.bakeryContext
                    .SaveChangesAsync();

                return this.Ok(menuItemId);
            }
        }
    }
}
