using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FoodRecipe.Data;
using FoodRecipe.Helpers;
using FoodRecipe.Models;
using FoodRecipe.Models.Predicates;

namespace FoodRecipe.Controllers
{
    public class PSharpRecipeFilter
        : IRecipeFilter
    {
        private readonly IRecipeRepository _repository;

        public PSharpRecipeFilter(IRecipeRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _repository = repository;
        }

        public IEnumerable<Recipe> GetFor(FilterCriteria filterCriteria)
        {
            if (filterCriteria == null)
                throw new ArgumentNullException(nameof(filterCriteria));

            return from recipe in _repository.GetAll()
                   where (from filterRecipeName in Regex.Replace(filterCriteria.RecipeName.Trim(), @"\s+", " ").Split(' ')
                          let normalizedFilterCriteria =
                              new FilterCriteria
                              {
                                  RecipeName = filterRecipeName,
                                  MinPreparationTime = filterCriteria.MinPreparationTime,
                                  MaxPreparationTime = filterCriteria.MaxPreparationTime,
                                  RecipeType = filterCriteria.RecipeType,
                                  Ingredients = filterCriteria.Ingredients
                              }
                          where PrologHelper.FindAll<FilterRecipe_2>(normalizedFilterCriteria, recipe).Any()
                          select filterRecipeName)
                         .Any()
                   select recipe;
        }
    }
}