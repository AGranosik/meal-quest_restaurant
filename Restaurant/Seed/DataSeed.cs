﻿using MassTransit.Mediator;
using webapi.Controllers.Restaurants.Requests;

namespace webapi.Seed
{
    public class DataSeed(IMediator mediator)
    {
        private readonly IMediator _mediator = mediator;

        public async Task Seed()
        {
            var restaurants = GenerateRestaurants(100);
            foreach(var restaurant in restaurants)
            {
                await _mediator.Send(restaurant);
            }
        }

        private List<CreateRestaurantRequest> GenerateRestaurants(int n)
        {
            Random rand = new Random();
            var result = new List<CreateRestaurantRequest>(n);
            var openingHours = new List<short> { 6, 7, 8, 9 };
            var closingHours = new List<short> { 20, 21, 22, 23 };
            for(var i =0; i < n; i++)
            {
                var owner = new CreateOwnerRequest($"owner-seed-name-{i}", $"owner-seed-surname-{i}", new CreateAddressRequest($"seed-street-{i}", $"city-seed-{i}", rand.NextDouble(), rand.NextDouble()));
                var openDays = Enumerable.Range(0, 7)
                    .Select(d => new WorkingDayRequest((DayOfWeek)d, new DateTime(2020, 12, 12, openingHours[i%openingHours.Count], 00, 00), new DateTime(2020, 12, 12, closingHours[i%closingHours.Count], 00, 00))).ToList();
                var openinghours = new OpeningHoursRequest(openDays);
                result.Add(new CreateRestaurantRequest($"Data-seed-{i}", owner, openinghours));


            }

            return result;
        }
    }


}
