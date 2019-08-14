using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sample.ParseTree.Tests
{
    [TestClass]
    public class TimeTests
    {

        [TestMethod]
        public void Method1()
        {
            var input = new string[]{ "Jalapeno Cheddar Pork Chops",
                                      "Prep Time: 50 minutes",
                                      "Servings: 2",
                                      "https://www.allrecipes.com/recipe/273630/jalapeno-cheddar-pork-chops/?internalSource=rotd&referringContentType=Homepage&clickId=cardslot%201" };

            var recipe = new MetadataParser().ExtractRecipe(input);

            Assert.AreEqual("50 minutes", recipe.PrepTime);
        }

        [TestMethod]
        public void Method2()
        {
            var input = new string[]{ "Jalapeno Cheddar Pork Chops",
                                      "Prep Time: 50 minutes",
                                      "Servings: 2",
                                      "https://www.allrecipes.com/recipe/273630/jalapeno-cheddar-pork-chops/?internalSource=rotd&referringContentType=Homepage&clickId=cardslot%201" };

            var recipe = new MetadataParser().ExtractRecipe(input);

            Assert.AreEqual("Jalapeno Cheddar Pork Chops", recipe.Name);
        }
    }
}
