using System.Collections.Generic;

namespace FoodRecipe.Models
{
    public class FilterCriteria
    {
        public string RecipeName
        {
            get;
            set;
        }

        public RecipeType? RecipeType
        {
            get;
            set;
        }

        public double? MinPreparationTime
        {
            get;
            set;
        }
        public double? MaxPreparationTime
        {
            get;
            set;
        }

        public IEnumerable<FilterIngredient> Ingredients
        {
            get;
            set;
        }

        public IEnumerable<string> ExcludedIngredients
        {
            get;
            set;
        }
    }
}