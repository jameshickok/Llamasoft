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

    public class MenuItemControllerUnitTests : IDisposable
    {
        private readonly SqliteConnection connection;
        private readonly DbContextOptions<BakeryContext> contextOptions;

        public MenuItemControllerUnitTests()
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
        public async Task GetMenuItems_ValidMenuId_ShouldReturnAllMenuItemsFromMenuAsync()
        {
            // Arrange
            using (var context = new BakeryContext(this.contextOptions))
            {
                context.Menus.Add(new Menu
                {
                    MenuId = new Guid("f547cb11-af79-4942-bece-af172f488d1a"),
                    Name = "Menu1",
                    Category = "Test",
                    Items = new List<MenuItem>
                    {
                        new MenuItem { MenuItemId = new Guid("09e2a4c0-78f8-4fd1-8585-2fae8f5f8bf1"), Name = "MenuItem1", Price = 1.99 },
                        new MenuItem { MenuItemId = new Guid("e9509933-a327-4a91-9828-e3f66c057050"), Name = "MenuItem3", Price = 3.99 },
                    }
                });

                context.Menus.Add(new Menu
                {
                    MenuId = new Guid("72941934-39bc-4182-be57-6a217f286782"),
                    Name = "Menu2",
                    Category = "Test",
                    Items = new List<MenuItem>
                    {
                        new MenuItem { MenuItemId = new Guid("95a93c4b-9aab-4bd8-b025-541c25c76394"), Name = "MenuItem2", Price = 2.99 },
                    }
                });

                context.SaveChanges();
            }

            using (var context = new BakeryContext(this.contextOptions))
            {
                var menuItemController = GetMenuItemController(context);

                // Act
                var result = await menuItemController.GetMenuItems(new Guid("f547cb11-af79-4942-bece-af172f488d1a"));
                var okResult = result as OkObjectResult;
                var resultValue = okResult.Value as IQueryable<MenuItemDto>;

                // Assert
                resultValue.Should().BeEquivalentTo(new MenuItemDto[] {
                    new MenuItemDto {
                        MenuItemId = new Guid("09e2a4c0-78f8-4fd1-8585-2fae8f5f8bf1"),
                        MenuId = new Guid("f547cb11-af79-4942-bece-af172f488d1a"),
                        Name = "MenuItem1",
                        Price = 1.99
                    },
                    new MenuItemDto {
                        MenuItemId = new Guid("e9509933-a327-4a91-9828-e3f66c057050"),
                        MenuId = new Guid("f547cb11-af79-4942-bece-af172f488d1a"),
                        Name = "MenuItem3",
                        Price = 3.99
                    },
                });
            }
        }

        [Fact]
        [Trait("TODO", "FixMe")]
        public async Task Get_WithMenuIdThatDoesNotExist_ShouldReturnANotFoundResult()
        {
            using (var context = new BakeryContext(this.contextOptions))
            {
                // Arrange
                var menuController = GetMenuItemController(context);

                // Act
                // Note: No menus exist, so this should not be found
                var result = await menuController.GetMenuItems(new Guid("09e2a4c0-78f8-4fd1-8585-2fae8f5f8bf1"));

                // The Controller class has a method, this.NotFound(), 
                // See: https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.notfound?view=aspnetcore-2.2
                result.Should().BeOfType(typeof(NotFoundResult));
            }
        }

        private static MenuItemController GetMenuItemController(BakeryContext bakeryContext, string requestHost = "localhost", string requestScheme = "http")
        {
            var menuItemController = new MenuItemController(bakeryContext);

            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(m => m.Host).Returns(new HostString(requestHost));
            mockRequest.Setup(m => m.Scheme).Returns(requestScheme);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.Request).Returns(mockRequest.Object);

            menuItemController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            return menuItemController;
        }
    }
}
