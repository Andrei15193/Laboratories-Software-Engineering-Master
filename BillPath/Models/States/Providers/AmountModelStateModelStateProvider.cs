using System;

namespace BillPath.Models.States.Providers
{
    public class AmountModelStateModelStateProvider<TTransaction>
        : DefaultModelStateProvider
        where TTransaction : Transaction<TTransaction>
    {
        public override Type ModelType
            => typeof(Amount);
        public override Type ModelContainerType
            => typeof(TTransaction);

        protected override ModelState GetModelStateFor(object container, object aggregate)
            => new AmountModelState<TTransaction>((TTransaction)container);
    }
}