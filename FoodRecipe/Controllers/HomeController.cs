using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using FoodRecipe.Data;
using FoodRecipe.Models;

namespace FoodRecipe.Controllers
{
    public class HomeController
        : Controller
    {
        private class RecipeRepositoryMock
            : IRecipeRepository
        {
            public IEnumerable<Recipe> GetAll()
                => from number in Enumerable.Range(1, 4)
                   select new Recipe
                   {
                       Name = "test " + number.ToString(),
                       Description = "test",
                       EstimatedPreparationTimeMinutes = 10 * number,
                       RecipeType = Enum.GetValues(typeof(RecipeType)).Cast<RecipeType>().ElementAt(number % Enum.GetValues(typeof(RecipeType)).Length),
                       Ingredients = Enumerable
                           .Range(1, number)
                           .Select(ingredientNumber =>
                               new RecipeIngredient
                               {
                                   Name = "test " + ingredientNumber.ToString(),
                                   Quantity = number * ingredientNumber,
                                   Unit = RecipeIngredientUnit.Grams
                               })
                   };
        }

        private readonly IRecipeRepository _repository;
        private readonly IRecipeFilter _recipeFilter;

        public HomeController()
        {
            _repository = new RecipeRepositoryMock();
            _recipeFilter = new PSharpRecipeFilter(_repository);
        }

        public ActionResult Browse()
            => View(_GetFilterCriteria());
        [ChildActionOnly]
        public ActionResult ShowResults(FilterCriteria filterCriteria)
            => View(_recipeFilter.GetFor(filterCriteria));

        private FilterCriteria _GetFilterCriteria()
            => new FilterCriteria
            {
                RecipeName = Request[nameof(FilterCriteria.RecipeName)] ?? string.Empty,
                RecipeType = _GetRecipeType(Request[nameof(FilterCriteria.RecipeType)]),
                MinPreparationTime = _TryGetDouble(Request[nameof(FilterCriteria.MinPreparationTime)]),
                MaxPreparationTime = _TryGetDouble(Request[nameof(FilterCriteria.MaxPreparationTime)]),
                Ingredients = _GetIncludedIngredients()
            };
        private RecipeType? _GetRecipeType(string value)
        {
            RecipeType recipeType;
            if (Enum.TryParse(value, true, out recipeType))
                return recipeType;
            else
                return null;
        }
        private IEnumerable<FilterIngredient> _GetIncludedIngredients()
            => (from ingredientName in _GetAllIngredients()
                join includedIngredient in from string includedIngredientKey in Request.Params.Keys
                                           where Regex.IsMatch(includedIngredientKey, $@"^{nameof(FilterCriteria.Ingredients)}_\d+$")
                                           select new FilterIngredient
                                           {
                                               Name = Request[includedIngredientKey],
                                               InclusionOption = _GetFilterIngredientInclusionOption(Request[includedIngredientKey + nameof(FilterIngredient.InclusionOption)]).GetValueOrDefault(),
                                               MinQuantity = _TryGetDouble(Request[includedIngredientKey + nameof(FilterIngredient.MinQuantity)]),
                                               MaxQuantity = _TryGetDouble(Request[includedIngredientKey + nameof(FilterIngredient.MaxQuantity)])
                                           }
                on ingredientName.ToLowerInvariant() equals includedIngredient.Name.ToLowerInvariant() into includedIngredientsByName
                select includedIngredientsByName
                    .DefaultIfEmpty(new FilterIngredient { Name = ingredientName })
                    .Single())
                .ToList();
        private IEnumerable<string> _GetAllIngredients()
            => _repository
            .GetAll()
            .SelectMany(recipe => recipe.Ingredients)
            .Select(ingredient => ingredient.Name)
            .Distinct();
        private FilterIngredientInclusionOption? _GetFilterIngredientInclusionOption(string value)
        {
            FilterIngredientInclusionOption filterIngredientInclusionOption;
            if (Enum.TryParse(value, true, out filterIngredientInclusionOption))
                return filterIngredientInclusionOption;
            else
                return null;
        }
        private double? _TryGetDouble(string value)
        {
            double doubleValue;
            if (double.TryParse(value, out doubleValue))
                return doubleValue;
            else
                return null;
        }

        public ActionResult About()
            => View();
    }
}