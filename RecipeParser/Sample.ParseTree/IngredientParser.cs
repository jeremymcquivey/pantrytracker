using Sample.ParseTree.Model;
using Sample.ParseTree.Rules;

namespace Sample.Grammar
{
    public class IngredientParser
    {
        private UnitsOfMeasure _unitsOfMeasure =
            new UnitsOfMeasure();

        public Ingredient ProcessSentence(string phrase)
        {
            if (string.IsNullOrEmpty(phrase))
                return null;

            var words = phrase.GetWords()
                              .DetectQuantities()
                              .CombineFractions()
                              .FindUnits(_unitsOfMeasure)
                              .DetectSubquantities()
                              .DefineNames()
                              .RemoveUnwantedWords();

            return (words.ToIngredient() ?? new Ingredient())
                         .AdjustSubQuantities();
        }
    }
}