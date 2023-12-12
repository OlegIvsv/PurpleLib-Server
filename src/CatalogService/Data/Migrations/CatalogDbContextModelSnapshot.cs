﻿// <auto-generated />
using System;
using CatalogService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CatalogService.Data.Migrations
{
    [DbContext(typeof(CatalogDbContext))]
    partial class CatalogDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CatalogService.Entities.CatalogItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("FloraId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("OfferEndsAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("OriginalPrice")
                        .HasColumnType("integer");

                    b.Property<string>("Seller")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("SoldAmount")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("FloraId")
                        .IsUnique();

                    b.ToTable("CatalogItems");
                });

            modelBuilder.Entity("CatalogService.Entities.Flora", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Color")
                        .HasColumnType("text");

                    b.Property<DateTime?>("LifeTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Flora");
                });

            modelBuilder.Entity("CatalogService.Entities.FloraPicture", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("FloraId")
                        .HasColumnType("uuid");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FloraId");

                    b.ToTable("FloraPicture");
                });

            modelBuilder.Entity("CatalogService.Entities.CatalogItem", b =>
                {
                    b.HasOne("CatalogService.Entities.Flora", "Flora")
                        .WithOne("CatalogItem")
                        .HasForeignKey("CatalogService.Entities.CatalogItem", "FloraId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Flora");
                });

            modelBuilder.Entity("CatalogService.Entities.FloraPicture", b =>
                {
                    b.HasOne("CatalogService.Entities.Flora", "Flora")
                        .WithMany("Pictures")
                        .HasForeignKey("FloraId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Flora");
                });

            modelBuilder.Entity("CatalogService.Entities.Flora", b =>
                {
                    b.Navigation("CatalogItem")
                        .IsRequired();

                    b.Navigation("Pictures");
                });
#pragma warning restore 612, 618
        }
    }
}
