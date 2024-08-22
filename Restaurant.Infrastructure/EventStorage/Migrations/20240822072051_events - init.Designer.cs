﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using infrastructure.EventStorage;

#nullable disable

namespace infrastructure.EventStorage.Migrations
{
    [DbContext(typeof(EventdbContext))]
    [Migration("20240822072051_events - init")]
    partial class eventsinit
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("events")
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("infrastructure.EventStorage.DatabaseModels.DomainEventModel<domain.Restaurants.Aggregates.DomainEvents.RestaurantEvent>", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("EventId"));

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<int>("StreamId")
                        .HasColumnType("integer");

                    b.HasKey("EventId");

                    b.ToTable("RestaurantEvents", "events");
                });
#pragma warning restore 612, 618
        }
    }
}
