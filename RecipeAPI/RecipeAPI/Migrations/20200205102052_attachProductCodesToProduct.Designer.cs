﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RecipeAPI.Models;

namespace RecipeAPI.Migrations
{
    [DbContext(typeof(RecipeContext))]
    [Migration("20200205102052_attachProductCodesToProduct")]
    partial class attachProductCodesToProduct
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PantryTracker.Model.Pantry.PantryTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ProductId");

                    b.Property<double>("Quantity");

                    b.Property<string>("Size");

                    b.Property<int>("TransactionType");

                    b.Property<string>("Unit");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("PantryTracker.Model.Products.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DefaultUnit");

                    b.Property<string>("Name");

                    b.Property<string>("OwnerId");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("PantryTracker.Model.Products.ProductCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Brand");

                    b.Property<string>("Code");

                    b.Property<string>("Description");

                    b.Property<string>("OwnerId");

                    b.Property<int?>("ProductId");

                    b.Property<string>("Size");

                    b.Property<string>("Unit");

                    b.Property<int?>("VarietyId");

                    b.Property<string>("Vendor");

                    b.Property<string>("VendorCode");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("VarietyId");

                    b.HasIndex("Code", "OwnerId")
                        .HasName("UniqueProductCodeByUser");

                    b.ToTable("ProductCodes");
                });

            modelBuilder.Entity("PantryTracker.Model.Products.ProductVariety", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<int>("ProductId");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Varieties");
                });

            modelBuilder.Entity("PantryTracker.Model.Products.UserProductPreference", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ProductId");

                    b.Property<Guid>("RecipeId");

                    b.Property<string>("matchingText");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("RecipeId");

                    b.ToTable("UserProductPreferences");
                });

            modelBuilder.Entity("PantryTracker.Model.Recipes.Direction", b =>
                {
                    b.Property<Guid>("RecipeId");

                    b.Property<int>("Index");

                    b.Property<string>("Text");

                    b.HasKey("RecipeId", "Index");

                    b.ToTable("Directions");
                });

            modelBuilder.Entity("PantryTracker.Model.Recipes.Ingredient", b =>
                {
                    b.Property<Guid>("RecipeId");

                    b.Property<int>("Index");

                    b.Property<string>("Container");

                    b.Property<string>("Descriptor");

                    b.Property<string>("Name");

                    b.Property<string>("Quantity");

                    b.Property<string>("SubQuantity");

                    b.Property<string>("Unit");

                    b.HasKey("RecipeId", "Index");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("PantryTracker.Model.Recipes.Recipe", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body");

                    b.Property<string>("Credit");

                    b.Property<string>("OwnerId");

                    b.Property<string>("PrepTime");

                    b.Property<string>("PublicState");

                    b.Property<string>("RawText");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("PantryTracker.Model.Pantry.PantryTransaction", b =>
                {
                    b.HasOne("PantryTracker.Model.Products.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PantryTracker.Model.Products.ProductCode", b =>
                {
                    b.HasOne("PantryTracker.Model.Products.Product", "Product")
                        .WithMany("Codes")
                        .HasForeignKey("ProductId");

                    b.HasOne("PantryTracker.Model.Products.ProductVariety", "Variety")
                        .WithMany()
                        .HasForeignKey("VarietyId");
                });

            modelBuilder.Entity("PantryTracker.Model.Products.ProductVariety", b =>
                {
                    b.HasOne("PantryTracker.Model.Products.Product", "Product")
                        .WithMany("Varieties")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PantryTracker.Model.Products.UserProductPreference", b =>
                {
                    b.HasOne("PantryTracker.Model.Products.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");

                    b.HasOne("PantryTracker.Model.Recipes.Recipe", "Recipe")
                        .WithMany()
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PantryTracker.Model.Recipes.Direction", b =>
                {
                    b.HasOne("PantryTracker.Model.Recipes.Recipe")
                        .WithMany("Directions")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PantryTracker.Model.Recipes.Ingredient", b =>
                {
                    b.HasOne("PantryTracker.Model.Recipes.Recipe")
                        .WithMany("Ingredients")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
