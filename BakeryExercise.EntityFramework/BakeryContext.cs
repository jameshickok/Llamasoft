namespace BakeryExercise.EntityFramework
{
    using Microsoft.EntityFrameworkCore;

    public class BakeryContext : DbContext
    {
        public BakeryContext(DbContextOptions<BakeryContext> options) : base(options)
        {
        }

        protected BakeryContext() : base()
        {

        }

        public virtual DbSet<Menu> Menus { get; set; }

        public virtual DbSet<MenuItem> MenuItems { get; set; }

        public virtual DbSet<Portion> Portions { get; set; }

        public virtual DbSet<Food> Food { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<MenuItemPortion> MenuItemPortions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            ModelSchemaSetup.BuildSchema(builder);
            ModelDataSeeder.SeedData(builder);
        }
    }
}
