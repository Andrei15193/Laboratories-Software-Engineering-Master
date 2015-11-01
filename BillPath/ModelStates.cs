using System;
using System.Collections.Concurrent;

namespace BillPath
{
    public static class ModelStates
    {
        private static readonly ConcurrentDictionary<object, ModelState> _modelStates
            = new ConcurrentDictionary<object, ModelState>();

        public static ModelState GetFor(object model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            return _modelStates.GetOrAdd(model, newModel => new ModelState(newModel));
        }

        public static void Clear()
            => _modelStates.Clear();
    }
}