﻿using infrastructure.Database.MenuContext;
using infrastructure.Database.MenuContext.Models.Configurations;
using infrastructure.Database.RestaurantContext;
using infrastructure.Database.RestaurantContext.Models.Configurations;
using infrastructure.EventStorage;
using Microsoft.EntityFrameworkCore;
using Respawn;
using Respawn.Graph;

namespace integrationTests.Restaurants
{
    public class BaseRestaurantIntegrationTests : BaseContainerIntegrationTests<RestaurantDbContext>
    {
        protected MenuDbContext _menuDbContext;
        protected EventDbContext _eventDbContext;
        public BaseRestaurantIntegrationTests()
        {
            
        }
        protected override async Task OneTimeSetUp()
        {
            await base.OneTimeSetUp();
            _connection = _dbContext.Database.GetDbConnection();
            await _connection.OpenAsync();

            _respawner = await Respawner.CreateAsync(_connection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                TablesToInclude = [.. _restaurantTables, new Table(MenuDatabaseConstants.SCHEMA, MenuDatabaseConstants.RESTAURANTS)],
                SchemasToInclude =
                [
                    "public",
                    RestaurantDatabaseConstants.SCHEMA,
                    MenuDatabaseConstants.SCHEMA
                ]
            });

            _menuDbContext = await GetDifferentDbContext<MenuDbContext>();
            _eventDbContext = await GetDifferentDbContext<EventDbContext>();
        }

        public override async Task OneTimeTearDown()
        {
            await base.OneTimeTearDown();
            _connection!.Dispose();
        }
    }
}
