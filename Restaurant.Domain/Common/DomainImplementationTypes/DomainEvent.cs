using domain.Common.BaseTypes;

namespace domain.Common.DomainImplementationTypes
{
    public abstract class DomainEvent
    {
        protected DomainEvent(int? streamId)
        {
            StreamId = streamId;
        }
        public abstract string Serialize();
        public abstract string GetAssemblyName();
        public int? StreamId { get; private set; }
        public void SetId(int streamId) => StreamId = streamId;
    }
}
