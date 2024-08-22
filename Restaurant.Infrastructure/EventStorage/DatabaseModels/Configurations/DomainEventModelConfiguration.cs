using System.Text.Json;
using domain.Common.DomainImplementationTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace infrastructure.EventStorage.DatabaseModels.Configurations
{
    internal class DomainEventModelConfiguration<TEvent> : IEntityTypeConfiguration<DomainEventModel<TEvent>>
        where TEvent : DomainEvent

    {
        public virtual void Configure(EntityTypeBuilder<DomainEventModel<TEvent>> builder)
        {
            builder.ToTable(typeof(TEvent).Name + "s", "events");

            builder.Property(e => e.EventId)
                .ValueGeneratedOnAdd();

            builder.HasKey(e => e.EventId);

            builder.Property(e => e.StreamId)
                .IsRequired();

            var converter = new ValueConverter<TEvent, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<TEvent>(v, (JsonSerializerOptions)null));

            builder.Property(e => e.Data)
                .HasConversion(converter)
                .IsRequired()
                .HasColumnType("jsonb");
        }
    }
}
