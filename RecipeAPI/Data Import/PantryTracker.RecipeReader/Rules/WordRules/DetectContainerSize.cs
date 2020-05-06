using System.Collections.Generic;
using System.Linq;

namespace PantryTracker.RecipeReader.Rules
{
    internal static partial class EnumerableRules
    {
        public static IEnumerable<Word> DetectContainerSize(this IEnumerable<Word> wordList)
        {
            foreach (var word in wordList.Where(w => w.PartOfSpeech == PartOfSpeech.Unit))
            {
                var size = wordList.Where(w => w.PartOfSpeech == PartOfSpeech.Quantity &&
                                                         w.Position <= word.Position - 1);
                if(size.Count() >= 2)
                {
                    size.Last().PartOfSpeech = PartOfSpeech.ContainerSize;
                }
            }

            return wordList;
        }
    }
}