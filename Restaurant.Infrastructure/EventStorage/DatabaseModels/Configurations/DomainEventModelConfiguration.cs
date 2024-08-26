using domain.Common.DomainImplementationTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

            builder.Property(e => e.AssemblyName)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(e => e.Success);

            builder.Property(e => e.SerializedData)
                .IsRequired();

            builder.Ignore(e => e.Data);
        }
    }
}
