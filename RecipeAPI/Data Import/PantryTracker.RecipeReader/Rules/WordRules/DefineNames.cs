using System.Collections.Generic;
using System.Linq;

namespace PantryTracker.RecipeReader.Rules
{
    internal static partial class EnumerableRules
    {
        public static IEnumerable<Word> DefineNames(this IEnumerable<Word> wordList)
        {
            foreach (var word in wordList.Where(w => w.PartOfSpeech == null))
            {
                word.PartOfSpeech = PartOfSpeech.Name;
            }   

            return wordList;
        }
    }
}
