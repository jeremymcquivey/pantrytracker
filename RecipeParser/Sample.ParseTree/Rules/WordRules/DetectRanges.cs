using Sample.ParseTree.Model;
using System.Collections.Generic;
using System.Linq;

namespace Sample.ParseTree.Rules
{
    internal static partial class EnumerableRules
    {
        public static IEnumerable<Word> DetectRanges(this IEnumerable<Word> wordList)
        {
            var positionsToRemove = new List<int>();
            foreach (var word in wordList.Where(x => x.PartOfSpeech == PartOfSpeech.Quantity ||
                                                    x.PartOfSpeech == PartOfSpeech.Fraction))
            {
                var next = wordList.SingleOrDefault(w => w.Position == word.Position + 1);
                var nextOfNext = wordList.SingleOrDefault(w => w.Position == word.Position + 2);

                if (next?.PartOfSpeech == PartOfSpeech.Quantity &&
                    nextOfNext?.PartOfSpeech != PartOfSpeech.SubQuantity)
                {
                    positionsToRemove.Add(next.Position);
                    word.Contents = $"{word.Contents}-{next.Contents}";
                }
            }

            return wordList.Where(w => !positionsToRemove.Contains(w.Position))
                           .ToList()
                           .Reposition();
        }
    }
}
