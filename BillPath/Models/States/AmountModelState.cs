using System;

namespace BillPath.Models.States
{
    public class AmountModelState<TTransaction>
        : ModelState
        where TTransaction : Transaction<TTransaction>
    {
        private sealed class AmountAdapter
        {
            private readonly TTransaction _owner;

            public AmountAdapter(TTransaction owner)
            {
                if (owner == null)
                    throw new ArgumentNullException(nameof(owner));

                _owner = owner;
            }

            public decimal Value
            {
                get
                {
                    return _owner.Amount.Value;
                }
                set
                {
                    _owner.Amount = new Amount(value, Currency);
                }
            }
            public Currency Currency
            {
                get
                {
                    return _owner.Amount.Currency;
                }
                set
                {
                    _owner.Amount = new Amount(Value, value);
                }
            }
        }

        public AmountModelState(TTransaction container)
            : base(new AmountAdapter(container))
        {
        }
    }
}