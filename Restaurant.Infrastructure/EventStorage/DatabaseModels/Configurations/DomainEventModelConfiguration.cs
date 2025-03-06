using domain.Common.BaseTypes;
using domain.Common.DomainImplementationTypes.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.EventStorage.DatabaseModels.Configurations;

internal class DomainEventModelConfiguration<TAggregate, TKey> : IEntityTypeConfiguration<DomainEventModel<TAggregate, TKey>>
    where TAggregate : Aggregate<TKey>
    where TKey : SimpleValueType<int, TKey>
{
    public virtual void Configure(EntityTypeBuilder<DomainEventModel<TAggregate, TKey>> builder)
    {
        builder.ToTable(typeof(TAggregate).Name + "s", "events");

        builder.Property(e => e.EventId)
            .ValueGeneratedOnAdd();

        builder.HasKey(e => e.EventId);

        builder.Property(e => e.StreamId)
            .IsRequired();

        builder.Property(e => e.AssemblyName)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(e => e.SerializedData)
            .IsRequired();

        builder.Ignore(e => e.Data);
    }
}