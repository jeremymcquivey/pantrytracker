using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PantryTracker.RecipeReader;

namespace Pantrytracker.RecipeReader.Tests
{
    [TestClass]
    public class RecipeIntegrationTest
    {
        [TestMethod]
        public void JalapenoCheddarPorkChops()
        {
            var filename = Path.Combine(Environment.CurrentDirectory, "SampleFiles", "JalapenoCheddarPorkChops.txt");
            if (!File.Exists(filename))
            {
                Assert.Fail($"File {filename} does not exist.");
            }

            var parser = new MetadataParser();
            var lines = File.ReadAllLines(filename);

            var recipe = parser.ExtractRecipe(lines);

            Assert.AreEqual(9, recipe.Ingredients.Count());

            Assert.AreEqual("egg", recipe.Ingredients.First().Name);
            Assert.AreEqual("1", recipe.Ingredients.First().Quantity);

            Assert.AreEqual("2", recipe.Ingredients.Single(x => x.Name.Contains("flour")).Quantity);
            Assert.AreEqual("tablespoons", recipe.Ingredients.Single(x => x.Name.Contains("flour")).Unit);
            
            Assert.AreEqual("1", recipe.Ingredients.Single(x => x.Name.Contains("pepper")).Quantity);
            Assert.AreEqual("pinch", recipe.Ingredients.Single(x => x.Name.Contains("pepper")).Unit);

            Assert.AreEqual("1/4", recipe.Ingredients.Single(x => x.Name.Contains("Cheddar cheese")).Quantity);
            Assert.AreEqual("cup", recipe.Ingredients.Single(x => x.Name.Contains("Cheddar cheese")).Unit);

            Assert.AreEqual("1/4", recipe.Ingredients.Single(x => x.Name.Contains("potato chips")).Quantity);
            Assert.AreEqual("cup", recipe.Ingredients.Single(x => x.Name.Contains("potato chips")).Unit);

            Assert.AreEqual("1 1/2", recipe.Ingredients.Single(x => x.Name.Contains("lime zest")).Quantity);
            Assert.AreEqual("teaspoons", recipe.Ingredients.Single(x => x.Name.Contains("lime zest")).Unit);

            Assert.AreEqual("2", recipe.Ingredients.Single(x => x.Name.Contains("pork chops")).Quantity);
            Assert.AreEqual(null, recipe.Ingredients.Single(x => x.Name.Contains("pork chops")).Unit);

            Assert.AreEqual("2", recipe.Ingredients.Single(x => x.Name.Contains("olive oil")).Quantity);
            Assert.AreEqual("teaspoons", recipe.Ingredients.Single(x => x.Name.Contains("olive oil")).Unit);

            Assert.AreEqual("1", recipe.Ingredients.Single(x => x.Name.Contains("margarine")).Quantity);
            Assert.AreEqual("teaspoon", recipe.Ingredients.Single(x => x.Name.Contains("margarine")).Unit);
        }

        [TestMethod]
        public void WholeWheatBread()
        {
            var filename = Path.Combine(Environment.CurrentDirectory, "SampleFiles", "WholeWheatBread.txt");

            if(!File.Exists(filename))
            {
                Assert.Fail($"File {filename} does not exist.");
            }

            var parser = new MetadataParser();
            var lines = File.ReadAllLines(filename);
            var recipe = parser.ExtractRecipe(lines);

            Assert.AreEqual(6, recipe.Ingredients.Count());

            Assert.AreEqual("6", recipe.Ingredients.Single(x => x.Name.Contains("water")).Quantity);
            Assert.AreEqual("cups", recipe.Ingredients.Single(x => x.Name.Contains("water")).Unit);

            Assert.AreEqual("2", recipe.Ingredients.Single(x => x.Name.Contains("salt")).Quantity);
            Assert.AreEqual("Tbsp.", recipe.Ingredients.Single(x => x.Name.Contains("salt")).Unit);

            Assert.AreEqual("2/3", recipe.Ingredients.Single(x => x.Name.Contains("oil")).Quantity);
            Assert.AreEqual("cup", recipe.Ingredients.Single(x => x.Name.Contains("oil")).Unit);

            Assert.AreEqual("2/3", recipe.Ingredients.Single(x => x.Name.Contains("honey")).Quantity);
            Assert.AreEqual("cup", recipe.Ingredients.Single(x => x.Name.Contains("honey")).Unit);

            Assert.AreEqual("2", recipe.Ingredients.Single(x => x.Name.Contains("yeast")).Quantity);
            Assert.AreEqual("Tbsp.", recipe.Ingredients.Single(x => x.Name.Contains("yeast")).Unit);

            Assert.AreEqual("15", recipe.Ingredients.Single(x => x.Name.Contains("flour")).Quantity);
            Assert.AreEqual("cup", recipe.Ingredients.Single(x => x.Name.Contains("flour")).Unit);
        }

