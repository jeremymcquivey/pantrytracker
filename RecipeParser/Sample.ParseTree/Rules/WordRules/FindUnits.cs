using System.Collections.Generic;
using System.Linq;
using Sample.ParseTree.Model;

namespace Sample.ParseTree.Rules
{
    internal static partial class EnumerableRules
    {
        public static IEnumerable<Word> FindUnits(this IEnumerable<Word> wordList, UnitsOfMeasure units)
        {
            foreach (var word in wordList.Where(w => w.PartOfSpeech == null))
            {
                if (units.IsUnitOfMeasure(word.Contents))
                {
                    word.PartOfSpeech = PartOfSpeech.Unit;
                }
            }
            return wordList;
        }
    }
}