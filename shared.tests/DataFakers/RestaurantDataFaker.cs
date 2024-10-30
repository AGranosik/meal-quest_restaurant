using domain.Common.ValueTypes.Strings;
using domain.Restaurants.Aggregates;
using domain.Restaurants.Aggregates.Entities;
using domain.Restaurants.ValueObjects;
using domain.Restaurants.ValueObjects.Identifiers;

namespace sharedTests.DataFakers
{
    public static class RestaurantDataFaker
    {
        public static Name ValidRestaurantName
            => new("test");

        public static Owner ValidOwner
            => Owner.Create(new Name("test"), new Name("surname"), Address.Create(new Street("street"), new City("city"), new Coordinates(10, 10)).Value).Value;

        public static OpeningHours ValidOpeningHours
            => OpeningHours.Create(
            [
                WorkingDay.Create(DayOfWeek.Monday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                WorkingDay.FreeDay(DayOfWeek.Tuesday).Value,
                WorkingDay.Create(DayOfWeek.Wednesday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                WorkingDay.Create(DayOfWeek.Thursday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                WorkingDay.Create(DayOfWeek.Friday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                WorkingDay.Create(DayOfWeek.Saturday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
                WorkingDay.Create(DayOfWeek.Sunday, new TimeOnly(12, 00), new TimeOnly(14, 00)).Value,
            ]).Value;

        public static Restaurant ValidRestaurant()
        {
            var result = Restaurant.Create(ValidRestaurantName, ValidOwner, ValidOpeningHours).Value;
            result.SetId(new RestaurantId(2));
            return result;
        }
    }
}
