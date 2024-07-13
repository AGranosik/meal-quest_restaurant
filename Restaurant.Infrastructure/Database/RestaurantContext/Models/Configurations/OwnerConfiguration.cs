﻿using domain.Common.ValueTypes.Strings;
using domain.Restaurants.ValueObjects.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Database.RestaurantContext.Models.Configurations
{
    internal class OwnerConfiguration : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            builder.ToTable("Owners", "restaurant");

            builder.Property(o => o.Id)
                .HasConversion(owner => owner.Value, db => new OwnerId(db));

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Name)
                .HasConversion(owner => owner.Value.Value, db => new Name(db));

            builder.Property(o => o.Surname)
                .HasConversion(owner => owner.Value.Value, db => new Name(db));

            builder.HasOne<Address>("Address")
                .WithMany();
        }
    }
}
