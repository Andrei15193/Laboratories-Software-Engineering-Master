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
}