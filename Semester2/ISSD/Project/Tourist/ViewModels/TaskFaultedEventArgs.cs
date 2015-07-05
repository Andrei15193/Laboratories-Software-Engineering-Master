using System;
using System.Collections.Generic;
namespace Tourist.ViewModels
{
    public class TaskFaultedEventArgs
        : EventArgs
    {
        private readonly AggregateException _aggregateException;

        public TaskFaultedEventArgs(AggregateException aggregateException)
        {
            if (aggregateException == null)
                throw new ArgumentNullException("aggregateException");

            _aggregateException = aggregateException;
        }

        public Exception Exception
        {
            get
            {
                return _aggregateException.InnerException;
            }
        }
        public IEnumerable<Exception> Exceptions
        {
            get
            {
                return _aggregateException.Flatten().InnerExceptions;
            }
        }
        public AggregateException AggregateException
        {
            get
            {
                return _aggregateException;
            }
        }
    }
}