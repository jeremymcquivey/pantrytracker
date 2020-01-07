using System.Collections.Generic;
using System.Linq;
using PantryTracker.Model.Recipe;

namespace PantryTracker.RecipeReader.Rules
{
    internal static partial class StringRules
    {
        public static RecipeIngredient ToIngredient(this IEnumerable<Word> wordList)
        {
            return new RecipeIngredient()
            {
                Name = string.Join(" ", wordList.Where(w => w.PartOfSpeech == PartOfSpeech.Name)?.Select(w => w.Contents)),
                Quantity = wordList.FirstOrDefault(w => w.PartOfSpeech == PartOfSpeech.Quantity)?.Contents ?? string.Empty,
                SubQuantity = wordList.FirstOrDefault(w => w.PartOfSpeech == PartOfSpeech.SubQuantity)?.Contents,
                Unit = wordList.FirstOrDefault(w => w.PartOfSpeech == PartOfSpeech.Unit)?.Contents
            };
        }
    }
}