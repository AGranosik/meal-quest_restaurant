﻿using System.Net;
using System.Net.Http.Json;
using application.Restaurants.Queries.GetRestaurantQueries.Dtos;
using domain.Restaurants.Aggregates;
using domain.Restaurants.Aggregates.DomainEvents;
using domain.Restaurants.ValueObjects.Identifiers;
using FluentAssertions;
using integrationTests.Restaurants.DataMocks;
using Microsoft.EntityFrameworkCore;
using webapi.Controllers.Restaurants.Requests;

namespace integrationTests.Restaurants
{
    [TestFixture]
    public class RestaurantCreateEndpointTests : BaseRestaurantIntegrationTests
    {
        private const string _endpoint = "/api/Restaurant";

        [Test]
        public async Task CreateRestaurant_RequestIsNull_Repsonse()
        {
            var response = await _client.PostAsJsonAsync<CreateRestaurantRequest>(_endpoint, null, CancellationToken.None);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var anyRestaurants = await _dbContext.Restaurants.AnyAsync();
            anyRestaurants.Should().BeFalse();
        }

        [Test]
        public async Task CreateRestaurant_Valid_Created()
        {
            var request = RestaurantDataFaker.ValidRequest();

            var response = await _client.PostAsJsonAsync(_endpoint, request, CancellationToken.None);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var resultString = await response.Content.ReadAsStringAsync();
            var result = ApiResponseDeserializator.Deserialize<RestaurantId>(resultString);

            result.Should().NotBeNull();
            result!.Value.Should().BeGreaterThan(0);

            var anyRestaurants = await _dbContext.Restaurants.AnyAsync();
            anyRestaurants.Should().BeTrue();
        }

        // similar test for menu
        [Test]
        public async Task CreateRestaurant_Valid_AllMappedProperly()
        {
            var request = RestaurantDataFaker.ValidRequest();

            var response = await _client.PostAsJsonAsync(_endpoint, request, CancellationToken.None);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var resultString = await response.Content.ReadAsStringAsync();
            var result = ApiResponseDeserializator.Deserialize<RestaurantId>(resultString);

            result.Should().NotBeNull();
            result!.Value.Should().BeGreaterThan(0);

            var dbRestaurant = await _dbContext.Restaurants
                .Include(r => r.Owner)
                    .ThenInclude(o => o.Address)
                .Include(r => r.OpeningHours)
                    .ThenInclude(oh => oh.WorkingDays)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id! == result);

            CompareRestaurant(request, dbRestaurant)
                .Should().BeTrue();
        }

        [Test]
        public async Task CreateRestaurant_CreatedInMenuContext_Success()
        {
            var request = RestaurantDataFaker.ValidRequest();

            var response = await _client.PostAsJsonAsync(_endpoint, request, CancellationToken.None);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var resultString = await response.Content.ReadAsStringAsync();
            var result = ApiResponseDeserializator.Deserialize<RestaurantId>(resultString);


            var menuDb = await _menuDbContext.Restaurants
                .Where(r => r.Value == result!.Value)
                .ToListAsync();

            menuDb.Count.Should().Be(1);
        }

        [Test]
        public async Task CreateRestaurant_Valid_StoredInEventStore()
        {
            var request = RestaurantDataFaker.ValidRequest();

            //mvoe to method to extract response from request
            var response = await _client.PostAsJsonAsync(_endpoint, request, CancellationToken.None);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var resultString = await response.Content.ReadAsStringAsync();
            var result = ApiResponseDeserializator.Deserialize<RestaurantId>(resultString);


            var events = await _eventDbContext.GetDbSet<RestaurantEvent>()
                .Where(e => e.StreamId == result!.Value)
                .ToListAsync();

            events.Count.Should().Be(1);

            events[0].Data.Should().BeAssignableTo<RestaurantCreatedEvent>();
        }
        private static bool CompareRestaurant(CreateRestaurantRequest request, Restaurant db)
        {
            
            if (db.Owner.Name.Value != request.Owner!.Name!
                || db!.Owner!.Surname.Value != request.Owner!.Surname) return false;

            var dbAddress = db.Owner.Address;
            var requestAddress = request.Owner.Address;

            if (dbAddress.City.Value != requestAddress!.City!
                || dbAddress.Coordinates.X != requestAddress.XCoordinate
                || dbAddress.Coordinates.Y != requestAddress.YCoordinate)
                return false;

            if (!db.OpeningHours.WorkingDays.Any(dtoWd => request.OpeningHours.WorkingDays.Any(domainWWd => dtoWd.Day == domainWWd.Day))
                || db.OpeningHours.WorkingDays.Count != request.OpeningHours.WorkingDays.Count)
                return false;


            return true;
        }

    }
}
