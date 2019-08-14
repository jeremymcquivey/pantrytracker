using System;
using System.Collections.Generic;
using System.Linq;
using Sample.Grammar;
using Sample.ParseTree.Model;

namespace Sample.ParseTree.Rules
{
    internal static partial class EnumerableRules
    {
        internal static IEnumerable<Word> RemoveUnwantedWords(this IEnumerable<Word> wordList)
        {
            var maxPosition = wordList.Where(w => w.PartOfSpeech != PartOfSpeech.Name)
                                      .Select(w => w.Position)
                                      .Max();

            return wordList.Where(x => x.PartOfSpeech != PartOfSpeech.Name ||
                                           x.Position > maxPosition)
                            .ToList()
                            .Reposition();
        }
    }
}
