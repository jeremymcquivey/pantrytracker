using System.Collections.Generic;
using System.Linq;

namespace PantryTracker.RecipeReader.Rules
{
    internal static partial class EnumerableRules
    {
        public static IEnumerable<Word> DetectSubquantities(this IEnumerable<Word> wordList)
        {
            foreach (var word in wordList.Where(w => w.PartOfSpeech == PartOfSpeech.Unit))
            {
                var subQuantity = wordList.SingleOrDefault(w => w.Position == word.Position - 1);
                if(null != subQuantity)
                {
                    subQuantity.PartOfSpeech = PartOfSpeech.SubQuantity;
                }
            }

            return wordList;
        }
    }
}
