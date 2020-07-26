using Microsoft.EntityFrameworkCore;
using PantryTracker.Model.Grocery;
using PantryTracker.Model.Menu;
using PantryTracker.Model.Pantry;
using PantryTracker.Model.Products;
using PantryTracker.Model.Recipes;

namespace RecipeAPI.Models
{
    public class RecipeContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<Direction> Directions { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductVariety> Varieties { get; set; }

        public DbSet<UserProductPreference> UserProductPreferences { get; set; }

        public DbSet<PantryTransaction> Transactions { get; set; }

        public DbSet<ProductCode> ProductCodes { get; set; }

        public DbSet<ListItem> GroceryListItems { get; set; }

        public DbSet<CalendarMenuEntry> MenuEntries { get; set; }

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
                .HasIndex(nameof(Recipe.OwnerId));

            modelBuilder.Entity<Recipe>()
                .HasMany(recipe => recipe.Ingredients);

            modelBuilder.Entity<UserProductPreference>()
                .HasOne(preference => preference.Product);

            modelBuilder.Entity<UserProductPreference>()
                .HasOne(preference => preference.Recipe);

            modelBuilder.Entity<PantryTransaction>()
                .HasKey(trans => trans.Id);

            modelBuilder.Entity<PantryTransaction>()
                .HasOne(trans => trans.Product);

            modelBuilder.Entity<PantryTransaction>()
                .HasIndex(nameof(PantryTransaction.UserId));

            modelBuilder.Entity<ProductCode>()
                .HasKey(code => code.Id);

            modelBuilder.Entity<ProductCode>()
                .HasIndex(code => new { code.Code, code.OwnerId })
                .HasName("UniqueProductCodeByUser");

            modelBuilder.Entity<ProductCode>()
                .HasOne(code => code.Product)
                .WithMany(product => product.Codes);

            modelBuilder.Entity<ProductVariety>()
                .HasOne(variety => variety.Product)
                .WithMany(product => product.Varieties);

            modelBuilder.Entity<CalendarMenuEntry>()
                .Ignore(p => p.RecipeName);

            modelBuilder.Entity<CalendarMenuEntry>()
                .HasKey(entry => entry.Id);

            modelBuilder.Entity<CalendarMenuEntry>()
                .HasOne(entry => entry.Recipe);

            modelBuilder.Entity<CalendarMenuEntry>()
                .HasIndex(entry => new { entry.OwnerId, entry.RecipeId, entry.Date })
                .HasName("UniqueRecipePerDay");
        }
    }
}
