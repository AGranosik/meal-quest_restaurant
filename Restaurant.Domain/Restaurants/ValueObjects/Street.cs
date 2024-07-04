﻿using core.SimpleTypes;
using domain.Common.BaseTypes;

namespace domain.Restaurants.ValueObjects
{
    public class Street(string streetName) : ValueObject<Street>
    {
        public NotEmptyString StreetName { get; } = streetName;

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            Street other = obj as Street;
            if (other == null) return false;
            return StreetName == other.StreetName;
        }
    }
}
