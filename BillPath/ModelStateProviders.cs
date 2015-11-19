using System;
using System.Collections.Concurrent;

namespace BillPath
{
    public static class ModelStateProviders
    {
        private static IModelStateProvider _defaultModelStateProvider
            = new DefaultModelStateProvider();
        private static readonly ConcurrentDictionary<Tuple<Type, Type>, IModelStateProvider> _modelStateProviders =
            new ConcurrentDictionary<Tuple<Type, Type>, IModelStateProvider>();

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
                Tuple.Create(modelType, (Type)null),
                modelStateProvider,
                delegate { return modelStateProvider; });
        }
        public static void SetFor<TModel>(IModelStateProvider modelStateProvider)
           => SetFor(typeof(TModel), modelStateProvider);

        public static IModelStateProvider GetFor(Type modelType)
        {
            IModelStateProvider modelStateProvider;
            if (_modelStateProviders.TryGetValue(Tuple.Create(modelType, (Type)null), out modelStateProvider))
                return modelStateProvider;
            else
                return _defaultModelStateProvider;
        }
        public static IModelStateProvider GetFor<TModel>()
           => GetFor(typeof(TModel));

        public static void SetFor(Type rootType, Type aggregateType, IModelStateProvider modelStateProvider)
        {
            if (modelStateProvider == null)
                throw new ArgumentNullException(nameof(modelStateProvider));

            _modelStateProviders.AddOrUpdate(
                Tuple.Create(rootType, aggregateType),
                modelStateProvider,
                delegate { return modelStateProvider; });
        }
        public static void SetFor<TRoot, TAggregate>(IModelStateProvider modelStateProvider)
           => SetFor(typeof(TRoot), typeof(TAggregate), modelStateProvider);

        public static IModelStateProvider GetFor(Type rootType, Type aggregateType)
        {
            IModelStateProvider modelStateProvider;
            if (_modelStateProviders.TryGetValue(Tuple.Create(rootType, aggregateType), out modelStateProvider))
                return modelStateProvider;
            else
                return _defaultModelStateProvider;
        }
        public static IModelStateProvider GetFor<TRoot, TAggregate>()
           => GetFor(typeof(TRoot), typeof(TAggregate));
    }
}