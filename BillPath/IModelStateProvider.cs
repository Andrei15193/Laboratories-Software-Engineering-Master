namespace BillPath
{
    public interface IModelStateProvider
    {
        ModelState GetForRoot(object model);
        ModelState GetForAggregate(object owner, object aggregate);
    }
}