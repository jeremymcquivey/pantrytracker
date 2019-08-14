using System;
using System.Collections.Generic;
using System.Linq;
using Sample.Grammar;
using Sample.ParseTree.Model;

namespace Sample.ParseTree.Rules
{
    internal static partial class EnumerableRules
    {
        private static IEnumerable<Word> Reposition(this IEnumerable<Word> wordList)
        {
            var wordArray = wordList.ToArray();
            for (int i = 0; i < wordArray.Length; i++)
            {
                wordList.First(x => x.Position == wordArray[i].Position)
                        .Position = i;
            }

            return wordList.ToList();
        }
    }
}