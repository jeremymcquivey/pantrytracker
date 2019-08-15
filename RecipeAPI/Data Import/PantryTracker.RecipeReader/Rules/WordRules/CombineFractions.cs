using System.Collections.Generic;
using System.Linq;
using PantryTracker.RecipeReader;

namespace PantryTracker.RecipeReader.Rules
{
    internal static partial class EnumerableRules
    {
        public static IEnumerable<Word> CombineFractions(this IEnumerable<Word> wordList)
        {
            var positionsToRemove = new List<int>();
            foreach (var fraction in wordList.Where(w => w.PartOfSpeech == PartOfSpeech.Fraction))
            {
                var prefix = wordList.SingleOrDefault(x => x.Position == fraction.Position - 1);
                if (prefix?.PartOfSpeech == PartOfSpeech.Quantity)
                {
                    prefix.Contents = $"{prefix.Contents} {fraction.Contents}";
                    positionsToRemove.Add(fraction.Position);
                }
                else
                {
                    fraction.PartOfSpeech = PartOfSpeech.Quantity;
                }
            }

            return wordList.Where(w => !positionsToRemove.Contains(w.Position))
                           .ToList()
                           .Reposition();
        }
    }
}
