using System;
using System.Collections.Generic;
using System.Linq;
using PantryTracker.Model.Recipe;
using PantryTracker.RecipeReader.Rules;

namespace PantryTracker.RecipeReader
{
    public class MetadataParser
    {
        private UnitsOfTime _time =
            new UnitsOfTime();

        public Recipe ExtractRecipe(string[] input)
        {
            var recipe = new Recipe()
            {
                RawText = string.Join(Environment.NewLine, input)
            };
            
            var currentLineNumber = 0;
            if (string.IsNullOrEmpty(recipe.Title))
                recipe.Title = input.First().Trim();

            var ingredientParser = new IngredientParser();
            foreach(var sentence in input.Skip(1))
            {
                currentLineNumber++;

                if (string.IsNullOrEmpty(sentence))
                {
                    continue;
                }

                if(sentence.ToLower().Contains("directions") || 
                   sentence.ToLower().Contains("instructions"))
                {
                    break;
                }

                var ingredient = ingredientParser.ProcessSentence(sentence);

                var recentlyAddedPrepTime = false;
                if (string.IsNullOrEmpty(recipe.PrepTime))
                {
                    recentlyAddedPrepTime = true;
                    recipe.PrepTime = GetPrepTime(sentence)?.Trim();
                }

                if (!recentlyAddedPrepTime && null != ingredient)
                    (recipe.Ingredients as List<Ingredient>).Add(ingredient);
            }

            (recipe.Directions as List<string>).AddRange(input.Skip(currentLineNumber + 1));

            return recipe;
        }

        private string GetPrepTime(string sentence)
        {
            var words = sentence.GetWords()
                                .DetectQuantities()
                                .CombineFractions();

            return words.FindPrepTime();
        }
    }
}
