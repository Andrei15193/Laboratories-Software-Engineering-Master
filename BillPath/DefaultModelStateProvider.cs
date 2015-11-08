namespace BillPath
{
    internal sealed class DefaultModelStateProvider
        : IModelStateProvider
    {
        public ModelState GetForRoot(object model)
            => new ModelState(model);

        public ModelState GetForAggregate(object owner, object aggregate)
            => new ModelState(aggregate);
    }
}