using System.Collections.Generic;
using System.Linq;
using Sample.ParseTree.Model;

namespace Sample.ParseTree.Rules
{
    internal static partial class EnumerableRules
    {
        public static IEnumerable<Word> DetectQuantities(this IEnumerable<Word> wordList)
        {
            foreach (var word in wordList.Where(w => w.PartOfSpeech == null))
            {
                var fractionParts = word.Contents.Split('/');

                if (double.TryParse(word.Contents, out double numericValue))
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
