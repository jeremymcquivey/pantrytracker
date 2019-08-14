using System.Collections.Generic;

namespace Sample.ParseTree.Model
{
    public class Recipe
    {
        public string Name { get; set; }

        public string PrepTime { get; set; }

        public string Servings { get; set; }

        public string Credit { get; set; }

        public string RawText { get; set; }

        public IEnumerable<Ingredient> Ingredients { get; set; }
            = new List<Ingredient>();

        public IEnumerable<string> Directions { get; set; }
            = new List<string>();
    }
}