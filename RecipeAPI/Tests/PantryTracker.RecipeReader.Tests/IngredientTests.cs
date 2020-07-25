using Microsoft.VisualStudio.TestTools.UnitTesting;
using PantryTracker.Model.Meta;
using PantryTracker.RecipeReader;

namespace Pantrytracker.RecipeReader.Tests
{
    [TestClass]
    public class Ingredients
    {
        private IngredientParser _tree;
        private UnitAliases _allUnits;

        [TestInitialize]
        public void Setup()
        {
            _tree = new IngredientParser();
            _allUnits = new UnitAliases();
        }

        [TestMethod]
        public void ProcessQuantityWithSingleWordIngredient()
        {
            var ingredient = _tree.ProcessSentence("1 egg", _allUnits);

            Assert.AreEqual("1", ingredient.Quantity);
            Assert.AreEqual("egg", ingredient.Name);
        }

        [TestMethod]
        public void ProcessQuantityWithMultiWordIngredient()
        {
            var ingredient = _tree.ProcessSentence("2 tablespoons flour", _allUnits);

            Assert.AreEqual(null, ingredient.Size);
            Assert.AreEqual("2", ingredient.Quantity);
            Assert.AreEqual("flour", ingredient.Name);
            Assert.AreEqual("tbsp", ingredient.Unit);
        }

        [TestMethod]
        public void ProcessQuantityWithUnitAndSubQuantity()
        {
            var ingredient = _tree.ProcessSentence("2 heaping tablespoons flour", _allUnits);

            Assert.AreEqual("2", ingredient.Quantity);
            Assert.AreEqual(null, ingredient.Size);
            Assert.AreEqual("heaping flour", ingredient.Name);
            Assert.AreEqual("tbsp", ingredient.Unit);
        }

        [TestMethod]
        public void ProcessQuantityWithUnit()
        {
            var ingredient = _tree.ProcessSentence("2 tablespoons all-purpose flour", _allUnits);

            Assert.AreEqual(null, ingredient.Size);
            Assert.AreEqual("2", ingredient.Quantity);
            //Removes hyphens from words. i.e. all-purpose. Eventually this should be smart enough to look beyond this.
            Assert.AreEqual("all purpose flour", ingredient.Name);
            Assert.AreEqual("tbsp", ingredient.Unit);
        }

        [TestMethod]
        public void ProcessLessThanOneFractionalQuantityWithUnit()
        {
            var ingredient = _tree.ProcessSentence("1/2 tablespoons flour", _allUnits);

            Assert.AreEqual(null, ingredient.Size);
            Assert.AreEqual("1/2", ingredient.Quantity);
            Assert.AreEqual("flour", ingredient.Name);
            Assert.AreEqual("tbsp", ingredient.Unit);
        }

        [TestMethod]
        public void ProcessGreaterThanOneFractionalQuantityWithUnit()
        {
            var ingredient = _tree.ProcessSentence("2 1/2 tablespoons flour", _allUnits);

            Assert.AreEqual(null, ingredient.Size);
            Assert.AreEqual("2 1/2", ingredient.Quantity);
            Assert.AreEqual("flour", ingredient.Name);
            Assert.AreEqual("tbsp", ingredient.Unit);
        }

        [TestMethod]
        public void ProcessNonStandardUnitOfMeasure()
        {
            var ingredient = _tree.ProcessSentence("1 pinch ground black pepper", _allUnits);

            Assert.AreEqual(null, ingredient.Size);
            Assert.AreEqual("1", ingredient.Quantity);
            Assert.AreEqual("ground black pepper", ingredient.Name);
            Assert.AreEqual("pinch", ingredient.Unit);
        }

        [TestMethod]
        public void RemoveWordsBeforeInitialQuantity()
        {
            var ingredient = _tree.ProcessSentence("steak and fish 1 pinch ground black pepper", _allUnits);

            Assert.AreEqual(null, ingredient.Size);
            Assert.AreEqual("1", ingredient.Quantity);
            Assert.AreEqual("ground black pepper", ingredient.Name);
            Assert.AreEqual("pinch", ingredient.Unit);
        }

