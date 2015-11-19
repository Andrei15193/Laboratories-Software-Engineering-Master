using System;

namespace BillPath.Models.States
{
    public class AmountModelStateModelStateProvider<TTransaction>
        : IModelStateProvider
        where TTransaction : Transaction<TTransaction>
    {
        public ModelState GetForAggregate(ModelState owner, object aggregate)
            => new AmountModelState<TTransaction>(owner);

        public ModelState GetForRoot(object model)
            => new ModelState(model);
    }

    public class AmountModelState<TTransaction>
        : ModelState
        where TTransaction : Transaction<TTransaction>
    {
        private sealed class AmountAdapter
        {
            private readonly ModelState _owner;

            public AmountAdapter(ModelState owner)
            {
                if (owner == null)
                    throw new ArgumentNullException(nameof(owner));

                _owner = owner;
            }

            public decimal Value
            {
                get
                {
                    return ((TTransaction)_owner.Model).Amount.Value;
                }
                set
                {
                    _owner[nameof(Amount)] = new Amount(value, Currency);
                }
            }
            public Currency Currency
            {
                get
                {
                    return ((TTransaction)_owner.Model).Amount.Currency;
                }
                set
                {
                    _owner[nameof(Amount)] = new Amount(Value, value);
                }
            }
        }

        public AmountModelState(ModelState owner)
            : base(new AmountAdapter(owner))
        {
        }
    }
}