using PantryTracker.Model.Meta;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

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

        internal static IEnumerable<Word> Sanitize(this IEnumerable<Word> wordList, UnitAliases units)
        {
            foreach(var word in wordList)
            {
                switch(word.PartOfSpeech)
                {
                    case PartOfSpeech.Quantity:
                    case PartOfSpeech.ContainerSize:
                        word.Contents = SanitizeNumber(word.Contents);
                        break;
                    case PartOfSpeech.Unit:
                        word.Contents = units.GetSanitizedUnit(word.Contents);
                        break;
                }
            }

            return wordList;
        }

        internal static string SanitizeNumber(string input)
        {
            return new string(input.Where(c => NumericCharacters.Contains(c)).ToArray());
        }
    }
}
