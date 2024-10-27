namespace unitTests.DataFakers
{
    public class MockedException : Exception
    {
        public MockedException() : base("Mocked exception.")
        {
        }

        public MockedException(string? message) : base(message)
        {
        }

        public MockedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
