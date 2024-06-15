namespace Restaurant.Core.SimpleTypes
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
            => left.Value == rigtt.Value;

        public static bool operator !=(NotEmptyString left, NotEmptyString rigtt)
            => !(left == rigtt);

        public string Value => _value;

        public static implicit operator NotEmptyString(string value) => new(value);
    }
}
