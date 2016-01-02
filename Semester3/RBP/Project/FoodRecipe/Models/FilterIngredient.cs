namespace FoodRecipe.Models
{
    public class FilterIngredient
    {
        public string Name
        {
            get;
            set;
        }

        public FilterIngredientInclusionOption InclusionOption
        {
            get;
            set;
        }

        public double? MinQuantity
        {
            get;
            set;
        }
        public double? MaxQuantity
        {
            get;
            set;
        }
    }
}