using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Tests
{
    [TestClass]
    public class ObservableTests
    {
        private sealed class ObservableMock<T>
            : Observable<T>
        {
            new public void Notify(T value)
            {
                base.Notify(value);
            }

            new public void NotifyError(Exception error)
            {
                base.NotifyError(error);
            }
        }

        [TestMethod]
        public void TestSubscribingToAnObservableDoesNotReturnNull()
        {
            var observable = new Observable<int>();

            var subscription = observable.Subscribe(new DelegateObserver<int>());

            Assert.IsNotNull(subscription);
        }

        [TestMethod]
        public void TestSubscribedObserverReceivesOnNextNotification()
        {
            var invocationCount = 0;
            var observable = new ObservableMock<int>();
            observable.Subscribe(new DelegateObserver<int>(onNext: delegate { invocationCount++; }));

            observable.Notify(0);

            Assert.AreEqual(1, invocationCount);
        }

        [TestMethod]
        public void TestSubscribedObserverReceivesOnCompletedNotification()
        {
            var invocationCount = 0;
            var observable = new ObservableMock<int>();
            observable.Subscribe(new DelegateObserver<int>(onCompleted: delegate { invocationCount++; }));

            observable.Notify(0);

            Assert.AreEqual(1, invocationCount);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(5)]
        [DataRow(8)]
        [DataRow(13)]
        [DataRow(21)]
        public void TestSubscribedObserversFirstReceiveOnNextNotificationsBeforeOnCompleted(int observersCount)
        {
            var onNextInvocationCount = 0;
            var onCompletedInvocationCount = 0;
            var observable = new ObservableMock<int>();

            for (int observerIndex = 0; observerIndex < observersCount; observerIndex++)
                observable.Subscribe(new DelegateObserver<int>(
                    onNext: delegate
                    {
                        Assert.AreEqual(0, onCompletedInvocationCount);
                        onNextInvocationCount++;
                    },
                    onCompleted: delegate
                    {
                        Assert.AreEqual(observersCount, onNextInvocationCount);
                        onCompletedInvocationCount++;
                    }));

            observable.Notify(0);
        }

        [TestMethod]
        public void TestSubscribedObserverReceivesOnErrorNotification()
        {
            var invocationCount = 0;
            var observable = new ObservableMock<int>();
            observable.Subscribe(new DelegateObserver<int>(onError: delegate { invocationCount++; }));

            observable.NotifyError(new Exception());

            Assert.AreEqual(1, invocationCount);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(5)]
        [DataRow(8)]
        [DataRow(13)]
        [DataRow(21)]
        public void TestSubscribedObserversFirstReceiveOnErrorNotificationsBeforeOnCompleted(int observersCount)
        {
            var onErrorInvocationCount = 0;
            var onCompletedInvocationCount = 0;
            var observable = new ObservableMock<int>();

            for (int observerIndex = 0; observerIndex < observersCount; observerIndex++)
                observable.Subscribe(new DelegateObserver<int>(
                    onError: delegate
                    {
                        Assert.AreEqual(0, onCompletedInvocationCount);
                        onErrorInvocationCount++;
                    },
                    onCompleted: delegate
                    {
                        Assert.AreEqual(observersCount, onErrorInvocationCount);
                        onCompletedInvocationCount++;
                    }));

            observable.NotifyError(new Exception());
        }

        [TestMethod]
        public void TestExceptionIsThrownWhenTryingToNotifyNullError()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new ObservableMock<object>().NotifyError(null));
        }

        [TestMethod]
        public void TestExceptionIsThrownWhenTryingToSubscribeNullObserver()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new Observable<object>().Subscribe(null));
        }

        [TestMethod]
        public void TestSubscribingSameObserverMultipleTimesWillReceiveOnlyOneNotification()
        {
            var onNextInvocationCount = 0;
            var onErrorInvocationCount = 0;
            var onCompletedInvocationCount = 0;
            var observable = new ObservableMock<int>();
            var observer = new DelegateObserver<int>(
                onNext: delegate { onNextInvocationCount++; },
                onError: delegate { onErrorInvocationCount++; },
                onCompleted: delegate { onCompletedInvocationCount++; });
            observable.Subscribe(observer);
            observable.Subscribe(observer);

            observable.Notify(0);
            observable.NotifyError(new Exception());

            Assert.AreEqual(1, onNextInvocationCount);
            Assert.AreEqual(1, onErrorInvocationCount);
            Assert.AreEqual(2, onCompletedInvocationCount);
        }

        [TestMethod]
        public void TestSubscribingSameOverserverMultipleTimesReturnsEqualSubscriptions()
        {
            var observable = new Observable<int>();
            var observer = new DelegateObserver<int>();

            var firstSubscription = observable.Subscribe(observer);
            var secondSubscription = observable.Subscribe(observer);

            Assert.AreEqual(firstSubscription, secondSubscription);
        }

        [TestMethod]
        public void TestObserverWillNoLongerReceiveNotificationsAfterCallingDisposeOnSubscription()
        {
            var onNextInvocationCount = 0;
            var onErrorInvocationCount = 0;
            var onCompletedInvocationCount = 0;
            var observable = new ObservableMock<int>();
            var observer = new DelegateObserver<int>(
                onNext: delegate { onNextInvocationCount++; },
                onError: delegate { onErrorInvocationCount++; },
                onCompleted: delegate { onCompletedInvocationCount++; });
            var subscription = observable.Subscribe(observer);

            subscription.Dispose();

            observable.Notify(0);
            observable.NotifyError(new Exception());

            Assert.AreEqual(0, onNextInvocationCount);
            Assert.AreEqual(0, onErrorInvocationCount);
            Assert.AreEqual(0, onCompletedInvocationCount);
        }

        [TestMethod]
        public void TestObserverWillNoLongerReceiveNotificationAfterCallingDisposeOnSubscriptionOnceRegardlessOfTheirNumber()
        {
            var onNextInvocationCount = 0;
            var onErrorInvocationCount = 0;
            var onCompletedInvocationCount = 0;
            var observable = new ObservableMock<int>();
            var observer = new DelegateObserver<int>(
                onNext: delegate { onNextInvocationCount++; },
                onError: delegate { onErrorInvocationCount++; },
                onCompleted: delegate { onCompletedInvocationCount++; });
            var subscription = observable.Subscribe(observer);
            observable.Subscribe(observer);

            subscription.Dispose();

            observable.Notify(0);
            observable.NotifyError(new Exception());

            Assert.AreEqual(0, onNextInvocationCount);
            Assert.AreEqual(0, onErrorInvocationCount);
            Assert.AreEqual(0, onCompletedInvocationCount);
        }

        [TestMethod]
        public void TestNotifyingObserverPassesSameParameterToObserver()
        {
            var expectedValue = new object();
            var observable = new ObservableMock<object>();
            var observer = new DelegateObserver<object>(
                onNext: actualValue => Assert.AreSame(expectedValue, actualValue));
            observable.Subscribe(observer);

            observable.Notify(expectedValue);
        }

        [TestMethod]
        public void TestNotifyingObserverPassesSameErrorToObserver()
        {
            var expectedError = new Exception();
            var observable = new ObservableMock<object>();
            var observer = new DelegateObserver<object>(
                onError: actualError => Assert.AreSame(expectedError, actualError));
            observable.Subscribe(observer);

            observable.NotifyError(expectedError);
        }
    }
}