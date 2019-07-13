namespace BakeryExercise.Controllers
{
    using BakeryExercise.Controllers.DTO;
    using BakeryExercise.EntityFramework;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/menu/")]
    public class MenuController : Controller
    {
        public BakeryContext BakeryContext { get; set; }

        public MenuController(BakeryContext bakeryContext)
        {
            this.BakeryContext = bakeryContext;
        }
        
        [HttpGet]
        public Task<ActionResult> Get()
        {
            return Task.FromResult<ActionResult>(
                this.Ok(this.BakeryContext.Menus
                    .Select(m => new MenuDto
                    {
                        MenuId = m.MenuId,
                        Name = m.Name,
                        Category = m.Category
                    })));
        }

        [HttpGet("{menuId}")]
        public Task<ActionResult> Get(Guid menuId)
        {
            var MenuDto = this.BakeryContext.Menus
                    .Select(m => new MenuDto
                    {
                        MenuId = m.MenuId,
                        Name = m.Name,
                        Category = m.Category
                    })
                    .FirstOrDefault(m => m.MenuId == menuId);

            if(MenuDto == null)
            {
                return Task.FromResult<ActionResult>(
                this.NotFound(menuId));
            }

            return Task.FromResult<ActionResult>(
                this.Ok(MenuDto));
        }
        
        [Authorize(Policy = "IsAdmin")]
        [HttpPost]
        public async Task<ActionResult> AddMenu([FromBody] MenuDto menu)
        {
            var newMenu = new Menu
            {
                Name = menu.Name,
                Category = menu.Category,
            };
            this.BakeryContext.Menus.Add(newMenu);
            await this.BakeryContext.SaveChangesAsync();
            return this.Created(new Uri($"{this.Request.Scheme}://{this.Request.Host}/api/menu/{newMenu.MenuId}"), null);
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpPut]
        public async Task<ActionResult> DeleteMenu(Guid menuId)
        {
            //The idea here is to delete the lowest level items first and work up the heirarchy, checking each level to see whether any other entities are related.
            // EntityFramework can help with this by applying CascadeOnDelete(bool) within its fluent API configuration.
            // HOWEVER, if the below Menu item is simply removed with a cascading delete, then it might possibly delete items that are unintended.
            //TODO: If I had more time, then I would look into refactoring this into more abstract, reusable code.
            var MenuToDelete = await this.BakeryContext
                .Menus
                .Include(x => x.Items)
                .ThenInclude(x => x.MenuItemPortions)
                .ThenInclude(x => x.Portion)
                .ThenInclude(x => x.Food)
                .FirstOrDefaultAsync(x => x.MenuId == menuId);

            if(MenuToDelete == null)
            {
                return this.NotFound(menuId);
            }

            var MenuItemsToDelete = MenuToDelete.Items;

            var MenuItemPortionsToDelete = MenuItemsToDelete
                .Select(x => x.MenuItemPortions)
                .ToList();

            //At this point, we are no longer necessarily connected to a menu, so we want to check them first.
            var PortionsToCheck = MenuToDelete
                .Items
                .SelectMany(x => x.MenuItemPortions, (p, c) => c.Portion)
                .ToList();

            var FoodsToCheck = PortionsToCheck
                .Select(x => x.Food)
                .ToList();
            
            //Are any of these foods contained in other portions?
            //Possible scalability issue: Each iteration hits the database context, so a lambda JOIN would be a better route, but 
            // you cannot perform a JOIN on context entities with an IEnumerable implementation in memory, so the entire list of Portion entities would need to be eager loaded
            // before JOINING to FoodsToCheck, for example.
            // In this use case, the assumption is made that an average given menu will not have that many associated foods in contrast to eager loading all Portions.
            foreach (var FoodItem in FoodsToCheck)
            {
                var AllPortionsWithThisfood = await this.BakeryContext
                    .Portions
                    .Include(x => x.Food)
                    .Where(x => x.Food.FoodId == FoodItem.FoodId)
                    .ToListAsync();

                var IsThisFoodSafeToDelete = !AllPortionsWithThisfood
                    .Except(PortionsToCheck)
                    .Any();

                if(IsThisFoodSafeToDelete)
                {
                    this.BakeryContext.Food.Remove(FoodItem);
                }
            }
            await this.BakeryContext.SaveChangesAsync();

            foreach(var PortionItem in PortionsToCheck)
            {
                var AllMenuItemPortionsWithThisPortion = await this.BakeryContext
                    .MenuItemPortions
                    .Include(x => x.Portion)
                    .Where(x => x.PortionId == PortionItem.PortionId)
                    .ToListAsync();
                /*
                var IsThisPortionSafeToDelete = !AllMenuItemPortionsWithThisPortion
                    .Except(MenuItemPortionsToDelete)
                    .Any();
                    */

            }
            //TODO: Remaining functionality of this method.
            return this.Ok(menuId);
        }
    }
}
