using System.ComponentModel.DataAnnotations;

namespace FoodRecipe.Models
{
    public enum FilterIngredientInclusionOption
    {
        [Display(Name = "May contain")]
        MayContain,
        [Display(Name = "Must contain")]
        MustContain,
        [Display(Name = "Must not contain")]
        MustNotContain
    }
}