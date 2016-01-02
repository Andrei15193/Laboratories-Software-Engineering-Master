using System.ComponentModel.DataAnnotations;

namespace FoodRecipe.Models
{
    public enum RecipeType
    {
        [Display(Name = "Soup")]
        Soup,
        [Display(Name = "Main Course")]
        MainCourse,
        [Display(Name = "Dessert")]
        Dessert
    }
}