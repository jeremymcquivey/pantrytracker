using System;
using System.Collections.Generic;
using System.Linq;
using Sample.ParseTree.Model;

namespace Sample.ParseTree.Rules
{
    internal static partial class EnumerableRules
    {
        public static string FindPrepTime(this IEnumerable<Word> words)
        {
            var time = new UnitsOfTime();

            foreach (var numeric in words.Where(w => w.PartOfSpeech == PartOfSpeech.Quantity ||
                                                     w.PartOfSpeech == PartOfSpeech.Fraction))
            {
                var next = words.SingleOrDefault(w => w.Position == numeric.Position + 1);
                if (time.isUnitOfTime(next?.Contents))
                {
                    return $"{numeric.Contents} {next.Contents}";
                }
            }

            return null;
        }
    }
}
