using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace BillPath
{
    public sealed class ModelValidator
    {
        private static readonly ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> _runtimePropertiesCache
            = new ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>();
        private static IEnumerable<PropertyInfo> _GetRuntimePropertiesFor(Type type)
            => _runtimePropertiesCache.GetOrAdd(
                type,
                typeToCache => typeToCache.GetRuntimeProperties().ToList());

        public IEnumerable<ValidationResult> Validate(object model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var instanceType = model.GetType();

            return (from runtimeProperty in _GetRuntimePropertiesFor(instanceType)
                    where runtimeProperty.CanRead
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