using System;
using System.Collections.Concurrent;

namespace BillPath
{
    public static class ModelStateProviders
    {
        private static IModelStateProvider _defaultModelStateProvider
            = new DefaultModelStateProvider();
        private static readonly ConcurrentDictionary<Type, IModelStateProvider> _modelStateProviders =
            new ConcurrentDictionary<Type, IModelStateProvider>();

        public static IModelStateProvider DefaultModelStateProvider
        {
            get
            {
                return _defaultModelStateProvider;
            }
            set
            {
                _defaultModelStateProvider = value ?? new DefaultModelStateProvider();
            }
        }

        public static void SetFor(Type modelType, IModelStateProvider modelStateProvider)
        {
            if (modelStateProvider == null)
                throw new ArgumentNullException(nameof(modelStateProvider));

            _modelStateProviders.AddOrUpdate(
                modelType,
                modelStateProvider,
                delegate { return modelStateProvider; });
        }
        public static IModelStateProvider GetFor(Type modelType)
        {
            IModelStateProvider modelStateProvider;
            if (_modelStateProviders.TryGetValue(modelType, out modelStateProvider))
                return modelStateProvider;
            else
                return _defaultModelStateProvider;
        }
    }
}