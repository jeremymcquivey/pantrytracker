using PantryTracker.Model.Recipe;
using PantryTracker.RecipeReader.Rules;

namespace PantryTracker.RecipeReader
{
    public class IngredientParser
    {
        private UnitsOfMeasure _unitsOfMeasure =
            new UnitsOfMeasure();

        public RecipeIngredient ProcessSentence(string phrase)
        {
            if (string.IsNullOrEmpty(phrase))
                return null;

            var words = phrase.GetWords()
                              .DetectQuantities()
                              .CombineFractions()
                              .FindUnits(_unitsOfMeasure)
                              .DetectSubquantities()
                              .DetectRanges()
                              .DefineNames()
                              .RemoveUnwantedWords();

            return (words.ToIngredient() ?? new RecipeIngredient())
                         .AdjustSubQuantities();
        }
    }
}