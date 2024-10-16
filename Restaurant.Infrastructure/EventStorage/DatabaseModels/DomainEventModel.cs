using System.Text.Json;
using domain.Common.DomainImplementationTypes;

namespace infrastructure.EventStorage.DatabaseModels
{
    public class DomainEventModel<T>
        where T : DomainEvent
    {
        private T? _data;

        public static DomainEventModel<T> Pending(int streamId, T data)
            => new(streamId, data);

        private DomainEventModel(int streamId, T data)
        {
            _data = data;
            StreamId = streamId;
            AssemblyName = data.GetAssemblyName();
            SerializedData = data.Serialize();
            PropgationStatus = EventProapgationStatus.Pending;
        }
        private DomainEventModel() { }

        public int EventId { get; }
        public int StreamId { get; }
        public EventProapgationStatus PropgationStatus { get; private set; }
        public string? AssemblyName { get; }
        public string? SerializedData { get; }
        public string? Reason { get; private set; }

        public T? Data => _data ?? (T?)JsonSerializer.Deserialize(SerializedData!, Type.GetType(AssemblyName!)!, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        public void Failed(string reason)
        {
            Reason = reason;
            PropgationStatus = EventProapgationStatus.Failed;
        }

        public void Success()
            => PropgationStatus = EventProapgationStatus.Propagated;
    }

    public enum EventProapgationStatus
    {
        Pending, Propagated, Failed
    }
}
