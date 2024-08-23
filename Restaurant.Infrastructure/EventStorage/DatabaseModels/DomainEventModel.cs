using domain.Common.DomainImplementationTypes;

namespace infrastructure.EventStorage.DatabaseModels
{
    public class DomainEventModel<T>
        where T : DomainEvent
    {
        public DomainEventModel(int streamId, T data)
        {
            StreamId = streamId;
            Data = data;
        }
        private DomainEventModel() { }

        public int EventId { get; }
        public int StreamId { get; }
        public T Data { get; }
    }
}
