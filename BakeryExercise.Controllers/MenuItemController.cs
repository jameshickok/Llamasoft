namespace BakeryExercise.Controllers
{
    using BakeryExercise.Controllers.DTO;
    using BakeryExercise.EntityFramework;
    using Microsoft.AspNetCore.Mvc;
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
    }
}
