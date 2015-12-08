using System.Collections.Generic;
using FoodRecipe.Models;

namespace FoodRecipe.Data
{
    public interface IRecipeRepository
    {
        IEnumerable<Recipe> GetAll();
    }
}