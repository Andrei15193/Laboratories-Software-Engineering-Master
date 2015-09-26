#if NETFX_CORE
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace BillPath.Models
{
    public sealed class ModelValidator
    {
        public IEnumerable<ValidationResult> Validate(object instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            var instanceType = instance.GetType();

            return (from runtimeProperty in instanceType.GetRuntimeProperties()
                    where runtimeProperty.CanRead
                    let validationContext =
                        new ValidationContext(instance)
                        {
                            MemberName = runtimeProperty.Name
                        }
                    let propertyValue = runtimeProperty.GetValue(instance)
                    from validationAttribute in runtimeProperty.GetCustomAttributes<ValidationAttribute>(true)
                    let validationResult = validationAttribute.GetValidationResult(propertyValue, validationContext)
                    where validationResult != null
                    select validationResult)
                   .Concat((instance as IValidatableObject)?.Validate(new ValidationContext(instance))
                           ?? Enumerable.Empty<ValidationResult>());
        }
    }
}
#endif