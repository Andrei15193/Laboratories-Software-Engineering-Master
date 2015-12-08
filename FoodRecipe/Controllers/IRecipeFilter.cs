using System.Collections.Generic;
using FoodRecipe.Models;

namespace FoodRecipe.Controllers
{
    public interface IRecipeFilter
    {
        IEnumerable<Recipe> GetFor(FilterCriteria filterCriteria);
    }
}