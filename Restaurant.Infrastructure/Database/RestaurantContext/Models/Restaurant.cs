namespace infrastructure.Database.RestaurantContext.Models
{
    internal class Restaurant : domain.Restaurants.Aggregates.Restaurant
    {
        public Restaurant() : base(){ }

        public static Restaurant CastToDbModel(domain.Restaurants.Aggregates.Restaurant domain)
        {
            return new Restaurant
            {
                Id = domain.Id,
                _menus = domain.Menus.Select(m => new domain.Restaurants.ValueObjects.Identifiers.MenuRestaurantId(m.RestaurantId, m.Name)).ToList(),
                OpeningHours = domain.OpeningHours,
                Owner = domain.Owner,
            };
        }

        public static domain.Restaurants.Aggregates.Restaurant CastToDomainModel(Restaurant dbModel)
            => dbModel;
    }
}
