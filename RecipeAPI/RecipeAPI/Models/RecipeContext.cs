using Microsoft.EntityFrameworkCore;
using PantryTracker.Model.Recipe;

namespace RecipeAPI.Models
{
    public class RecipeContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }

        public DbSet<Direction> Directions { get; set; }

        public RecipeContext(DbContextOptions<RecipeContext> options):
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RecipeIngredient>()
                .HasKey(ingredient => new { ingredient.RecipeId, ingredient.Index });

            modelBuilder.Entity<Direction>()
                .HasKey(direction => new { direction.RecipeId, direction.Index });
        }
    }
}
