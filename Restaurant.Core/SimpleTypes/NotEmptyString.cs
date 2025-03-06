using System.Security.AccessControl;

namespace core.SimpleTypes;

public class NotEmptyString
{
    public NotEmptyString(string value)
    {
        if(string.IsNullOrEmpty(value))
            throw new ArgumentException("value cannot be null or empty");

        if(string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("value cannot be null or white spacec");

        Value = value; 
    }

    public static bool operator ==(NotEmptyString? left, NotEmptyString? right)
    {
        if (left is null || right is null) return false;
        if (ReferenceEquals(left, right)) return true;

        return left.Value == right.Value;
    }

    public static bool operator !=(NotEmptyString? left, NotEmptyString? right)
        => !(left == right);

    public string Value { get; }

    public static implicit operator NotEmptyString(string value) => new(value);

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;

        var other = obj as NotEmptyString;
        if (other == null) return false;
        return Value == other.Value;
    }
}