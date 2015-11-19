namespace BillPath
{
    public interface IModelStateProvider
    {
        ModelState GetForRoot(object model);
        ModelState GetForAggregate(ModelState owner, object aggregate);
    }
}