        [TestMethod]
        public void CinnamonRolls()
        {
            var filename = Path.Combine(Environment.CurrentDirectory, "SampleFiles", "CinnamonRolls.txt");

            if (!File.Exists(filename))
            {
                Assert.Fail($"File {filename} does not exist.");
            }

            var parser = new MetadataParser();
            var lines = File.ReadAllLines(filename);
            var recipe = parser.ExtractRecipe(lines);

            Assert.AreEqual(15, recipe.Ingredients.Count());

            Assert.AreEqual("3", recipe.Ingredients.Single(x => x.Name.Contains("yeast")).Quantity);
            Assert.AreEqual("Tbsp.", recipe.Ingredients.Single(x => x.Name.Contains("yeast")).Unit);

            Assert.AreEqual("2", recipe.Ingredients.First(x => x.Name.Contains("milk")).Quantity);
            Assert.AreEqual("cups", recipe.Ingredients.First(x => x.Name.Contains("milk")).Unit);

            Assert.AreEqual("2", recipe.Ingredients.Single(x => x.Name.Contains("water")).Quantity);
            Assert.AreEqual("cups", recipe.Ingredients.Single(x => x.Name.Contains("water")).Unit);

            Assert.AreEqual("5", recipe.Ingredients.Single(x => x.Name.Contains("eggs")).Quantity);
            Assert.AreEqual(null, recipe.Ingredients.Single(x => x.Name.Contains("eggs")).Unit);

            Assert.AreEqual("2/3", recipe.Ingredients.Single(x => x.Name.Contains("shortening")).Quantity);
            Assert.AreEqual("cup", recipe.Ingredients.Single(x => x.Name.Contains("shortening")).Unit);

            Assert.AreEqual("1", recipe.Ingredients.First(x => x.Name.Contains("sugar")).Quantity);
            Assert.AreEqual("cup", recipe.Ingredients.First(x => x.Name.Contains("sugar")).Unit);

            Assert.AreEqual("2", recipe.Ingredients.Single(x => x.Name.Contains("salt")).Quantity);
            Assert.AreEqual("tsp.", recipe.Ingredients.Single(x => x.Name.Contains("salt")).Unit);

            Assert.AreEqual("12", recipe.Ingredients.Single(x => x.Name.Contains("flour")).Quantity);
            Assert.AreEqual("cups", recipe.Ingredients.Single(x => x.Name.Contains("flour")).Unit);

            Assert.AreEqual(string.Empty, recipe.Ingredients.First(x => x.Name.Contains("butter")).Quantity);
            Assert.AreEqual(null, recipe.Ingredients.First(x => x.Name.Contains("butter")).Unit);

            Assert.AreEqual(string.Empty, recipe.Ingredients.Single(x => x.Name.Contains("cinnamon")).Quantity);
            Assert.AreEqual(null, recipe.Ingredients.Single(x => x.Name.Contains("cinnamon")).Unit);

            Assert.AreEqual(string.Empty, recipe.Ingredients.Single(x => x.Name.Contains("brown sugar")).Quantity);
            Assert.AreEqual(null, recipe.Ingredients.Single(x => x.Name.Contains("brown sugar")).Unit);

            Assert.AreEqual(string.Empty, recipe.Ingredients.Single(x => x.Name.Contains("cream cheese")).Quantity);
            Assert.AreEqual(null, recipe.Ingredients.Single(x => x.Name.Contains("cream cheese")).Unit);

            Assert.AreEqual("1", recipe.Ingredients.Last(x => x.Name.Contains("butter")).Quantity);
            Assert.AreEqual("cube", recipe.Ingredients.Last(x => x.Name.Contains("butter")).Unit);

            Assert.AreEqual("4", recipe.Ingredients.Single(x => x.Name.Contains("powdered sugar")).Quantity);
            Assert.AreEqual("cups", recipe.Ingredients.Single(x => x.Name.Contains("powdered sugar")).Unit);

            Assert.AreEqual(string.Empty, recipe.Ingredients.Last(x => x.Name.Contains("milk")).Quantity);
            Assert.AreEqual(null, recipe.Ingredients.Last(x => x.Name.Contains("milk")).Unit);
        }

