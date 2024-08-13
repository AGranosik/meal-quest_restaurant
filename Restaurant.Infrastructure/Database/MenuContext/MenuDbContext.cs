using domain.Menus.Aggregates.Entities;
using infrastructure.Database.MenuContext.Models.Configurations;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Database.MenuContext
{
    public class MenuDbContext(DbContextOptions<MenuDbContext> options) : DbContext(options)
    {
        public DbSet<Menu> Menus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("menu");

            modelBuilder.ApplyConfiguration(new GroupConfiguration())
                .ApplyConfiguration(new IngredientConfiguration())
                .ApplyConfiguration(new MealConfiguration())
                .ApplyConfiguration(new MenuConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
