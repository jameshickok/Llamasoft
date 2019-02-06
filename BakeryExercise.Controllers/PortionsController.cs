namespace BakeryExercise.Controllers
{
    using BakeryExercise.Controllers.DTO;
    using BakeryExercise.EntityFramework;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("/api/menu/{menuId}/items/{menuItemId}/portions")]
    public class PortionsController : Controller
    {
        private readonly BakeryContext bakeryContext;

        public PortionsController(BakeryContext bakeryContext)
        {
            this.bakeryContext = bakeryContext;
        }

        [HttpGet]
        public Task<ActionResult> Get(Guid menuId, Guid menuItemId)
        {
            return Task.FromResult<ActionResult>(
                this.Ok(this.bakeryContext.MenuItemPortions
                    .Where(mip => mip.MenuItemId == menuItemId && mip.MenuItem.Menu.MenuId == menuId)
                    .Select(mip => new PortionDto {
                        PortionId = mip.PortionId,
                        FoodId = mip.Portion.Food.FoodId,
                        Amount = mip.Portion.Amount
                    })));
        }
    }
}