        [TestMethod]
        public void PizzaCrust()
        {
            var filename = Path.Combine(Environment.CurrentDirectory, "SampleFiles", "PizzaCrust.txt");

            if (!File.Exists(filename))
            {
                Assert.Fail($"File {filename} does not exist.");
            }

            var parser = new MetadataParser();
            var lines = File.ReadAllLines(filename);
            var recipe = parser.ExtractRecipe(lines);

            Assert.AreEqual(7, recipe.Ingredients.Count());

            Assert.AreEqual("1 1/3", recipe.Ingredients.Single(x => x.Name.Contains("water")).Quantity);
            Assert.AreEqual("cups", recipe.Ingredients.Single(x => x.Name.Contains("water")).Unit);

            Assert.AreEqual("1/4", recipe.Ingredients.Single(x => x.Name.Contains("powdered milk")).Quantity);
            Assert.AreEqual("cup", recipe.Ingredients.Single(x => x.Name.Contains("powdered milk")).Unit);

            Assert.AreEqual("1/2", recipe.Ingredients.Single(x => x.Name.Contains("Salt")).Quantity);
            Assert.AreEqual("tsp.", recipe.Ingredients.Single(x => x.Name.Contains("Salt")).Unit);

            Assert.AreEqual("4", recipe.Ingredients.Single(x => x.Name.Contains("Flour")).Quantity);
            Assert.AreEqual("cup", recipe.Ingredients.Single(x => x.Name.Contains("Flour")).Unit);

            Assert.AreEqual("1", recipe.Ingredients.Single(x => x.Name.Contains("SuGAr")).Quantity);
            Assert.AreEqual("Tbsp.", recipe.Ingredients.Single(x => x.Name.Contains("SuGAr")).Unit);

            Assert.AreEqual("1", recipe.Ingredients.Single(x => x.Name.Contains("yeast")).Quantity);
            Assert.AreEqual("Tbsp.", recipe.Ingredients.Single(x => x.Name.Contains("yeast")).Unit);

            Assert.AreEqual("2", recipe.Ingredients.Single(x => x.Name.Contains("vegetable oil")).Quantity);
            Assert.AreEqual("Tbsp.", recipe.Ingredients.Single(x => x.Name.Contains("vegetable oil")).Unit);
        }

        [TestMethod]
        public void Tortillas()
        {
            var filename = Path.Combine(Environment.CurrentDirectory, "SampleFiles", "Tortillas.txt");

            if (!File.Exists(filename))
            {
                Assert.Fail($"File {filename} does not exist.");
            }

            var parser = new MetadataParser();
            var lines = File.ReadAllLines(filename);
            var recipe = parser.ExtractRecipe(lines);

            Assert.AreEqual(4, recipe.Ingredients.Count());

            Assert.AreEqual("4", recipe.Ingredients.Single(x => x.Name.Contains("flour")).Quantity);
            Assert.AreEqual("cups", recipe.Ingredients.Single(x => x.Name.Contains("flour")).Unit);

            Assert.AreEqual("1/4", recipe.Ingredients.Single(x => x.Name.Contains("salt")).Quantity);
            Assert.AreEqual("tsp.", recipe.Ingredients.Single(x => x.Name.Contains("salt")).Unit);

            Assert.AreEqual("1/4", recipe.Ingredients.Single(x => x.Name.Contains("shortening")).Quantity);
            Assert.AreEqual("cup", recipe.Ingredients.Single(x => x.Name.Contains("shortening")).Unit);

            Assert.AreEqual("1 1/2", recipe.Ingredients.Single(x => x.Name.Contains("water")).Quantity);
            Assert.AreEqual("cups", recipe.Ingredients.Single(x => x.Name.Contains("water")).Unit);
        }

        [TestMethod]
        public void PotatoBaconCheddarSoup()
        {
            var filename = Path.Combine(Environment.CurrentDirectory, "SampleFiles", "PotatoBaconCheddarSoup.txt");

            if (!File.Exists(filename))
            {
                Assert.Fail($"File {filename} does not exist.");
            }

            var parser = new MetadataParser();
            var lines = File.ReadAllLines(filename);
            var recipe = parser.ExtractRecipe(lines);

            Assert.AreEqual(5, recipe.Ingredients.Count());

            Assert.AreEqual("5-8", recipe.Ingredients.Single(x => x.Name.Contains("potatoes")).Quantity);
            Assert.AreEqual(null, recipe.Ingredients.Single(x => x.Name.Contains("potatoes")).Unit);

            Assert.AreEqual("3", recipe.Ingredients.Single(x => x.Name.Contains("celery")).Quantity);
            Assert.AreEqual(null, recipe.Ingredients.Single(x => x.Name.Contains("celery")).Unit);

            Assert.AreEqual("1", recipe.Ingredients.Single(x => x.Name.Contains("bacon")).Quantity);
            Assert.AreEqual("pound", recipe.Ingredients.Single(x => x.Name.Contains("bacon")).Unit);

            Assert.AreEqual("1 1/2", recipe.Ingredients.Single(x => x.Name.Contains("milk")).Quantity);
            Assert.AreEqual("cups", recipe.Ingredients.Single(x => x.Name.Contains("milk")).Unit);

            Assert.AreEqual("2", recipe.Ingredients.Single(x => x.Name.Contains("cheddar cheese")).Quantity);
            Assert.AreEqual("cups", recipe.Ingredients.Single(x => x.Name.Contains("cheddar cheese")).Unit);
        }

