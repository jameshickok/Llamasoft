namespace BakeryExercise.UnitTests
{
    using BakeryExercise.Controllers;
    using BakeryExercise.Controllers.DTO;
    using BakeryExercise.EntityFramework;
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;
    
    public class PortionsControllerUnitTests : IDisposable
    {
        private readonly SqliteConnection connection;
        private readonly DbContextOptions<BakeryContext> contextOptions;

        public PortionsControllerUnitTests()
        {
            this.connection = new SqliteConnection("DataSource=:memory:");
            this.connection.Open();

            this.contextOptions = new DbContextOptionsBuilder<BakeryContext>()
                .UseSqlite(this.connection)
                .Options;

            using (var context = new BakeryContext(this.contextOptions))
            {
                context.Database.EnsureCreated();
                context.MenuItemPortions.RemoveRange(context.MenuItemPortions);
                context.MenuItems.RemoveRange(context.MenuItems);
                context.Menus.RemoveRange(context.Menus);
                context.Portions.RemoveRange(context.Portions);
                context.Food.RemoveRange(context.Food);
                context.SaveChanges();
            }
        }

        public void Dispose()
        {
            this.connection.Close();
        }

        [Fact]
        public async Task Get_ValidMenuAndMenuItem_ShouldReturnAllPortions()
        {
            // Arrange
            Menu newMenu;
            MenuItem newMenuItem;
            Food newFood;
            Portion newPortion;

            // We need to add a Menu with a MenuItem, A Portion with a Food, and then associate the Portion to the MenuItem
            using (var context = new BakeryContext(this.contextOptions))
            {
                // Add the Menu
                newMenu = new Menu
                {
                    MenuId = new Guid("f547cb11-af79-4942-bece-af172f488d1a"),
                    Name = "Menu1",
                    Category = "Test",
                    Items = new List<MenuItem>()
                };

                context.Menus.Add(newMenu);

                // Add the MenuItem
                newMenuItem = new MenuItem
                {
                    MenuItemId = new Guid("09e2a4c0-78f8-4fd1-8585-2fae8f5f8bf1"),
                    Name = "MenuItem1",
                    Price = 1.99
                };

                newMenu.Items.Add(newMenuItem);

                // Add the Food
                newFood = new Food
                {
                    FoodId = new Guid("bf13b0b8-0f13-4aeb-ab50-6af773340763"),
                    Name = "Food1",
                };

                context.Food.Add(newFood);

                // Add the Portion
                newPortion = new Portion
                {
                    PortionId = new Guid("b28f8536-931c-43fb-ba73-710513147e49"),
                    Food = newFood,
                    Amount = 2
                };

                context.Portions.Add(newPortion);

                // Associate the Portion with the MenuItem
                context.MenuItemPortions.Add(new MenuItemPortion
                {
                    MenuItemId = newMenuItem.MenuItemId,
                    PortionId = newPortion.PortionId
                });

                context.SaveChanges();
            }

            using (var context = new BakeryContext(this.contextOptions))
            {
                var portionsController = GetPortionsController(context);

                // Act
                var result = await portionsController.Get(newMenu.MenuId, newMenuItem.MenuItemId);
                var okResult = result as OkObjectResult;
                var resultValue = okResult.Value as IQueryable<PortionDto>;

                // Assert
                resultValue.Should().BeEquivalentTo(new PortionDto[] 
                {
                    new PortionDto { PortionId = newPortion.PortionId, FoodId = newFood.FoodId, Amount = 2 }
                });
            }
        }

        [Fact]
        [Trait("TODO", "FixMe")]
        public void Get_InvalidMenuAndMenuItem_ShouldReturnNotFound()
        {
            "You".Should().Be("adding a test here");
        }

        private static PortionsController GetPortionsController(BakeryContext bakeryContext, string requestHost = "localhost", string requestScheme = "http")
        {
            var portionsController = new PortionsController(bakeryContext);

            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(m => m.Host).Returns(new HostString(requestHost));
            mockRequest.Setup(m => m.Scheme).Returns(requestScheme);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.Request).Returns(mockRequest.Object);

            portionsController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            return portionsController;
        }
    }
}
