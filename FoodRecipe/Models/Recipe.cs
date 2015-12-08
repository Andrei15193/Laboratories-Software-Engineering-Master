using System.Collections.Generic;

namespace FoodRecipe.Models
{
    public class Recipe
    {
        public string Name
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public RecipeType RecipeType
        {
            get;
            set;
        }

        public double EstimatedPreparationTimeMinutes
        {
            get;
            set;
        }

        public IEnumerable<RecipeIngredient> Ingredients
        {
            get;
            set;
        }
    }
}