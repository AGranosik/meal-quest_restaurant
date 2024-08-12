using System.Reflection;
using domain.Menus.Aggregates.Entities;
using domain.Menus.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Database.MenuContext
{
    public class MenuDbContext(DbContextOptions<MenuDbContext> options) : DbContext(options)
    {
        //public DbSet<Menu> Menus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("menu");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
