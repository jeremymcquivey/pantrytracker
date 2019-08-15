using System.Collections.Generic;
using System.Linq;
using Sample.ParseTree.Model;

namespace Sample.ParseTree.Rules
{
    internal static partial class EnumerableRules
    {
        public static IEnumerable<Word> DefineNames(this IEnumerable<Word> wordList)
        {
            foreach (var word in wordList.Where(w => w.PartOfSpeech == null))
            {
                word.PartOfSpeech = PartOfSpeech.Name;
            }   

            return wordList;
        }
    }
}
