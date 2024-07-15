using domain.Restaurants.ValueObjects;

namespace infrastructure.Database.RestaurantContext.Models
{
    internal class OpeningHours : domain.Restaurants.ValueObjects.OpeningHours
    {
        private OpeningHours(List<WorkingDay> workingDays) : base(workingDays) {}

        public OpeningHours() : base() { }
    }
}
