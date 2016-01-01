using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace BillPath
{
    public static class ModelStateProviders
    {
        private struct ModelStateProviderKey
            : IEquatable<ModelStateProviderKey>
        {
            private readonly Type _modelType;
            private readonly Type _modelContainerType;

            public ModelStateProviderKey(Type modelType)
                : this(modelType, typeof(object))
            {
            }
            public ModelStateProviderKey(Type modelType, Type modelContainerType)
            {
                if (modelType == null)
                    throw new ArgumentNullException(nameof(modelType));
                if (modelContainerType == null)
                    throw new ArgumentNullException(nameof(modelContainerType));

                _modelType = modelType;
                _modelContainerType = modelContainerType;
            }

            public ModelStateProviderKey GetForBaseClassOrDefault()
                => _modelType.GetTypeInfo().BaseType != null
                ? new ModelStateProviderKey(
                    _modelType.GetTypeInfo().BaseType,
                    _modelContainerType)
                : default(ModelStateProviderKey);

            public override bool Equals(object obj)
            {
                var modelStateProviderKey = obj as ModelStateProviderKey?;
                return modelStateProviderKey.HasValue
                    && Equals(modelStateProviderKey.Value);
            }
            public bool Equals(ModelStateProviderKey other)
                => Equals(_modelType, other._modelType)
                && Equals(_modelContainerType, other._modelContainerType);
            public override int GetHashCode()
                => (_modelType?.GetHashCode() ?? 0) ^ (_modelContainerType?.GetHashCode() ?? 0);
        }

        private static ModelStateProvider _defaultModelStateProvider
            = new DefaultModelStateProvider();
        private static readonly ConcurrentDictionary<ModelStateProviderKey, ModelStateProvider> _modelStateProviders =
            new ConcurrentDictionary<ModelStateProviderKey, ModelStateProvider>();

        public static ModelStateProvider DefaultModelStateProvider
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

        public static ModelStateProvider GetFor(Type modelType)
        {
            ModelStateProvider modelStateProvider;
            if (_TryGetModelStateProvider(
                new ModelStateProviderKey(modelType),
                out modelStateProvider))
                return modelStateProvider;
            else
                return _defaultModelStateProvider;
        }
        public static ModelStateProvider GetFor(Type aggregateType, Type modelContainerType)
        {
            ModelStateProvider modelStateProvider;
            if (_TryGetModelStateProvider(
                new ModelStateProviderKey(
                    aggregateType,
                    modelContainerType),
                out modelStateProvider))
                return modelStateProvider;
            else
                return _defaultModelStateProvider;
        }

        private static bool _TryGetModelStateProvider(ModelStateProviderKey modelStateProviderKey, out ModelStateProvider modelStateProvider)
        {
            modelStateProvider = null;
            while (!modelStateProviderKey.Equals(default(ModelStateProviderKey))
                && !_modelStateProviders.TryGetValue(modelStateProviderKey, out modelStateProvider))
            {
                modelStateProviderKey = modelStateProviderKey.GetForBaseClassOrDefault();
                modelStateProvider = null;
            }

            return (modelStateProvider != null);
        }

        public static void Add(ModelStateProvider modelStateProvider)
        {
            if (modelStateProvider == null)
                throw new ArgumentNullException(nameof(modelStateProvider));

            _modelStateProviders.AddOrUpdate(
                new ModelStateProviderKey(
                    modelStateProvider.ModelType,
                    modelStateProvider.ModelContainerType),
                modelStateProvider,
                delegate { return modelStateProvider; });
        }
        public static void Remove(ModelStateProvider modelStateProvider)
        {
            if (modelStateProvider == null)
                throw new ArgumentNullException(nameof(modelStateProvider));

            if (_modelStateProviders.Values.Contains(modelStateProvider))
                _modelStateProviders.TryRemove(
                    new ModelStateProviderKey(
                        modelStateProvider.ModelType,
                        modelStateProvider.ModelContainerType),
                    out modelStateProvider);
        }
    }
}