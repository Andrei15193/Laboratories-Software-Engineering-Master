using System;
using System.Collections.Generic;

namespace BillPath
{
    public class Observable<T>
        : IObservable<T>
    {
        private readonly ICollection<IObserver<T>> _observers;
        private readonly Action<IObserver<T>> _unsubscribe;

        public Observable()
        {
            _observers = new HashSet<IObserver<T>>();
            _unsubscribe = observer => _observers.Remove(observer);
        }

        public ObserverSubscription<T> Subscribe(IObserver<T> observer)
        {
            _observers.Add(observer);
            return new ObserverSubscription<T>(observer, _unsubscribe);
        }
        IDisposable IObservable<T>.Subscribe(IObserver<T> observer)
            => Subscribe(observer);

        protected void Notify(T value)
        {
            foreach (var observer in _observers)
                observer.OnNext(value);
            foreach (var observer in _observers)
                observer.OnCompleted();
        }

        protected void NotifyError(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            foreach (var observer in _observers)
                observer.OnError(error);
            foreach (var observer in _observers)
                observer.OnCompleted();
        }
    }
}