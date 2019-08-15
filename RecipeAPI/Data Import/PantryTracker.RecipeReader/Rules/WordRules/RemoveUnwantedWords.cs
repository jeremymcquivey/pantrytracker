using System.Collections.Generic;
using System.Linq;

namespace PantryTracker.RecipeReader.Rules
{
    internal static partial class EnumerableRules
    {
        internal static IEnumerable<Word> RemoveUnwantedWords(this IEnumerable<Word> wordList)
        {
            var nonWords = wordList.Where(w => w.PartOfSpeech != PartOfSpeech.Name);

            var maxPosition = nonWords.Any() ? nonWords
                                      .Select(w => w.Position)
                                      .Min() : 0;

            return wordList.Where(x => x.PartOfSpeech != PartOfSpeech.Name ||
                                           x.Position >= maxPosition)
                            .ToList()
                            .Reposition();
        }
    }
}