        [TestMethod]
        public void ProcessNoSubquantityWithFractionalQuantity()
        {
            var ingredient = _tree.ProcessSentence("1/4 cup shredded Cheddar cheese", _allUnits);

            Assert.AreEqual(null, ingredient.Size);
            Assert.AreEqual("1/4", ingredient.Quantity);
            Assert.AreEqual("shredded Cheddar cheese", ingredient.Name);
            Assert.AreEqual("c", ingredient.Unit);
        }

        [TestMethod]
        public void ValidateSpecialCharactersCanBePartOfName()
        {
            var ingredient = _tree.ProcessSentence("1/4 cup crushed jalapeno potato chips (such as Miss Vickie's®)", _allUnits);

            Assert.AreEqual(null, ingredient.Size);
            Assert.AreEqual("1/4", ingredient.Quantity);
            Assert.AreEqual("crushed jalapeno potato chips (such as Miss Vickie's®)", ingredient.Name);
            Assert.AreEqual("c", ingredient.Unit);
        }

        [TestMethod]
        public void ProcessFractionalQuantityWithUnit()
        {
            var ingredient = _tree.ProcessSentence("1 1/2 teaspoons grated lime zest", _allUnits);

            Assert.AreEqual(null, ingredient.Size);
            Assert.AreEqual("1 1/2", ingredient.Quantity);
            Assert.AreEqual("grated lime zest", ingredient.Name);
            Assert.AreEqual("tsp", ingredient.Unit);
        }

        [TestMethod]
        public void HyphensAreStrippedFromName()
        {
            var ingredient = _tree.ProcessSentence("2 bone-in pork chops", _allUnits);

            Assert.AreEqual("2", ingredient.Quantity);
            Assert.AreEqual(null, ingredient.Size);
            //Removes hyphens from words. i.e. bone-in. Eventually this should be smart enough to look beyond this.
            Assert.AreEqual("bone in pork chops", ingredient.Name);
        }

        [TestMethod]
        public void PluralWholeQuantityWithUnit()
        {
            var ingredient = _tree.ProcessSentence("2 teaspoons olive oil", _allUnits);

            Assert.AreEqual(null, ingredient.Size);
            Assert.AreEqual("2", ingredient.Quantity);
            Assert.AreEqual("olive oil", ingredient.Name);
            Assert.AreEqual("tsp", ingredient.Unit);
        }

        [TestMethod]
        public void SingularWholeQuantityWithUnit()
        {
            var ingredient = _tree.ProcessSentence("1 teaspoon margarine", _allUnits);

            Assert.AreEqual(null, ingredient.Size);
            Assert.AreEqual("1", ingredient.Quantity);
            Assert.AreEqual("margarine", ingredient.Name);
            Assert.AreEqual("tsp", ingredient.Unit);
        }

        [TestMethod]
        public void QtyContainerSizeUnitStructure()
        {
            var ingredient = _tree.ProcessSentence("2 14.4 oz. Jars peach halves", _allUnits);

            Assert.AreEqual("2", ingredient.Quantity);
            Assert.AreEqual("14.4", ingredient.Size);
            Assert.AreEqual("oz", ingredient.Unit);
        }

        [TestMethod]
        public void QtyContainerSizeUnitAltStructure()
        {
            var ingredient = _tree.ProcessSentence("2 jars (14.4 oz. each) Jars peach halves", _allUnits);

            Assert.AreEqual("2", ingredient.Quantity);
            Assert.AreEqual("14.4", ingredient.Size);
            Assert.AreEqual("oz", ingredient.Unit);
            Assert.AreEqual("jar", ingredient.Container);
        }

        [TestMethod]
        public void DozenIsUnit()
        {
            var ingredient = _tree.ProcessSentence("2 dozen eggs", _allUnits);

            Assert.AreEqual("2", ingredient.Quantity);
            Assert.AreEqual(null, ingredient.Size);
            Assert.AreEqual("doz", ingredient.Unit);
        }
    }
}