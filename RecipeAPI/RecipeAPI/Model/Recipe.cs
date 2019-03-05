namespace RecipeAPI.Model
{
    public class Recipe
    {
        public string Title { get; set; }

        public string Citation { get; set; }

        public string Body { get; set; }

        public string PublicState { get; set; } = RecipeStates.Private;
    }
}
