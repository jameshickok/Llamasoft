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

    [Route("api/food")]
    public class FoodController : Controller
    {
        private readonly BakeryContext bakeryContext;

        public FoodController(BakeryContext bakeryContext)
        {
            this.bakeryContext = bakeryContext;
        }

        [HttpGet]
        public Task<ActionResult> Get()
        {
            return Task.FromResult<ActionResult>(
                this.Ok(this.bakeryContext.Food.Select(f => new FoodDto
                {
                    FoodId = f.FoodId,
                    Name = f.Name,
                    IsVegan = f.IsVegan
                })));
        }

        [HttpGet("{foodId}")]
        public async Task<ActionResult> GetFoodAsync(Guid foodId)
        {
            return await Task.FromResult<ActionResult>(
                this.Ok(this.bakeryContext.Food
                    .Where(f => f.FoodId == foodId)
                    .Select(f => new FoodDto
                    {
                        FoodId = f.FoodId,
                        Name = f.Name,
                        IsVegan = f.IsVegan
                    }).FirstOrDefaultAsync()));
        }

        [HttpPost]
        [Authorize(Policy = "IsAdmin")]
        public async Task<CreatedAtActionResult> AddFoodAsync(string name, bool isVegan)
        {
            var NewFood = new Food
            {
                FoodId = Guid.NewGuid(),
                Name = name,
                IsVegan = isVegan
            };

            await this.bakeryContext
                .Food
                .AddAsync(NewFood);

            await this.bakeryContext
                .SaveChangesAsync();

            return this.CreatedAtAction("AddFoodAsync", new FoodDto
            {
                FoodId = NewFood.FoodId,
                Name = NewFood.Name,
                IsVegan = NewFood.IsVegan
            });
        }
        
        [HttpDelete]
        [Authorize(Policy = "IsAdmin")]
        public async Task<ActionResult> DeleteFoodAsync(Guid foodId)
        {
            var FoodToDelete = await this.bakeryContext
                .Food
                .FindAsync(foodId);

            if (FoodToDelete == null)
            {
                return this.NotFound(foodId);
            }
            else
            {
                this.bakeryContext
                .Food
                .Remove(FoodToDelete);

                await this.bakeryContext
                    .SaveChangesAsync();

                return this.Ok(foodId);
            }
        }
    }
}
