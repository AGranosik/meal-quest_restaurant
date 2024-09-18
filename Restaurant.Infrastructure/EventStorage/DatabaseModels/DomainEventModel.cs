using System.Text.Json;
using domain.Common.DomainImplementationTypes;

namespace infrastructure.EventStorage.DatabaseModels
{
    public class DomainEventModel<T>
        where T : DomainEvent
    {
        private T? _data;
        public DomainEventModel(int streamId, T data, bool success)
        {
            _data = data;
            StreamId = streamId;
            Success = success;
            AssemblyName = data.GetAssemblyName();
            SerializedData = data.Serialize();
        }
        private DomainEventModel() { }

        public int EventId { get; }
        public int StreamId { get; }
        public bool Success { get; }
        public string? AssemblyName { get; }
        public string? SerializedData { get; set; }

        public T? Data => _data ?? (T?)JsonSerializer.Deserialize(SerializedData!, Type.GetType(AssemblyName!)!, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }
}
