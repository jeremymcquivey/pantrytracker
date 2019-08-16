using Microsoft.EntityFrameworkCore;
using PantryTracker.Model.Recipe;

namespace RecipeAPI.Data
{
    /// <summary>
    /// Database Access Layer for the Recipes.
    /// </summary>
    public class RecipeContext : DbContext
    {
        public RecipeContext(DbContextOptions<RecipeContext> options) :
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ingredient>()
                    .HasKey(nameof(Ingredient.Index), nameof(Ingredient.RecipeId))
                    .HasName("PK_Ingredient");

            modelBuilder.Entity<Ingredient>()
                    .HasOne(ingr => ingr.Recipe)
                    .WithMany(rec => rec.Ingredients)
                    .HasForeignKey(ingr => ingr.RecipeId)
                    .HasConstraintName("FK_Ingredient_Recipe");

            modelBuilder.Entity<Recipe>()
                    .Ignore(r => r.Tags)
                    .Ignore(r => r.Reviews)
                    .HasKey(nameof(Recipe.Id))
                    .HasName("PK_Recipe");
        }

        /// <summary>
        /// Ingredients that are part of recipes.
        /// </summary>
        public DbSet<Ingredient> Ingredients { get; set; }
        
        /// <summary>
        /// Recipes are the principal object in the database.
        /// </summary>
        public DbSet<Recipe> Recipes { get; set; }

        //public DbSet<object> RecipeDirections { get; set; }
    }
}