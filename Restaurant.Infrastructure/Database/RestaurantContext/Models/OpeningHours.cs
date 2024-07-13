namespace infrastructure.Database.RestaurantContext.Models
{
    internal class OpeningHours : domain.Restaurants.ValueObjects.OpeningHours
    {
        private OpeningHours(TimeOnly from, TimeOnly to): base(TimeOnly.MinValue, TimeOnly.MaxValue) {}

        public OpeningHours() : base() { }
    }
}
