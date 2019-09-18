﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RecipeAPI.Data;

namespace RecipeAPI.Migrations
{
    [DbContext(typeof(RecipeContext))]
    [Migration("20190816024701_InitialCreateRecipeWithIngredients")]
    partial class InitialCreateRecipeWithIngredients
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PantryTracker.Model.Recipe.Ingredient", b =>
                {
                    b.Property<int>("Index");

                    b.Property<Guid>("RecipeId");

                    b.Property<string>("Container");

                    b.Property<string>("Descriptor");

                    b.Property<string>("Name");

                    b.Property<string>("Quantity");

                    b.Property<string>("SubQuantity");

                    b.Property<string>("Unit");

                    b.HasKey("Index", "RecipeId")
                        .HasName("PK_Ingredient");

                    b.HasIndex("RecipeId");

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

                    b.HasKey("Id")
                        .HasName("PK_Recipe");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("PantryTracker.Model.Recipe.Ingredient", b =>
                {
                    b.HasOne("PantryTracker.Model.Recipe.Recipe", "Recipe")
                        .WithMany("Ingredients")
                        .HasForeignKey("RecipeId")
                        .HasConstraintName("FK_Ingredient_Recipe")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}