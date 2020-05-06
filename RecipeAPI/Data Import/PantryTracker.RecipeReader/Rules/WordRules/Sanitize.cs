using System.Collections.Generic;
using System.Linq;

namespace PantryTracker.RecipeReader.Rules
{
    internal static partial class EnumerableRules
    {
        private static readonly char[] NumericCharacters = 
        {
            '1',
            '2',
            '3',
            '4',
            '5',
            '6',
            '7',
            '8',
            '9',
            '0',
            '.',
            ',',
            '/',
            ' ',
            '-'
        };

        internal static IEnumerable<Word> Sanitize(this IEnumerable<Word> wordList)
        {
            foreach(var word in wordList)
            {
                switch(word.PartOfSpeech)
                {
                    case PartOfSpeech.Quantity:
                    case PartOfSpeech.ContainerSize:
                        word.Contents = SanitizeWord(word.Contents);
                        break;
                }
            }

            return wordList;
        }

        internal static string SanitizeWord(string input)
        {
            return new string(input.Where(c => NumericCharacters.Contains(c)).ToArray());
        }
    }
}
