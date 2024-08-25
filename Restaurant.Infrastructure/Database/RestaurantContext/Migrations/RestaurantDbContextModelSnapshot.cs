﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using infrastructure.Database.RestaurantContext;

#nullable disable

namespace infrastructure.Database.RestaurantContext.Migrations
{
    [DbContext(typeof(RestaurantDbContext))]
    partial class RestaurantDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("restuarant")
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("domain.Restaurants.Aggregates.Entities.Menu", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("RestaurantId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId");

                    b.ToTable("Menus", "restaurant");
                });

            modelBuilder.Entity("domain.Restaurants.Aggregates.Entities.Owner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AddressID")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AddressID");

                    b.ToTable("Owners", "restaurant");
                });

            modelBuilder.Entity("domain.Restaurants.Aggregates.Restaurant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("OpeningHoursID")
                        .HasColumnType("integer");

                    b.Property<int>("OwnerId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OpeningHoursID");

                    b.HasIndex("OwnerId");

                    b.ToTable("Restaurants", "restaurant");
                });

            modelBuilder.Entity("domain.Restaurants.ValueObjects.Address", b =>
                {
                    b.Property<int>("AddressID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AddressID"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("AddressID");

                    b.ToTable("Addresses", "restaurant");
                });

            modelBuilder.Entity("domain.Restaurants.ValueObjects.OpeningHours", b =>
                {
                    b.Property<int>("OpeningHoursID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("OpeningHoursID"));

                    b.HasKey("OpeningHoursID");

                    b.ToTable("OpeningHours", "restaurant");
                });

            modelBuilder.Entity("domain.Restaurants.ValueObjects.WorkingDay", b =>
                {
                    b.Property<int>("WorkingDayID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("WorkingDayID"));

                    b.Property<int>("Day")
                        .HasColumnType("integer");

                    b.Property<bool>("Free")
                        .HasColumnType("boolean");

                    b.Property<TimeSpan>("From")
                        .HasColumnType("interval");

                    b.Property<int?>("OpeningHoursID")
                        .HasColumnType("integer");

                    b.Property<TimeSpan>("To")
                        .HasColumnType("interval");

                    b.HasKey("WorkingDayID");

                    b.HasIndex("OpeningHoursID");

                    b.ToTable("WorkingDays", "restaurant");
                });

            modelBuilder.Entity("domain.Restaurants.Aggregates.Entities.Menu", b =>
                {
                    b.HasOne("domain.Restaurants.Aggregates.Restaurant", null)
                        .WithMany("Menus")
                        .HasForeignKey("RestaurantId");
                });

            modelBuilder.Entity("domain.Restaurants.Aggregates.Entities.Owner", b =>
                {
                    b.HasOne("domain.Restaurants.ValueObjects.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("domain.Restaurants.Aggregates.Restaurant", b =>
                {
                    b.HasOne("domain.Restaurants.ValueObjects.OpeningHours", "OpeningHours")
                        .WithMany()
                        .HasForeignKey("OpeningHoursID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("domain.Restaurants.Aggregates.Entities.Owner", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OpeningHours");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("domain.Restaurants.ValueObjects.Address", b =>
                {
                    b.OwnsOne("domain.Restaurants.ValueObjects.Address.Coordinates#domain.Restaurants.ValueObjects.Coordinates", "Coordinates", b1 =>
                        {
                            b1.Property<int>("AddressID")
                                .HasColumnType("integer");

                            b1.Property<double>("X")
                                .HasColumnType("double precision");

                            b1.Property<double>("Y")
                                .HasColumnType("double precision");

                            b1.HasKey("AddressID");

                            b1.ToTable("Addresses", "restaurant");

                            b1.WithOwner()
                                .HasForeignKey("AddressID");
                        });

                    b.Navigation("Coordinates")
                        .IsRequired();
                });

            modelBuilder.Entity("domain.Restaurants.ValueObjects.WorkingDay", b =>
                {
                    b.HasOne("domain.Restaurants.ValueObjects.OpeningHours", null)
                        .WithMany("WorkingDays")
                        .HasForeignKey("OpeningHoursID");
                });

            modelBuilder.Entity("domain.Restaurants.Aggregates.Restaurant", b =>
                {
                    b.Navigation("Menus");
                });

            modelBuilder.Entity("domain.Restaurants.ValueObjects.OpeningHours", b =>
                {
                    b.Navigation("WorkingDays");
                });
#pragma warning restore 612, 618
        }
    }
}
