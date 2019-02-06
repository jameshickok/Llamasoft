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
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class MenuControllerUnitTests : IDisposable
    {
        private readonly SqliteConnection connection;
        private readonly DbContextOptions<BakeryContext> contextOptions;

        public MenuControllerUnitTests()
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
        public async Task Get_NoParameters_ShouldReturnAllMenusAsMenuDtos()
        {
            // Arrange
            using (var context = new BakeryContext(this.contextOptions))
            {
                context.Add(new Menu { MenuId = new Guid("09e2a4c0-78f8-4fd1-8585-2fae8f5f8bf1"), Name = "Menu1", Category = "Menu1Category" });
                context.Add(new Menu { MenuId = new Guid("95a93c4b-9aab-4bd8-b025-541c25c76394"), Name = "Menu2", Category = "Menu2Category" });
                context.Add(new Menu { MenuId = new Guid("e9509933-a327-4a91-9828-e3f66c057050"), Name = "Menu3", Category = "Menu3Category" });
                context.SaveChanges();
            }
            
            using (var context = new BakeryContext(this.contextOptions))
            {
                var menuController = GetMenuController(context);

                // Act
                var result = await menuController.Get();
                var okResult = result as OkObjectResult;
                var resultValue = okResult.Value as IQueryable<MenuDto>;

                // Assert
                resultValue.Should().BeEquivalentTo(new MenuDto[] {
                    new MenuDto { MenuId = new Guid("09e2a4c0-78f8-4fd1-8585-2fae8f5f8bf1"), Name = "Menu1", Category = "Menu1Category" },
                    new MenuDto { MenuId = new Guid("95a93c4b-9aab-4bd8-b025-541c25c76394"), Name = "Menu2", Category = "Menu2Category" },
                    new MenuDto { MenuId = new Guid("e9509933-a327-4a91-9828-e3f66c057050"), Name = "Menu3", Category = "Menu3Category" },
                });
            }
        }

        [Fact]
        public async Task Add_NewMenu_ShouldAddAMenuToTheDatabase()
        {
            // Arrange
            var menuToAdd = new MenuDto { Name = "Menu1", Category = "Menu1Category" };

            using (var context = new BakeryContext(this.contextOptions))
            {
                var menuController = GetMenuController(context);
                
                // Act
                var result = await menuController.AddMenu(menuToAdd);

                // Assert
                var addedMenu = context.Menus.First();
                addedMenu.MenuId.Should().NotBeEmpty();
                addedMenu.Name.Should().Be("Menu1");
                addedMenu.Category.Should().Be("Menu1Category");

                var createdResult = result as CreatedResult;
                createdResult.Location.Should().Be($"http://localhost/api/menu/{addedMenu.MenuId}");
            }
        }

        [Fact]
        public async Task Get_WithMenuId_ShouldReturnTheMenu()
        {
            // Arrange
            using (var context = new BakeryContext(this.contextOptions))
            {
                context.Add(new Menu { MenuId = new Guid("09e2a4c0-78f8-4fd1-8585-2fae8f5f8bf1"), Name = "Menu1", Category = "Menu1Category" });
                context.SaveChanges();
            }

            using (var context = new BakeryContext(this.contextOptions))
            {
                var menuController = GetMenuController(context);

                // Act
                var result = await menuController.Get(new Guid("09e2a4c0-78f8-4fd1-8585-2fae8f5f8bf1"));
                var okResult = result as OkObjectResult;
                var resultValue = okResult.Value as MenuDto;

                // Assert
                resultValue.Should().BeEquivalentTo(new MenuDto { MenuId = new Guid("09e2a4c0-78f8-4fd1-8585-2fae8f5f8bf1"), Name = "Menu1", Category = "Menu1Category" });
            }
        }
       
        [Fact]
        [Trait("TODO", "FixMe")]
        public async Task Get_WithMenuIdThatDoesNotExist_ShouldReturnANotFoundResult()
        {
            using (var context = new BakeryContext(this.contextOptions))
            {
                // Arrange
                var menuController = GetMenuController(context);

                // Act
                // Note: No menus exist, so this should not be found
                var result = await menuController.Get(new Guid("09e2a4c0-78f8-4fd1-8585-2fae8f5f8bf1"));

                // The Controller class has a method, this.NotFound(), 
                // See: https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.notfound?view=aspnetcore-2.2
                result.Should().BeOfType(typeof(NotFoundResult));
            }
        }
            
        private static MenuController GetMenuController(BakeryContext bakeryContext, string requestHost = "localhost", string requestScheme = "http")
        {
            var menuController = new MenuController(bakeryContext);

            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(m => m.Host).Returns(new HostString(requestHost));
            mockRequest.Setup(m => m.Scheme).Returns(requestScheme);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.Request).Returns(mockRequest.Object);

            menuController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            return menuController;
        }
    }
}
