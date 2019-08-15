using System.Collections.Generic;
using System.Linq;

namespace PantryTracker.RecipeReader.Rules
{
    internal static partial class StringRules
    {
        public static IEnumerable<Word> GetWords(this string sentence)
        {
            var index = 0;
            return sentence?.Split(new char[] { ' ', '-' })
                            .Select(word => new Word
                            {
                                Position = index++,
                                Contents = word
                            })
                            .ToList();
        }
    }
}
