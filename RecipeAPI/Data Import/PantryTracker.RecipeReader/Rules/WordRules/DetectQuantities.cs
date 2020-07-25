using System.Collections.Generic;
using System.Linq;

namespace PantryTracker.RecipeReader.Rules
{
    internal static partial class EnumerableRules
    {
        public static IEnumerable<Word> DetectQuantities(this IEnumerable<Word> wordList)
        {
            foreach (var word in wordList.Where(w => w.PartOfSpeech == null))
            {
                var newWord = SanitizeNumber(word.Contents);
                var fractionParts = newWord.Split('/');

                if (double.TryParse(newWord, out double numericValue))
                {
                    word.PartOfSpeech = PartOfSpeech.Quantity;
                }
                else if (fractionParts.Length == 2 && int.TryParse(fractionParts[0], out int numerator)
                                              && int.TryParse(fractionParts[1], out int denomenator))
                {
                    word.PartOfSpeech = PartOfSpeech.Fraction;
                }
            }

            return wordList;
        }
    }
}
