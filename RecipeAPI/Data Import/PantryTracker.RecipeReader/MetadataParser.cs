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

            var directions = new List<string>();
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

                if(sentence.ToLower().Contains("ingredients"))
                {
                    (recipe.Ingredients as List<Ingredient>).Clear();
                    continue;
                }

                var ingredient = ingredientParser.ProcessSentence(sentence);

                var recentlyAddedPrepTime = false;
                if (string.IsNullOrEmpty(recipe.PrepTime))
                {
                    recipe.PrepTime = GetPrepTime(sentence)?.Trim();
                    recentlyAddedPrepTime = !string.IsNullOrEmpty(recipe.PrepTime);
                }

                if (!recentlyAddedPrepTime && null != ingredient)
                    (recipe.Ingredients as List<Ingredient>).Add(ingredient);
            }

            directions.AddRange(input.Skip(currentLineNumber + 1));

            List<string> toRemove = new List<string>();
            foreach(var direction in directions)
            {
                var ingredient = ingredientParser.ProcessSentence(direction);

                if(!string.IsNullOrEmpty(ingredient.Unit) && (!string.IsNullOrEmpty(ingredient.Quantity) || !string.IsNullOrEmpty(ingredient.SubQuantity)))
                {
                    //actually an ingredient.
                    (recipe.Ingredients as List<Ingredient>).Add(ingredient);
                    toRemove.Add(direction);
                }
            }

            foreach(var direction in toRemove)
            {
                (recipe.Directions as List<string>).Remove(direction);
            }

            var index = 1;
            recipe.Directions = (directions.Select(dir => new Direction()
            {
                Index = index++,
                Text = dir
            })).ToList();

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
