namespace BakeryExercise.Controllers
{
    using BakeryExercise.Controllers.DTO;
    using BakeryExercise.EntityFramework;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
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
            return Task.FromResult<ActionResult>(
                this.Ok(this.BakeryContext.Menus
                    .Select(m => new MenuDto
                    {
                        MenuId = m.MenuId,
                        Name = m.Name,
                        Category = m.Category
                    })
                    .FirstOrDefault(m => m.MenuId == menuId)));
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
    }
}
