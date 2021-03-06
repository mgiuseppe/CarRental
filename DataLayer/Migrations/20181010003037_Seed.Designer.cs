﻿// <auto-generated />
using System;
using CarRentalNovility.DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CarRentalNovility.DataLayer.Migrations
{
    [DbContext(typeof(CarRentalDbContext))]
    [Migration("20181010003037_Seed")]
    partial class Seed
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CarRentalNovility.Entities.Car", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("PlateNumber");

                    b.Property<int?>("TypeId");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.ToTable("Cars");

                    b.HasData(
                        new { Id = 1L, PlateNumber = "AA00BB", TypeId = 1 },
                        new { Id = 2L, PlateNumber = "AA01BB", TypeId = 1 },
                        new { Id = 3L, PlateNumber = "AA00SP", TypeId = 2 }
                    );
                });

            modelBuilder.Entity("CarRentalNovility.Entities.CarType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("CancellationRateFee");

                    b.Property<decimal>("DepositFee");

                    b.Property<string>("Name");

                    b.Property<decimal>("RentalRateFee");

                    b.HasKey("Id");

                    b.ToTable("CarTypes");

                    b.HasData(
                        new { Id = 1, CancellationRateFee = 5m, DepositFee = 0.2m, Name = "Family Car", RentalRateFee = 10m },
                        new { Id = 2, CancellationRateFee = 50m, DepositFee = 0.2m, Name = "Sport Car", RentalRateFee = 100m }
                    );
                });

            modelBuilder.Entity("CarRentalNovility.Entities.Client", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email");

                    b.Property<string>("FullName");

                    b.Property<string>("PhoneNumber");

                    b.HasKey("Id");

                    b.ToTable("Clients");

                    b.HasData(
                        new { Id = 1L, Email = "giuseppem@test.com", FullName = "Giuseppe M", PhoneNumber = "3476010101" },
                        new { Id = 2L, Email = "barban@test.com", FullName = "Barba N", PhoneNumber = "3476010202" }
                    );
                });

            modelBuilder.Entity("CarRentalNovility.Entities.ClientAccount", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("CancellationFeePaid");

                    b.Property<decimal>("CancellationFeeValueAtBookingMoment");

                    b.Property<decimal>("DepositFeePaid");

                    b.Property<decimal>("RentalFeePaid");

                    b.Property<decimal>("RentalRateFeeValueAtBookingMoment");

                    b.HasKey("Id");

                    b.ToTable("ClientAccount");

                    b.HasData(
                        new { Id = 1L, CancellationFeePaid = 0m, CancellationFeeValueAtBookingMoment = 5m, DepositFeePaid = 2.0m, RentalFeePaid = 0m, RentalRateFeeValueAtBookingMoment = 10m }
                    );
                });

            modelBuilder.Entity("CarRentalNovility.Entities.Reservation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("CarId");

                    b.Property<long?>("ClientAccountId");

                    b.Property<long?>("ClientId");

                    b.Property<DateTime>("PickUpDateTime");

                    b.Property<DateTime>("ReturnDateTime");

                    b.Property<int>("State");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.HasIndex("ClientAccountId");

                    b.HasIndex("ClientId");

                    b.ToTable("Reservations");

                    b.HasData(
                        new { Id = 1L, CarId = 1L, ClientAccountId = 1L, ClientId = 1L, PickUpDateTime = new DateTime(2018, 10, 10, 2, 30, 35, 737, DateTimeKind.Local), ReturnDateTime = new DateTime(2018, 10, 10, 10, 30, 35, 739, DateTimeKind.Local), State = 0 }
                    );
                });

            modelBuilder.Entity("CarRentalNovility.Entities.Car", b =>
                {
                    b.HasOne("CarRentalNovility.Entities.CarType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId");
                });

            modelBuilder.Entity("CarRentalNovility.Entities.Reservation", b =>
                {
                    b.HasOne("CarRentalNovility.Entities.Car", "Car")
                        .WithMany()
                        .HasForeignKey("CarId");

                    b.HasOne("CarRentalNovility.Entities.ClientAccount", "ClientAccount")
                        .WithMany()
                        .HasForeignKey("ClientAccountId");

                    b.HasOne("CarRentalNovility.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId");
                });
#pragma warning restore 612, 618
        }
    }
}
