namespace BakeryExercise.EntityFramework
{
    using Microsoft.EntityFrameworkCore;

    public static class ModelSchemaSetup
    {
        public static void BuildSchema(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>().HasKey(p => p.MenuId);
            modelBuilder.Entity<Menu>().HasIndex(p => p.Name);
            modelBuilder.Entity<Menu>().HasMany(m => m.Items);
            modelBuilder.Entity<Menu>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<Menu>().Property(p => p.Category).IsRequired();

            modelBuilder.Entity<MenuItem>().HasKey(p => p.MenuItemId);
            modelBuilder.Entity<MenuItem>().HasOne(p => p.Menu);
            modelBuilder.Entity<MenuItem>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<MenuItem>().Property(p => p.Price).IsRequired();

            modelBuilder.Entity<Portion>().HasKey(p => p.PortionId);
            modelBuilder.Entity<Portion>().HasOne(p => p.Food);
            modelBuilder.Entity<Portion>().Property(p => p.PortionId);
            modelBuilder.Entity<Portion>().Property(p => p.Amount).IsRequired();

            modelBuilder.Entity<MenuItemPortion>().HasKey(mip => new { mip.MenuItemId, mip.PortionId });

            modelBuilder.Entity<Food>().HasKey(p => p.FoodId);
            modelBuilder.Entity<Food>().Property(p => p.FoodId);
            modelBuilder.Entity<Food>().HasIndex(p => p.Name);

            modelBuilder.Entity<User>().HasKey(p => p.UserId);
            modelBuilder.Entity<User>().HasIndex(u => u.UserName);
            modelBuilder.Entity<User>().Property(p => p.Password).IsRequired();
            modelBuilder.Entity<User>().Property(p => p.UserName).IsRequired();            
        }
    }
}
