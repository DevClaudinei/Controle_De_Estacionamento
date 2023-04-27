﻿// <auto-generated />
using System;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Data.Migrations.Data
{
    [DbContext(typeof(DataContext))]
    [Migration("20230427231642_inserindo_tabelas_no_banco")]
    partial class inserindo_tabelas_no_banco
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.16");

            modelBuilder.Entity("DomainModels.Entities.Parking", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("AmountToPay")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("ArrivalTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DepartureTime")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("ParkingTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Plate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("PriceId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TimeToBeBilled")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PriceId");

                    b.ToTable("Parking", (string)null);
                });

            modelBuilder.Entity("DomainModels.Entities.Price", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("CurrentValue")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("FinalTerm")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("InitialTerm")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Price", (string)null);
                });

            modelBuilder.Entity("DomainModels.Entities.Parking", b =>
                {
                    b.HasOne("DomainModels.Entities.Price", "Price")
                        .WithMany("Parkings")
                        .HasForeignKey("PriceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Price");
                });

            modelBuilder.Entity("DomainModels.Entities.Price", b =>
                {
                    b.Navigation("Parkings");
                });
#pragma warning restore 612, 618
        }
    }
}