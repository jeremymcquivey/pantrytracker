using PantryTracker.Model.Meta;
using PantryTracker.Model.Recipes;
using PantryTracker.RecipeReader.Rules;

namespace PantryTracker.RecipeReader
{
    public class IngredientParser
    {
        public Ingredient ProcessSentence(string phrase, UnitAliases units)
        {
            if (string.IsNullOrEmpty(phrase))
                return null;

            var words = phrase.GetWords()
                              .DetectQuantities()
                              .CombineFractions()
                              .FindUnits(units)
                              .DetectContainerSize()
                              .DetectContainer()
                              .DetectRanges()
                              .DefineNames()
                              .RemoveUnwantedWords()
                              .Sanitize(units);

            return (words.ToIngredient() ?? new Ingredient())
                         .AdjustContainerSizes();
        }
    }
}