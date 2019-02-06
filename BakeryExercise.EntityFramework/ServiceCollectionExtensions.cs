namespace BakeryExercise.EntityFramework
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBakeryPersistence(this IServiceCollection serviceCollection) => 
            serviceCollection
                .AddDbContext<BakeryContext>(
                    dbOptions => dbOptions.UseSqlite("Data Source=bakery.db"));
    }
}
