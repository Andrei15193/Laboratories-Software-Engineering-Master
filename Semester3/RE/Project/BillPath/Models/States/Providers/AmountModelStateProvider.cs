namespace BillPath.Models.States.Providers
{
    public class AmountModelStateProvider<TTransaction>
        : DefaultModelStateProvider<Amount, TTransaction>
        where TTransaction : Transaction<TTransaction>
    {
        protected override ModelState GetModelStateFor(TTransaction transaction, Amount amount)
            => new AmountModelState<TTransaction>(transaction);
    }
}