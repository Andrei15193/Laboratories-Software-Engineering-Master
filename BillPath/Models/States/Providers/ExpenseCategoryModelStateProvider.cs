﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BillPath.Models.States.Providers
{
    public class ExpenseCategoryModelStateProvider
        : DefaultModelStateProvider<ExpenseCategory>
    {
        private static readonly ICollection<ModelState> _cachedModelStates =
            new List<ModelState>();

        private sealed class ExpenseCategory
            : Models.ExpenseCategory, IValidatableObject
        {
            public ExpenseCategory(Models.ExpenseCategory expenseCategory)
            {
                Name = expenseCategory.Name;
                Color = expenseCategory.Color;
            }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (_cachedModelStates.Any(cachedModelState =>
                    !ReferenceEquals(this, cachedModelState.Model)
                    && string.Equals(
                        Name,
                        (string)cachedModelState[nameof(Name)],
                        StringComparison.OrdinalIgnoreCase)))
                    yield return new ValidationResult("Category names must be unique", new[] { nameof(Name) });
            }
        }

        protected override ModelState GetModelStateFor(Models.ExpenseCategory model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
                return base.GetModelStateFor(new ExpenseCategory(model));

            var modelState = _cachedModelStates
                .Where(cachedModelState => string.Equals(
                    model.Name,
                    (string)cachedModelState[nameof(ExpenseCategory.Name)],
                    StringComparison.OrdinalIgnoreCase))
                .Select(cachedModelState => cachedModelState)
                .FirstOrDefault();

            if (modelState == null)
            {
                modelState = base.GetModelStateFor(new ExpenseCategory(model));
                _cachedModelStates.Add(modelState);
            }

            return modelState;
        }
        protected override ModelState GetModelStateFor(object container, Models.ExpenseCategory aggregate)
            => GetModelStateFor(aggregate);
    }
}