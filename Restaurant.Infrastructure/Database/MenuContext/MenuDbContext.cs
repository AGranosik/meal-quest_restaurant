using domain.Menus.Aggregates;
using domain.Menus.ValueObjects.Identifiers;
using infrastructure.Database.MenuContext.Models.Configurations;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Database.MenuContext
{
    internal class MenuDbContext(DbContextOptions<MenuDbContext> options) : DbContext(options)
    {
        public DbSet<Menu> Menus { get; set; }
        public DbSet<RestaurantIdMenuId> Restaurants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(MenuDatabaseConstants.SCHEMA);

            modelBuilder.ApplyConfiguration(new GroupConfiguration())
                .ApplyConfiguration(new IngredientConfiguration())
                .ApplyConfiguration(new MealConfiguration())
                .ApplyConfiguration(new RestaurantConfiguration())
                .ApplyConfiguration(new MenuConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
