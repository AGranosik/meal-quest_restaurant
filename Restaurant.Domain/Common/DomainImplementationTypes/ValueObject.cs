﻿namespace Restaurant.Domain.Common.BaseTypes
{
    public abstract class ValueObject<T> where T : ValueObject<T>
    {
        public abstract override bool Equals(object? obj);
        public static bool operator ==(ValueObject<T> left, ValueObject<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValueObject<T> left, ValueObject<T> right)
        {
            return !(left == right);
        }
    }
}