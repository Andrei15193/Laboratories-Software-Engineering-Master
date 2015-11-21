using System;
using System.Reflection;

namespace BillPath
{
    public abstract class ModelStateProvider
    {
        public abstract Type ModelType
        {
            get;
        }
        public virtual Type ModelContainerType
            => typeof(object);

        public ModelState GetForRoot(object model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            if (!ModelType.GetTypeInfo().IsAssignableFrom(model.GetType().GetTypeInfo()))
                throw new ArgumentException($"Must be an instance of type {ModelType.FullName}", nameof(model));

            return GetModelStateFor(model);
        }
        protected abstract ModelState GetModelStateFor(object model);

        public ModelState GetForAggregate(object container, object aggregate)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));
            if (!ModelContainerType.GetTypeInfo().IsAssignableFrom(container.GetType().GetTypeInfo()))
                throw new ArgumentException($"Must be an instance of type {ModelContainerType.FullName}", nameof(container));

            if (aggregate == null)
                throw new ArgumentNullException(nameof(aggregate));
            if (!ModelType.GetTypeInfo().IsAssignableFrom(aggregate.GetType().GetTypeInfo()))
                throw new ArgumentException($"Must be an instance of type {ModelType.FullName}", nameof(aggregate));

            return GetModelStateFor(container, aggregate);
        }
        protected abstract ModelState GetModelStateFor(object root, object aggregate);
    }
}