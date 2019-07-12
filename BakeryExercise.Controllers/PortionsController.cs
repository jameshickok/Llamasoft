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

        [HttpGet("{portionId}")]
        public async Task<ActionResult> GetPortionAsync(Guid portionId)
        {
            return await Task.FromResult<ActionResult>(
                this.Ok(this.bakeryContext.Portions
                    .Where(p => p.PortionId == portionId)
                    .Select(p => new PortionDto
                    {
                        PortionId = p.PortionId,
                        FoodId = p.Food.FoodId,
                        Amount = p.Amount
                    }).FirstOrDefaultAsync()));
        }

        [HttpPost]
        [Authorize(Policy = "IsAdmin")]
        public async Task<ActionResult> AddPortionAsync(Guid foodId, int amount)
        {
            var FoodForPortion = await this.bakeryContext
                .Food
                .FindAsync(foodId);

            if(FoodForPortion == null)
            {
                return this.NotFound(foodId);
            }

            var NewPortion = new Portion
            {
                PortionId = Guid.NewGuid(),
                Amount = amount,
                Food = FoodForPortion
            };

            await this.bakeryContext
                .Portions
                .AddAsync(NewPortion);

            await this.bakeryContext
                .SaveChangesAsync();

            return this.CreatedAtAction("AddPortionAsync", new PortionDto
            {
                PortionId = NewPortion.PortionId,
                FoodId = NewPortion.Food.FoodId,
                Amount = NewPortion.Amount
            });
        }

        /// <summary>
        /// A portion size can change, whereas a food is a value object.
        /// </summary>
        /// <param name="portionId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Policy = "IsAdmin")]
        public async Task<ActionResult> UpdatePortionAsync(Guid portionId, int amount)
        {
            var PortionToUpdate = await this.bakeryContext
                .Portions
                .FindAsync(portionId);

            if(PortionToUpdate == null)
            {
                return this.NotFound(portionId);
            }

            PortionToUpdate.Amount = amount;

            this.bakeryContext
                .Portions
                .Update(PortionToUpdate);

            await this.bakeryContext
                .SaveChangesAsync();

            return this.Ok(PortionToUpdate);
        }

        [HttpDelete]
        [Authorize(Policy = "IsAdmin")]
        public async Task<ActionResult> DeletePortionAsync(Guid portionId)
        {
            var PortionToDelete = await this.bakeryContext
                .Portions
                .FindAsync(portionId);

            if (PortionToDelete == null)
            {
                return this.NotFound(portionId);
            }
            else
            {
                this.bakeryContext
                .Portions
                .Remove(PortionToDelete);

                await this.bakeryContext
                    .SaveChangesAsync();

                return this.Ok(portionId);
            }
        }
    }
}
