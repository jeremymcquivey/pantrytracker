using Microsoft.EntityFrameworkCore;
using PantryTracker.Model.Recipes;

namespace RecipeAPI.Models
{
    public class RecipeContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<Direction> Directions { get; set; }

        public DbSet<Product> Products { get; set; }

        public RecipeContext(DbContextOptions<RecipeContext> options):
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasKey(product => product.Id);

            modelBuilder.Entity<Ingredient>()
                .HasKey(ingredient => new { ingredient.RecipeId, ingredient.Index });

            modelBuilder.Entity<Direction>()
                .HasKey(direction => new { direction.RecipeId, direction.Index });

            modelBuilder.Entity<Recipe>()
                .HasMany(recipe => recipe.Directions);

            modelBuilder.Entity<Recipe>()
                .HasMany(recipe => recipe.Ingredients);

            modelBuilder.Entity<Ingredient>()
                .HasOne(ingredient => ingredient.Product);
        }
    }
}
