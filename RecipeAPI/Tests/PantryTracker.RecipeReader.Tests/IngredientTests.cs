using Microsoft.VisualStudio.TestTools.UnitTesting;
using PantryTracker.RecipeReader;

namespace Pantrytracker.RecipeReader.Tests
{
    [TestClass]
    public class Ingredients
    {
        private IngredientParser _tree;

        [TestInitialize]
        public void Setup()
        {
            _tree = new IngredientParser();
        }

        [TestMethod]
        public void ProcessQuantityWithSingleWordIngredient()
        {
            var ingredient = _tree.ProcessSentence("1 egg");

            Assert.AreEqual("1", ingredient.Quantity);
            Assert.AreEqual("egg", ingredient.Name);
        }

        [TestMethod]
        public void ProcessQuantityWithMultiWordIngredient()
        {
            var ingredient = _tree.ProcessSentence("2 tablespoons flour");

            Assert.AreEqual(string.Empty, ingredient.SubQuantity);
            Assert.AreEqual("2", ingredient.Quantity);
            Assert.AreEqual("flour", ingredient.Name);
            Assert.AreEqual("tablespoons", ingredient.Unit);
        }

        [TestMethod]
        public void ProcessQuantityWithUnitAndSubQuantity()
        {
            var ingredient = _tree.ProcessSentence("2 heaping tablespoons flour");

            Assert.AreEqual("2", ingredient.Quantity);
            Assert.AreEqual("heaping", ingredient.SubQuantity);
            Assert.AreEqual("flour", ingredient.Name);
            Assert.AreEqual("tablespoons", ingredient.Unit);
        }

        [TestMethod]
        public void ProcessQuantityWithUnit()
        {
            var ingredient = _tree.ProcessSentence("2 tablespoons all-purpose flour");

            Assert.AreEqual(string.Empty, ingredient.SubQuantity);
            Assert.AreEqual("2", ingredient.Quantity);
            //Removes hyphens from words. i.e. all-purpose. Eventually this should be smart enough to look beyond this.
            Assert.AreEqual("all purpose flour", ingredient.Name);
            Assert.AreEqual("tablespoons", ingredient.Unit);
        }

        [TestMethod]
        public void ProcessLessThanOneFractionalQuantityWithUnit()
        {
            var ingredient = _tree.ProcessSentence("1/2 tablespoons flour");

            Assert.AreEqual(string.Empty, ingredient.SubQuantity);
            Assert.AreEqual("1/2", ingredient.Quantity);
            Assert.AreEqual("flour", ingredient.Name);
            Assert.AreEqual("tablespoons", ingredient.Unit);
        }

        [TestMethod]
        public void ProcessGreaterThanOneFractionalQuantityWithUnit()
        {
            var ingredient = _tree.ProcessSentence("2 1/2 tablespoons flour");

            Assert.AreEqual(string.Empty, ingredient.SubQuantity);
            Assert.AreEqual("2 1/2", ingredient.Quantity);
            Assert.AreEqual("flour", ingredient.Name);
            Assert.AreEqual("tablespoons", ingredient.Unit);
        }

        [TestMethod]
        public void ProcessNonStandardUnitOfMeasure()
        {
            var ingredient = _tree.ProcessSentence("1 pinch ground black pepper");

            Assert.AreEqual(string.Empty, ingredient.SubQuantity);
            Assert.AreEqual("1", ingredient.Quantity);
            Assert.AreEqual("ground black pepper", ingredient.Name);
            Assert.AreEqual("pinch", ingredient.Unit);
        }

        [TestMethod]
        public void RemoveWordsBeforeInitialQuantity()
        {
            var ingredient = _tree.ProcessSentence("steak and fish 1 pinch ground black pepper");

            Assert.AreEqual(string.Empty, ingredient.SubQuantity);
            Assert.AreEqual("1", ingredient.Quantity);
            Assert.AreEqual("ground black pepper", ingredient.Name);
            Assert.AreEqual("pinch", ingredient.Unit);
        }

        [TestMethod]
        public void ProcessNoSubquantityWithFractionalQuantity()
        {
            var ingredient = _tree.ProcessSentence("1/4 cup shredded Cheddar cheese");

            Assert.AreEqual(string.Empty, ingredient.SubQuantity);
            Assert.AreEqual("1/4", ingredient.Quantity);
            Assert.AreEqual("shredded Cheddar cheese", ingredient.Name);
            Assert.AreEqual("cup", ingredient.Unit);
        }

        [TestMethod]
        public void ValidateSpecialCharactersCanBePartOfName()
        {
            var ingredient = _tree.ProcessSentence("1/4 cup crushed jalapeno potato chips (such as Miss Vickie's®)");

            Assert.AreEqual(string.Empty, ingredient.SubQuantity);
            Assert.AreEqual("1/4", ingredient.Quantity);
            Assert.AreEqual("crushed jalapeno potato chips (such as Miss Vickie's®)", ingredient.Name);
            Assert.AreEqual("cup", ingredient.Unit);
        }

        [TestMethod]
        public void ProcessFractionalQuantityWithUnit()
        {
            var ingredient = _tree.ProcessSentence("1 1/2 teaspoons grated lime zest");

            Assert.AreEqual(string.Empty, ingredient.SubQuantity);
            Assert.AreEqual("1 1/2", ingredient.Quantity);
            Assert.AreEqual("grated lime zest", ingredient.Name);
            Assert.AreEqual("teaspoons", ingredient.Unit);
        }

        [TestMethod]
        public void HyphensAreStrippedFromName()
        {
            var ingredient = _tree.ProcessSentence("2 bone-in pork chops");

            Assert.AreEqual("2", ingredient.Quantity);
            Assert.AreEqual(null, ingredient.SubQuantity);
            //Removes hyphens from words. i.e. bone-in. Eventually this should be smart enough to look beyond this.
            Assert.AreEqual("bone in pork chops", ingredient.Name);
        }

        [TestMethod]
        public void PluralWholeQuantityWithUnit()
        {
            var ingredient = _tree.ProcessSentence("2 teaspoons olive oil");

            Assert.AreEqual(string.Empty, ingredient.SubQuantity);
            Assert.AreEqual("2", ingredient.Quantity);
            Assert.AreEqual("olive oil", ingredient.Name);
            Assert.AreEqual("teaspoons", ingredient.Unit);
        }

        [TestMethod]
        public void SingularWholeQuantityWithUnit()
        {
            var ingredient = _tree.ProcessSentence("1 teaspoon margarine");

            Assert.AreEqual(string.Empty, ingredient.SubQuantity);
            Assert.AreEqual("1", ingredient.Quantity);
            Assert.AreEqual("margarine", ingredient.Name);
            Assert.AreEqual("teaspoon", ingredient.Unit);
        }
    }
}
