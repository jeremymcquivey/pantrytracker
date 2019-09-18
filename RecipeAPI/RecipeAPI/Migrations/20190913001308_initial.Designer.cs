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
    [Migration("20190913001308_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PantryTracker.Model.Recipe.Ingredient", b =>
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

            modelBuilder.Entity("PantryTracker.Model.Recipe.Recipe", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body");

                    b.Property<string>("Credit");

                    b.Property<string>("OwnerId");

                    b.Property<string>("PrepTime");

                    b.Property<string>("PublicState");

                    b.Property<string>("RawText");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("PantryTracker.Model.Recipe.Ingredient", b =>
                {
                    b.HasOne("PantryTracker.Model.Recipe.Recipe", "Recipe")
                        .WithMany("Ingredients")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}