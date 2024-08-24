using domain.Common.DomainImplementationTypes;

namespace infrastructure.EventStorage.DatabaseModels
{
    public class DomainEventModel<T>
        where T : DomainEvent
    {
        public DomainEventModel(int streamId, T data, bool success)
        {
            StreamId = streamId;
            Data = data;
            Success = success;
        }
        private DomainEventModel() { }

        public int EventId { get; }
        public int StreamId { get; }
        public bool Success { get; set; }
        public T Data { get; }
    }
}
