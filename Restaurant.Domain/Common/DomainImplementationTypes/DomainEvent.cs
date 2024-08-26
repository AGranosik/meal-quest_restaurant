using domain.Common.BaseTypes;

namespace domain.Common.DomainImplementationTypes
{
    public abstract record DomainEvent(int? Id)
    {
        public abstract string Serialize();
        public abstract string GetAssemblyName();
        public int? StreamId { get; private set; } = Id;
        public void SetId(int streamId) => StreamId = streamId;
    }
}
