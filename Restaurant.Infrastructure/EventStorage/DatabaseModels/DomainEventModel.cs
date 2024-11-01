using System.Text.Json;
using domain.Common.BaseTypes;
using domain.Common.DomainImplementationTypes.Identifiers;

namespace infrastructure.EventStorage.DatabaseModels
{
    public class DomainEventModel<TAggregate, TKey>
        where TAggregate : Aggregate<TKey>
        where TKey : SimpleValueType<int, TKey>
    {
        private readonly JsonSerializerOptions _serializingOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        private readonly TAggregate? _data;

        public static DomainEventModel<TAggregate, TKey> Pending(TAggregate data)
            => new(data.Id!.Value, data);

        private DomainEventModel(int streamId, TAggregate data)
        {
            _data = data;
            StreamId = streamId;
            SerializedData = JsonSerializer.Serialize(data, _serializingOptions);
            AssemblyName = data.GetType().AssemblyQualifiedName;
            HandlingStatus = HandlingStatus.Pending;
        }
        private DomainEventModel() { }

        public int EventId { get; }
        public int StreamId { get; }
        public HandlingStatus HandlingStatus { get; set; }
        public string? SerializedData { get; }
        public string? AssemblyName { get; }
        public TAggregate? Data => _data ?? (TAggregate?)JsonSerializer.Deserialize(SerializedData!, Type.GetType(AssemblyName!)!, _serializingOptions);

        public void Failed()
            => HandlingStatus = HandlingStatus.Failed;

        public void Success()
            => HandlingStatus = HandlingStatus.Propagated;
    }

    public enum HandlingStatus
    {
        Pending, Propagated, Failed
    }
}
