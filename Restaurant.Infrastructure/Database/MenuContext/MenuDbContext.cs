﻿using domain.Menus.Aggregates;
using domain.Menus.Aggregates.Entities;
using domain.Menus.ValueObjects;
using domain.Menus.ValueObjects.Identifiers;
using infrastructure.Database.MenuContext.Models.Configurations;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Database.MenuContext;

internal class MenuDbContext : DbContext
{
    public MenuDbContext(DbContextOptions<MenuDbContext> options) : base(options)
    {
    }

    public DbSet<Menu> Menus { get; set; }
    public DbSet<MenuRestaurant> Restaurants { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(MenuDatabaseConstants.Schema);

        modelBuilder.ApplyConfiguration(new GroupConfiguration())
            .ApplyConfiguration(new IngredientConfiguration())
            .ApplyConfiguration(new MealConfiguration())
            .ApplyConfiguration(new RestaurantConfiguration())
            .ApplyConfiguration(new CategoryConfiguration())
            .ApplyConfiguration(new MenuConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}