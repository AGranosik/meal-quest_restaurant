﻿using Restaurant.Core.SimpleTypes;
using Restaurant.Domain.Common.BaseTypes;

namespace Restaurant.Domain.Restaurants.ValueObjects
{
    public class City(string name) : ValueObject<City>
    {
        public readonly NotEmptyString CityName = name;
        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            City? other = obj as City;
            return other != null && CityName == other.CityName;
        }
    }
}
