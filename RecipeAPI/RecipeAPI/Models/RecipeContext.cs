using Microsoft.EntityFrameworkCore;
using PantryTracker.Model.Recipe;

namespace RecipeAPI.Models
{
    public class RecipeContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }

        public RecipeContext(DbContextOptions<RecipeContext> options):
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ingredient>()
                .HasKey(ingredient => new { ingredient.RecipeId, ingredient.Index });
                
        }
    }
}
