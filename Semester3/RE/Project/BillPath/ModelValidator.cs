using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace BillPath
{
    public sealed class ModelValidator
    {
        public IEnumerable<ValidationResult> Validate(object model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var instanceType = model.GetType();

            return (from runtimeProperty in instanceType.GetRuntimeProperties()
                    let hasPublicGetter = runtimeProperty.GetMethod?.IsPublic ?? false
                    let isStatic = runtimeProperty.GetMethod?.IsStatic ?? false
                    let hasParameters = runtimeProperty.GetIndexParameters().Length > 0
                    where hasPublicGetter && !hasParameters && !isStatic
                    let validationContext =
                        new ValidationContext(model)
                        {
                            MemberName = runtimeProperty.Name
                        }
                    let propertyValue = runtimeProperty.GetValue(model)
                    from validationAttribute in runtimeProperty.GetCustomAttributes<ValidationAttribute>(true)
                    let validationResult = validationAttribute.GetValidationResult(propertyValue, validationContext)
                    where validationResult != null
                    select validationResult)
                   .Concat((model as IValidatableObject)?.Validate(new ValidationContext(model))
                           ?? Enumerable.Empty<ValidationResult>());
        }
    }
}