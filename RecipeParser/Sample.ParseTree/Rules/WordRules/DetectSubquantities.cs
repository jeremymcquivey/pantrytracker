using System;
using System.Collections.Generic;
using System.Linq;
using Sample.Grammar;
using Sample.ParseTree.Model;

namespace Sample.ParseTree.Rules
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
