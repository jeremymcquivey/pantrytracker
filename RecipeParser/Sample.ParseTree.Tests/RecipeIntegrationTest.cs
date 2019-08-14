using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sample.ParseTree.Tests
{
    [TestClass]
    public class RecipeIntegrationTest
    {
        [TestMethod]
        public void Method1()
        {
            var stuff = @"Jalapeno Cheddar Pork Chops
50 minutes
Source: https://www.allrecipes.com/recipe/273630/jalapeno-cheddar-pork-chops/?internalSource=rotd&referringContentType=Homepage&clickId=cardslot%201

1 egg
2 tablespoons all-purpose flour
1 pinch ground black pepper
1/4 cup shredded Cheddar cheese
1/4 cup crushed jalapeno potato chips
1 1/2 teaspoons grated lime zest
2 bone-in pork chops
2 teaspoons olive oil
1 teaspoon margarine

Preheat the oven to 400 degrees F (200 degrees C).
Whisk egg in a shallow bowl. Combine flour and pepper in another shallow bowl. Mix Cheddar cheese, jalapeno chips, and lime zest in a third bowl.
Toss pork chops in the flour to coat. Shake off excess, then dunk in egg. Coat chops in the chip and cheese mixture.
Heat oil and margarine in a medium skillet over medium-high heat. Fry each pork chop until browned on the outside, 2 minutes per side. Transfer chops to a baking dish or foil-lined baking pan.
Bake in the preheated oven until pork is no longer pink in the center, about 25 minutes. An instant-read thermometer inserted into the center should read 145 degrees F (63 degrees C).";

            var parser = new MetadataParser();

            var lines = stuff.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None
            );

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
    }
}