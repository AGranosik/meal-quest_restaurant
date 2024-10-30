﻿using domain.Restaurants.Aggregates;
using domain.Restaurants.Aggregates.DomainEvents;
using domain.Restaurants.ValueObjects.Identifiers;

namespace infrastructure.EventStorage.DatabaseModels.Configurations
{
    internal class RestaurantEventConfiguration : DomainEventModelConfiguration<Restaurant, RestaurantId>
    {
    }
}
