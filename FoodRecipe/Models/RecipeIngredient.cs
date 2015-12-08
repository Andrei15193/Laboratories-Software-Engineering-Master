namespace FoodRecipe.Models
{
    public class RecipeIngredient
    {
        public string Name
        {
            get;
            set;
        }

        public double Quantity
        {
            get;
            set;
        }

        public RecipeIngredientUnit Unit
        {
            get;
            set;
        }
    }
}