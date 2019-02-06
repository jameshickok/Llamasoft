namespace BakeryExercise.EntityFramework
{
    using Microsoft.EntityFrameworkCore;
    using System;

    public static class ModelDataSeeder
    {
        private static object CreateMenu(string name, string category, out dynamic menu)
        {
            menu = new
            {
                MenuId = Guid.NewGuid(),
                Name = name,
                Category = category,
            };
            return menu;
        }

        private static object CreateMenuItem(Guid menuId, string name, double price, out dynamic menuItem)
        {
            menuItem = new
            {
                MenuItemId = Guid.NewGuid(),
                MenuId = menuId,
                Name = name,
                Price = price
            };
            return menuItem;
        } 

        private static object CreateFood(string name, bool isVegan, out dynamic food)
        {
            food = new
            {
                FoodId = Guid.NewGuid(),
                Name = name,
                IsVegan = isVegan
            };
            return food;
        }

        private static object CreatePortion(Guid foodId, int amount, out dynamic portion)
        {
            portion = new
            {
                PortionId = Guid.NewGuid(),
                FoodId = foodId,
                Amount = amount
            };
            return portion;
        }

        public static void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User
            {
                UserId = Guid.NewGuid(),
                UserName = "admin@bakery.com",
                Password = "password",
                IsAdmin = true
            });

            modelBuilder.Entity<Menu>().HasData(
                CreateMenu("Everyday Breakfast", "Breakfast", out dynamic everyDayBreakfastMenu)
            );

            modelBuilder.Entity<MenuItem>().HasData(new[]
            {
                CreateMenuItem(everyDayBreakfastMenu.MenuId, "Single Doughnut", 1.99, out dynamic singleDoughnutItem),
                CreateMenuItem(everyDayBreakfastMenu.MenuId, "Dozen Doughnuts", 10.99, out dynamic dozenDoughnutsItem),
                CreateMenuItem(everyDayBreakfastMenu.MenuId, "Single Muffin", 2.99, out dynamic singleMuffinItem),
                CreateMenuItem(everyDayBreakfastMenu.MenuId, "Dozen Muffins", 13.99, out dynamic dozenMuffinsItem),
                CreateMenuItem(everyDayBreakfastMenu.MenuId, "Single Bagel", 0.99, out dynamic singleBagelItem),
                CreateMenuItem(everyDayBreakfastMenu.MenuId, "Dozen Bagels", 9.99, out dynamic dozenBagelsItem),
                CreateMenuItem(everyDayBreakfastMenu.MenuId, "Huge Spread", 39.99, out dynamic hugeSpreadItem),
            });
        
            modelBuilder.Entity<Food>().HasData(new[]
            {
                CreateFood("Doughnut", false, out dynamic doughnut),
                CreateFood("Muffin", false, out dynamic muffin),
                CreateFood("Bagel", false, out dynamic bagel),
            });

            modelBuilder.Entity<Portion>().HasData(new[]
            {
                CreatePortion(doughnut.FoodId, 1, out dynamic singleDoughnutPortion),
                CreatePortion(doughnut.FoodId, 12, out dynamic dozenDoughnutPortion),
                CreatePortion(muffin.FoodId, 1, out dynamic singleMuffinPortion),
                CreatePortion(muffin.FoodId, 12, out dynamic dozenMuffinPortion),
                CreatePortion(bagel.FoodId, 1, out dynamic singleBagelPortion),
                CreatePortion(bagel.FoodId, 12, out dynamic dozenBagelPortion),
            });

            modelBuilder.Entity<MenuItemPortion>().HasData(new[]
            {
                new { singleDoughnutItem.MenuItemId, singleDoughnutPortion.PortionId },
                new { dozenDoughnutsItem.MenuItemId, dozenDoughnutPortion.PortionId },
                new { singleMuffinItem.MenuItemId, singleMuffinPortion.PortionId },
                new { dozenMuffinsItem.MenuItemId, dozenMuffinPortion.PortionId },
                new { singleBagelItem.MenuItemId, singleBagelPortion.PortionId },
                new { dozenBagelsItem.MenuItemId, dozenBagelPortion.PortionId },
                new { hugeSpreadItem.MenuItemId, dozenDoughnutPortion.PortionId },
                new { hugeSpreadItem.MenuItemId, dozenMuffinPortion.PortionId },
                new { hugeSpreadItem.MenuItemId, dozenBagelPortion.PortionId },
            });
        }
    }
}
