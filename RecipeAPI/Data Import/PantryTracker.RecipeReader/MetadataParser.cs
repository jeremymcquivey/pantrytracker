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
            //detect a prep time
            //numeric followed by time unit
            foreach(var sentence in input)
            {
                currentLineNumber++;

                if(string.IsNullOrEmpty(recipe.Title))
                    recipe.Title = sentence.Trim();

                if (string.IsNullOrEmpty(sentence))
                    break;
                
                var words = sentence.GetWords()
                                    .DetectQuantities()
                                    .CombineFractions();

                if(string.IsNullOrEmpty(recipe.PrepTime))
                {
                    recipe.PrepTime = words.FindPrepTime()?.Trim();
                }
            }

            var ingredientParser = new IngredientParser();
            foreach(var sentence in input.Skip(currentLineNumber))
            {
                currentLineNumber++;

                if (string.IsNullOrEmpty(sentence))
                    break;

                var ingredient = ingredientParser.ProcessSentence(sentence);

                if (null != ingredient)
                    (recipe.Ingredients as List<Ingredient>).Add(ingredient);
            }

            (recipe.Directions as List<string>).AddRange(input.Skip(currentLineNumber));

            return recipe;
        }
    }
}
