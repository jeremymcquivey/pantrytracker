using System.Collections.Generic;

namespace PantryTracker.RecipeReader.Rules
{
    internal static partial class EnumerableRules
    {
        public static IEnumerable<Word> DetectContainer(this IEnumerable<Word> wordList)
        {
            return wordList;
        }
    }
}