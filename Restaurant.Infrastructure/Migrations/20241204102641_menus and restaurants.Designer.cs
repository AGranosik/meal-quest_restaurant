﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using infrastructure.EventStorage;

#nullable disable

namespace infrastructure.Migrations
{
    [DbContext(typeof(EventDbContext))]
    [Migration("20241204102641_menus and restaurants")]
    partial class menusandrestaurants
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

            modelBuilder.Entity("infrastructure.EventStorage.DatabaseModels.DomainEventModel<domain.Menus.Aggregates.Menu, domain.Menus.ValueObjects.Identifiers.MenuId>", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("EventId"));

                    b.Property<string>("AssemblyName")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<int>("HandlingStatus")
                        .HasColumnType("integer");

                    b.Property<string>("SerializedData")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("StreamId")
                        .HasColumnType("integer");

                    b.HasKey("EventId");

                    b.ToTable("Menus", "events");
                });

            modelBuilder.Entity("infrastructure.EventStorage.DatabaseModels.DomainEventModel<domain.Restaurants.Aggregates.Restaurant, domain.Restaurants.ValueObjects.Identifiers.RestaurantId>", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("EventId"));

                    b.Property<string>("AssemblyName")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<int>("HandlingStatus")
                        .HasColumnType("integer");

                    b.Property<string>("SerializedData")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("StreamId")
                        .HasColumnType("integer");

                    b.HasKey("EventId");

                    b.ToTable("Restaurants", "events");
                });
#pragma warning restore 612, 618
        }
    }
}
