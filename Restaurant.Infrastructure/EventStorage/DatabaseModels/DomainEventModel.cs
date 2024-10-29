using System.Reflection;
using System.Text.Json;
using domain.Common.DomainImplementationTypes;

namespace infrastructure.EventStorage.DatabaseModels
{
    public class DomainEventModel<T>
        where T : DomainEvent
    {
        private readonly JsonSerializerOptions _serializingOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        private readonly T? _data;

        public static DomainEventModel<T> Pending(int streamId, T data)
            => new(streamId, data);

        private DomainEventModel(int streamId, T data)
        {
            _data = data;
        }
        private DomainEventModel() { }

        public int EventId { get; }
        public int StreamId { get; }
        public HandlingStatus HandlingStatus { get; set; }
        public string? SerializedData { get; }
        public string? Reason { get; private set; }
        public string? AssemblyName { get; }
        public T? Data => _data ?? (T?)JsonSerializer.Deserialize(SerializedData!, Type.GetType(AssemblyName!)!, _serializingOptions);

        public void Failed(string reason)
        {
            Reason = reason;
            HandlingStatus = HandlingStatus.Failed;
        }

        public void Success()
            => HandlingStatus = HandlingStatus.Propagated;
    }

    public enum HandlingStatus
    {
        Pending, Propagated, Failed
    }
}
