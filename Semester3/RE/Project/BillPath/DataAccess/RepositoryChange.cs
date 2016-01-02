using System;

namespace BillPath.DataAccess
{
    public class RepositoryChange<TItem>
    {
        public RepositoryChange(TItem item, RepositoryChangeAction action)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            Item = item;
            Action = action;
        }

        public TItem Item
        {
            get;
        }
        public RepositoryChangeAction Action
        {
            get;
        }
    }
}