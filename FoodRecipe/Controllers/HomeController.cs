using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using FoodRecipe.Models;

namespace FoodRecipe.Controllers
{
    public class HomeController
        : Controller
    {
        public ActionResult Browse()
        {
            var filterCriteria = _GetFilterCriteria();
            return View(filterCriteria);
        }
        private IEnumerable<string> _GetAllIngredients()
        {
            yield return "Apple";
            yield return "Curry";
            yield return "Potatoes";
        }

        private FilterCriteria _GetFilterCriteria()
            => new FilterCriteria
            {
                RecipeName = Request[nameof(FilterCriteria.RecipeName)],
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
            => from ingredientName in _GetAllIngredients()
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
               .Single();
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