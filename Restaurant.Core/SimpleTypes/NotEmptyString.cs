namespace Restaurant.Core.SimpleTypes
{
    public class NotEmptyString
    {
        private readonly string _value;
        public NotEmptyString(string value)
        {
            if(string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value cannot be null or empty");

            if(string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("value cannot be null or white spacec");

            _value = value;
        }

        public string Value => _value;

        public static implicit operator string(NotEmptyString value) => value.Value;
        public static explicit operator NotEmptyString(string value) => new(value);
    }
}
