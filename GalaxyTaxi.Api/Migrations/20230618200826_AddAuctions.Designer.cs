﻿// <auto-generated />
using System;
using GalaxyTaxi.Api.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GalaxyTaxi.Api.Migrations
{
    [DbContext(typeof(Db))]
    [Migration("20230618200826_AddAuctions")]
    partial class AddAuctions
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GalaxyTaxi.Api.Database.Models.Account", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<int>("AccountTypeId")
                        .HasColumnType("integer");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("GalaxyTaxi.Api.Database.Models.Address", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<decimal>("Latitude")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("numeric");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("GalaxyTaxi.Api.Database.Models.Auction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<long>("CurrentWinnerId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsFinished")
                        .HasColumnType("boolean");

                    b.Property<long>("JourneyId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CurrentWinnerId");

                    b.HasIndex("JourneyId");

                    b.ToTable("Auctions");
                });

            modelBuilder.Entity("GalaxyTaxi.Api.Database.Models.Bid", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("AccountId")
                        .HasColumnType("bigint");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<long>("AuctionId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("AuctionId");

                    b.ToTable("Bids");
                });

            modelBuilder.Entity("GalaxyTaxi.Api.Database.Models.CustomerCompany", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("AccountId")
                        .HasColumnType("bigint");

                    b.Property<string>("IdentificationCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("MaxAmountPerEmployee")
                        .HasColumnType("double precision");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CustomerCompany");
                });

            modelBuilder.Entity("GalaxyTaxi.Api.Database.Models.Employee", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("CustomerCompanyId")
                        .HasColumnType("bigint");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("OfficeId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CustomerCompanyId");

                    b.HasIndex("OfficeId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("GalaxyTaxi.Api.Database.Models.EmployeeAddress", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("AddressId")
                        .HasColumnType("bigint");

                    b.Property<long>("EmployeeId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("EmployeeAddresses");
                });

            modelBuilder.Entity("GalaxyTaxi.Api.Database.Models.Journey", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("CustomerCompanyId")
                        .HasColumnType("bigint");

                    b.Property<long>("OfficeId")
                        .HasColumnType("bigint");

                    b.Property<long>("VendorCompanyId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CustomerCompanyId");

                    b.HasIndex("OfficeId");

                    b.HasIndex("VendorCompanyId");

                    b.ToTable("Journeys");
                });

            modelBuilder.Entity("GalaxyTaxi.Api.Database.Models.Office", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("AddressId")
                        .HasColumnType("bigint");

                    b.Property<long>("CustomerCompanyId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("WorkingEndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("WorkingStartTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("CustomerCompanyId");

                    b.ToTable("Offices");
                });

            modelBuilder.Entity("GalaxyTaxi.Api.Database.Models.Stop", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("EmployeeAddressId")
                        .HasColumnType("bigint");

                    b.Property<long>("JourneyId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeAddressId");

                    b.HasIndex("JourneyId");

                    b.ToTable("Stops");
                });

            modelBuilder.Entity("GalaxyTaxi.Api.Database.Models.VendorCompany", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("AccountId")
                        .HasColumnType("bigint");

                    b.Property<string>("IdentificationCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("VendorCompany");
                });

            modelBuilder.Entity("GalaxyTaxi.Api.Database.Models.Auction", b =>
                {
                    b.HasOne("GalaxyTaxi.Api.Database.Models.VendorCompany", "CurrentWinner")
                        .WithMany()
                        .HasForeignKey("CurrentWinnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GalaxyTaxi.Api.Database.Models.Journey", "Journey")
                        .WithMany()
                        .HasForeignKey("JourneyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CurrentWinner");

                    b.Navigation("Journey");
                });

            modelBuilder.Entity("GalaxyTaxi.Api.Database.Models.Bid", b =>
                {
                    b.HasOne("GalaxyTaxi.Api.Database.Models.Account", "Account")
                        .WithMany()
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GalaxyTaxi.Api.Database.Models.Auction", "Auction")
                        .WithMany("Bids")
                        .HasForeignKey("AuctionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Auction");
                });

            modelBuilder.Entity("GalaxyTaxi.Api.Database.Models.CustomerCompany", b =>
                {
                    b.HasOne("GalaxyTaxi.Api.Database.Models.Account", "Account")
                        .WithMany()
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("GalaxyTaxi.Api.Database.Models.Employee", b =>
                {
                    b.HasOne("GalaxyTaxi.Api.Database.Models.CustomerCompany", "CustomerCompany")
                        .WithMany("Employees")
                        .HasForeignKey("CustomerCompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GalaxyTaxi.Api.Database.Models.Office", "Office")
                        .WithMany("Employees")
                        .HasForeignKey("OfficeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CustomerCompany");

                    b.Navigation("Office");
                });

            modelBuilder.Entity("GalaxyTaxi.Api.Database.Models.EmployeeAddress", b =>
                {
                    b.HasOne("GalaxyTaxi.Api.Database.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GalaxyTaxi.Api.Database.Models.Employee", "Employee")
                        .WithMany("Addresses")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("GalaxyTaxi.Api.Database.Models.Journey", b =>
                {
                    b.HasOne("GalaxyTaxi.Api.Database.Models.CustomerCompany", "CustomerCompany")
                        .WithMany()
                        .HasForeignKey("CustomerCompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GalaxyTaxi.Api.Database.Models.Office", "Office")
                        .WithMany()
                        .HasForeignKey("OfficeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GalaxyTaxi.Api.Database.Models.VendorCompany", "VendorCompany")
                        .WithMany()
                        .HasForeignKey("VendorCompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CustomerCompany");

                    b.Navigation("Office");

                    b.Navigation("VendorCompany");
                });

            modelBuilder.Entity("GalaxyTaxi.Api.Database.Models.Office", b =>
                {
                    b.HasOne("GalaxyTaxi.Api.Database.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GalaxyTaxi.Api.Database.Models.CustomerCompany", "CustomerCompany")
                        .WithMany("Offices")
                        .HasForeignKey("CustomerCompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("CustomerCompany");
                });

            modelBuilder.Entity("GalaxyTaxi.Api.Database.Models.Stop", b =>
                {
                    b.HasOne("GalaxyTaxi.Api.Database.Models.EmployeeAddress", "EmployeeAddress")
                        .WithMany()
                        .HasForeignKey("EmployeeAddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GalaxyTaxi.Api.Database.Models.Journey", "Journey")
                        .WithMany("Jobs")
                        .HasForeignKey("JourneyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EmployeeAddress");

                    b.Navigation("Journey");
                });

            modelBuilder.Entity("GalaxyTaxi.Api.Database.Models.VendorCompany", b =>
                {
                    b.HasOne("GalaxyTaxi.Api.Database.Models.Account", "Account")
                        .WithMany()
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("GalaxyTaxi.Api.Database.Models.Auction", b =>
                {
                    b.Navigation("Bids");
                });

            modelBuilder.Entity("GalaxyTaxi.Api.Database.Models.CustomerCompany", b =>
                {
                    b.Navigation("Employees");

                    b.Navigation("Offices");
                });

            modelBuilder.Entity("GalaxyTaxi.Api.Database.Models.Employee", b =>
                {
                    b.Navigation("Addresses");
                });

            modelBuilder.Entity("GalaxyTaxi.Api.Database.Models.Journey", b =>
                {
                    b.Navigation("Jobs");
                });

            modelBuilder.Entity("GalaxyTaxi.Api.Database.Models.Office", b =>
                {
                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}
