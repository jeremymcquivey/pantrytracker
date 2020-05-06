using System.Collections.Generic;
using System.Linq;
using PantryTracker.Model.Recipes;

namespace PantryTracker.RecipeReader.Rules
{
    internal static partial class StringRules
    {
        public static Ingredient ToIngredient(this IEnumerable<Word> wordList)
        {
            return new Ingredient()
            {
                Name = string.Join(" ", wordList.Where(w => w.PartOfSpeech == PartOfSpeech.Name)?.Select(w => w.Contents)),
                Quantity = wordList.FirstOrDefault(w => w.PartOfSpeech == PartOfSpeech.Quantity)?.Contents ?? string.Empty,
                Size = wordList.FirstOrDefault(w => w.PartOfSpeech == PartOfSpeech.ContainerSize)?.Contents,
                Unit = wordList.FirstOrDefault(w => w.PartOfSpeech == PartOfSpeech.Unit)?.Contents
            };
        }
    }
}