        [TestMethod]
        public void RootBeer()
        {
            var filename = Path.Combine(Environment.CurrentDirectory, "SampleFiles", "RootBeer.txt");

            if (!File.Exists(filename))
            {
                Assert.Fail($"File {filename} does not exist.");
            }

            var parser = new MetadataParser();
            var lines = File.ReadAllLines(filename);
            var recipe = parser.ExtractRecipe(lines);

            Assert.AreEqual(4, recipe.Ingredients.Count());

            Assert.AreEqual("1", recipe.Ingredients.Single(x => x.Name.Contains("root beer extract")).Quantity);
            Assert.AreEqual(null, recipe.Ingredients.Single(x => x.Name.Contains("root beer extract")).Unit);

            Assert.AreEqual("2", recipe.Ingredients.Single(x => x.Name.Contains("sugar")).Quantity);
            Assert.AreEqual("2 1/2", recipe.Ingredients.Single(x => x.Name.Contains("sugar")).Size);
            Assert.AreEqual("lb.", recipe.Ingredients.Single(x => x.Name.Contains("sugar")).Unit);

            Assert.AreEqual("5", recipe.Ingredients.Single(x => x.Name.Contains("water")).Quantity);
            Assert.AreEqual("gallons", recipe.Ingredients.Single(x => x.Name.Contains("water")).Unit);

            Assert.AreEqual("5", recipe.Ingredients.Single(x => x.Name.Contains("dry ice")).Quantity);
            Assert.AreEqual("lb.", recipe.Ingredients.Single(x => x.Name.Contains("dry ice")).Unit);
        }

        [TestMethod]
        public void StrawberryLemonadeCupcakes()
        {
            var filename = Path.Combine(Environment.CurrentDirectory, "SampleFiles", "StrawberryLemonadeCupcakes.txt");

            if (!File.Exists(filename))
            {
                Assert.Fail($"File {filename} does not exist.");
            }

            var parser = new MetadataParser();
            var lines = File.ReadAllLines(filename);
            var recipe = parser.ExtractRecipe(lines);

            Assert.AreEqual(9, recipe.Ingredients.Count());

            Assert.AreEqual("3", recipe.Ingredients.Single(x => x.Name.Contains("flour")).Quantity);
            Assert.AreEqual("cups", recipe.Ingredients.Single(x => x.Name.Contains("flour")).Unit);

            Assert.AreEqual("1", recipe.Ingredients.Single(x => x.Name.Contains("baking powder")).Quantity);
            Assert.AreEqual("Tbsp.", recipe.Ingredients.Single(x => x.Name.Contains("baking powder")).Unit);

            Assert.AreEqual("1/2", recipe.Ingredients.First(x => x.Name.Contains("salt")).Quantity);
            Assert.AreEqual("tsp.", recipe.Ingredients.First(x => x.Name.Contains("salt")).Unit);

            Assert.AreEqual("1", recipe.Ingredients.First(x => x.Name.Contains("butter")).Quantity);
            Assert.AreEqual("cup", recipe.Ingredients.First(x => x.Name.Contains("butter")).Unit);

            Assert.AreEqual("2", recipe.Ingredients.Single(x => x.Name.Contains("sugar")).Quantity);
            Assert.AreEqual("cups", recipe.Ingredients.Single(x => x.Name.Contains("sugar")).Unit);

            Assert.AreEqual("4", recipe.Ingredients.Single(x => x.Name.Contains("eggs")).Quantity);
            Assert.AreEqual(null, recipe.Ingredients.Single(x => x.Name.Contains("eggs")).Unit);

            Assert.AreEqual("3", recipe.Ingredients.Single(x => x.Name.Contains("zest")).Quantity);
            Assert.AreEqual("Tbsp.", recipe.Ingredients.Single(x => x.Name.Contains("zest")).Unit);

            Assert.AreEqual("1/2", recipe.Ingredients.Single(x => x.Name.Contains("lemonade")).Quantity);
            Assert.AreEqual("cup", recipe.Ingredients.Single(x => x.Name.Contains("lemonade")).Unit);

            Assert.AreEqual("1", recipe.Ingredients.Single(x => x.Name.Contains("buttermilk")).Quantity);
            Assert.AreEqual("cup", recipe.Ingredients.Single(x => x.Name.Contains("buttermilk")).Unit);
        }
    }
}