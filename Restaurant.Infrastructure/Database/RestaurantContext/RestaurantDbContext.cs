﻿using domain.Restaurants.Aggregates;
using infrastructure.Database.RestaurantContext.Models.Configurations;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Database.RestaurantContext;

internal class RestaurantDbContext : DbContext
{
    public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
    {
    }

    public DbSet<Restaurant> Restaurants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(RestaurantDatabaseConstants.SCHEMA);

        modelBuilder
            .ApplyConfiguration(new AddressConfiguration())
            .ApplyConfiguration(new OpeningHoursConfiguration())
            .ApplyConfiguration(new OwnerConfiguration())
            .ApplyConfiguration(new RestaurantConfiguration())
            .ApplyConfiguration(new MenuConfiguration())
            .ApplyConfiguration(new WorkingDayConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}