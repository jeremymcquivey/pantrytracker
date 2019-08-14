using System.Collections.Generic;
using System.Linq;
using Sample.ParseTree.Model;

namespace Sample.ParseTree.Rules
{
    internal static partial class StringRules
    {
        public static IEnumerable<Word> GetWords(this string sentence)
        {
            var index = 0;
            return sentence?.Split(" ")
                            .Select(word => new Word
                            {
                                Position = index++,
                                Contents = word
                            })
                            .ToList();
        }
    }
}
