﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sample.ParseTree.Tests
{
    [TestClass]
    public class InstructionTest
    {
        [TestMethod]
        public void Method1()
        {
            var input = new string[]
            {
                "Jalapeno Cheddar Pork Chops",
                "50 minutes",
                "Source: https://www.allrecipes.com/recipe/273630/jalapeno-cheddar-pork-chops/?internalSource=rotd&referringContentType=Homepage&clickId=cardslot%201",
                "",
                "1 egg",
                "2 tablespoons all-purpose flour",
                "1 pinch ground black pepper",
                "1 / 4 cup shredded Cheddar cheese",
                "1 / 4 cup crushed jalapeno potato chips(such as Miss Vickie's®)",
                "1 1 / 2 teaspoons grated lime zest",
                "2 bone -in pork chops",
                "2 teaspoons olive oil",
                "1 teaspoon margarine",
                "",
                "Some directions here.",
                "Second directions",
                "Step 3"
            };

            var recipe = new MetadataParser().ExtractRecipe(input);
            Assert.AreEqual(3, recipe.Directions.Count());
            Assert.AreEqual(input.Last(), recipe.Directions.Last());
        }
    }
}