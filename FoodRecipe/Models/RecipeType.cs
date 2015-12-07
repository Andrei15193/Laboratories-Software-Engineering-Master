using System.ComponentModel.DataAnnotations;

namespace FoodRecipe.Models
{
    public enum RecipeType
    {
        Soup,
        [Display(Name = "Main Course")]
        MainCourse,
        Dessert
    }
}