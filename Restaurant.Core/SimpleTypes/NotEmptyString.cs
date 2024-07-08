using System.Security.AccessControl;

namespace core.SimpleTypes
{
    public class NotEmptyString
    {
        private readonly string _value;
        public NotEmptyString(string value)
        {
            if(string.IsNullOrEmpty(value))
                throw new ArgumentException("value cannot be null or empty");

            if(string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("value cannot be null or white spacec");

            _value = value;
        }

        public static bool operator ==(NotEmptyString left, NotEmptyString rigtt)
        {
            if (left is null || rigtt is null) return false;
            if (ReferenceEquals(left, rigtt)) return true;

            return left.Value == rigtt.Value;
        }

        public static bool operator !=(NotEmptyString left, NotEmptyString rigtt)
            => !(left == rigtt);

        public string Value => _value;

        public static implicit operator NotEmptyString(string value) => new(value);

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            NotEmptyString other = obj as NotEmptyString;
            if (other == null) return false;
            return Value == other.Value;
        }
    }
}
