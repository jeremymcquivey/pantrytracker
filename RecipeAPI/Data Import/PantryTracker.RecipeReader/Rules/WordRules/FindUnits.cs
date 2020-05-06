using PantryTracker.Model.Meta;
using System.Collections.Generic;
using System.Linq;

namespace PantryTracker.RecipeReader.Rules
{
    internal static partial class EnumerableRules
    {
        public static IEnumerable<Word> FindUnits(this IEnumerable<Word> wordList, UnitAliases units)
        {
            foreach (var word in wordList.Where(w => w.PartOfSpeech == null))
            {
                if (!string.IsNullOrEmpty(units.GetSanitizedUnit(word.Contents)))
                {
                    word.PartOfSpeech = PartOfSpeech.Unit;
                }
            }

            return wordList;
        }
    }
}