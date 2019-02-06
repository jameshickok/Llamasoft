namespace BakeryExercise.Controllers
{
    using BakeryExercise.Controllers.DTO;
    using BakeryExercise.EntityFramework;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
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
    }
}
