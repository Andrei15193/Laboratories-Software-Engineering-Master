using System;

namespace BillPath
{
    public class DefaultModelStateProvider
        : ModelStateProvider
    {
        public override Type ModelType
            => typeof(object);

        protected override ModelState GetModelStateFor(object model)
            => new ModelState(model);
        protected override ModelState GetModelStateFor(object container, object aggregate)
            => new ModelState(aggregate);
    }
    public class DefaultModelStateProvider<TModel>
        : DefaultModelStateProvider
    {
        public sealed override Type ModelType
            => typeof(TModel);

        protected sealed override ModelState GetModelStateFor(object model)
            => GetModelStateFor((TModel)model);
        protected virtual ModelState GetModelStateFor(TModel model)
            => base.GetModelStateFor(model);

        protected sealed override ModelState GetModelStateFor(object container, object aggregate)
            => GetModelStateFor(container, (TModel)aggregate);
        protected virtual ModelState GetModelStateFor(object container, TModel aggregate)
            => base.GetModelStateFor(container, aggregate);
    }

    public class DefaultModelStateProvider<TModel, TModelContainer>
        : DefaultModelStateProvider<TModel>
    {
        public sealed override Type ModelContainerType
            => typeof(TModelContainer);

        protected sealed override ModelState GetModelStateFor(object container, TModel aggregate)
            => GetModelStateFor((TModelContainer)container, aggregate);
        protected virtual ModelState GetModelStateFor(TModelContainer container, TModel aggregate)
            => base.GetModelStateFor(container, aggregate);
    }
}