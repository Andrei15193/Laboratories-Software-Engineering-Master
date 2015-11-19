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
        public ModelState GetFor(ModelState owner, object aggregate)
        {
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));
            if (aggregate == null)
                throw new ArgumentNullException(nameof(aggregate));

            return _modelStates.GetOrAdd(
                aggregate,
                delegate { return ModelStateProviders.GetFor(owner.Model.GetType(), aggregate.GetType()).GetForAggregate(owner, aggregate); });
        }
    }
}