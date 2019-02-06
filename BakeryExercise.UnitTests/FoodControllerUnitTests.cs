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
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;
    
    public class FoodControllerUnitTests : IDisposable
    {
        private readonly SqliteConnection connection;
        private readonly DbContextOptions<BakeryContext> contextOptions;

        public FoodControllerUnitTests()
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
        public async Task Get_NoParameters_ShouldReturnAllFoodsAsync()
        {
            // Arrange
            using (var context = new BakeryContext(this.contextOptions))
            {
                context.Add(new Food { FoodId = new Guid("09e2a4c0-78f8-4fd1-8585-2fae8f5f8bf1"), Name = "Food1", IsVegan = false });
                context.Add(new Food { FoodId = new Guid("95a93c4b-9aab-4bd8-b025-541c25c76394"), Name = "Food2", IsVegan = false });
                context.Add(new Food { FoodId = new Guid("e9509933-a327-4a91-9828-e3f66c057050"), Name = "Food3", IsVegan = false });
                context.SaveChanges();
            }

            using (var context = new BakeryContext(this.contextOptions))
            {
                var foodController = GetFoodController(context);

                // Act
                var result = await foodController.Get();
                var okResult = result as OkObjectResult;
                var resultValue = okResult.Value as IQueryable<FoodDto>;

                // Assert
                resultValue.Should().BeEquivalentTo(new FoodDto[] {
                    new FoodDto { FoodId = new Guid("09e2a4c0-78f8-4fd1-8585-2fae8f5f8bf1"), Name = "Food1", IsVegan = false },
                    new FoodDto { FoodId = new Guid("95a93c4b-9aab-4bd8-b025-541c25c76394"), Name = "Food2", IsVegan = false },
                    new FoodDto { FoodId = new Guid("e9509933-a327-4a91-9828-e3f66c057050"), Name = "Food3", IsVegan = false },
                });
            }
        }

        private static FoodController GetFoodController(BakeryContext bakeryContext, string requestHost = "localhost", string requestScheme = "http")
        {
            var foodController = new FoodController(bakeryContext);

            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(m => m.Host).Returns(new HostString(requestHost));
            mockRequest.Setup(m => m.Scheme).Returns(requestScheme);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.Request).Returns(mockRequest.Object);

            foodController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            return foodController;
        }
    }
}
