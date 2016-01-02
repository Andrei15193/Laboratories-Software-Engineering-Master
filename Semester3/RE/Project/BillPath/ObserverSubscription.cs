using System;

namespace BillPath
{
    public class ObserverSubscription<T>
        : IDisposable, IEquatable<ObserverSubscription<T>>
    {
        private readonly IObserver<T> _observer;
        private readonly Action<IObserver<T>> _disposeAction;

        public ObserverSubscription(IObserver<T> observer, Action<IObserver<T>> disposeAction)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (disposeAction == null)
                throw new ArgumentNullException(nameof(disposeAction));

            _observer = observer;
            _disposeAction = disposeAction;
        }

        public void Dispose()
            => _disposeAction(_observer);

        public bool Equals(ObserverSubscription<T> other)
            => other != null
            && _observer == other._observer
            && _disposeAction == other._disposeAction;
        public override bool Equals(object obj)
            => Equals(obj as ObserverSubscription<T>);
        public override int GetHashCode()
            => _observer.GetHashCode()
            ^ _disposeAction.GetHashCode();

        public static bool operator ==(ObserverSubscription<T> left, ObserverSubscription<T> right)
            => Equals(left, right);
        public static bool operator !=(ObserverSubscription<T> left, ObserverSubscription<T> right)
            => !Equals(left, right);
    }
}