using System;
using System.Collections.Concurrent;

namespace BillPath
{
    internal sealed class ModelStateCache
    {
        private readonly ConcurrentDictionary<object, ModelState> _modelStates;

        public ModelStateCache()
        {
            _modelStates = new ConcurrentDictionary<object, ModelState>();
        }
        public ModelStateCache(ModelState root)
            : this()
        {
            if (root == null)
                throw new ArgumentNullException(nameof(root));

            _modelStates.TryAdd(root.Model, root);
        }

        public ModelState GetFor(object model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            return _modelStates.GetOrAdd(
                model,
                delegate { return ModelStateProviders.GetFor(model.GetType()).GetForRoot(model); });
        }
        public ModelState GetFor(object container, object aggregate)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));
            if (aggregate == null)
                throw new ArgumentNullException(nameof(aggregate));

            return _modelStates.GetOrAdd(
                aggregate,
                delegate { return ModelStateProviders.GetFor(aggregate.GetType(), container.GetType()).GetForAggregate(container, aggregate); });
        }
    }
}