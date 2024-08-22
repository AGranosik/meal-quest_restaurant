using domain.Common.DomainImplementationTypes;

namespace infrastructure.EventStorage.DatabaseModels
{
    public class DomainEventModel<T>
        where T : DomainEvent
    {
        private DomainEventModel() { }

        public int EventId { get; set; }
        public int StreamId { get; set; }
        public T Data { get; set; }
    }
}
