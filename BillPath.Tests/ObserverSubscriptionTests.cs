using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Tests
{
    [TestClass]
    public class ObserverSubscriptionTests
    {
        [TestMethod]
        public void TestSubscriptionCallsDisposeAction()
        {
            var invocationCount = 0;
            var subscription = new ObserverSubscription<int>(new DelegateObserver<int>(), observer => invocationCount++);

            subscription.Dispose();

            Assert.AreEqual(
                1,
                invocationCount);
        }

        [TestMethod]
        public void TestSubscriptionDisposeActionIsCalledWithProvidedObserver()
        {
            var expectedObserver = new DelegateObserver<int>();
            var subscription = new ObserverSubscription<int>(
                expectedObserver,
                actualObserver => Assert.AreSame(expectedObserver, actualObserver));

            subscription.Dispose();
        }

        [TestMethod]
        public void TestExceptionIsThrownWhenSubscriptionIsCalledWithNullObserver()
            => Assert.ThrowsException<ArgumentNullException>(() => new ObserverSubscription<int>(
                null,
                delegate { }));

        [TestMethod]
        public void TestExceptionIsThrownWhenDisposeActionIsNull()
            => Assert.ThrowsException<ArgumentNullException>(() => new ObserverSubscription<int>(
                new DelegateObserver<int>(),
                null));

        [TestMethod]
        public void TestSubscriptionCreatedWithExactSameParametersAreEqual()
        {
            var observer = new DelegateObserver<object>();
            Action<IObserver<object>> disposeAction = delegate { };

            var firstSubscription = new ObserverSubscription<object>(observer, disposeAction);
            var secondSubscription = new ObserverSubscription<object>(observer, disposeAction);

            Assert.IsTrue(firstSubscription == secondSubscription);
            Assert.IsFalse(firstSubscription != secondSubscription);
            Assert.IsTrue(firstSubscription.Equals(secondSubscription));
            Assert.AreEqual(firstSubscription.GetHashCode(), secondSubscription.GetHashCode());
        }
        [TestMethod]
        public void TestSubscriptionCreatedWithDifferentObserversAreNotEqual()
        {
            Action<IObserver<object>> disposeAction = delegate { };

            var firstSubscription = new ObserverSubscription<object>(new DelegateObserver<object>(), disposeAction);
            var secondSubscription = new ObserverSubscription<object>(new DelegateObserver<object>(), disposeAction);

            Assert.IsFalse(firstSubscription == secondSubscription);
            Assert.IsTrue(firstSubscription != secondSubscription);
            Assert.IsFalse(firstSubscription.Equals(secondSubscription));
        }
        [TestMethod]
        public void TestSubscriptionCreatedWithDifferentDisposeActionsAreNotEqual()
        {
            var observer = new DelegateObserver<object>();

            var firstSubscription = new ObserverSubscription<object>(observer, delegate { });
            var secondSubscription = new ObserverSubscription<object>(observer, delegate { });

            Assert.IsFalse(firstSubscription == secondSubscription);
            Assert.IsTrue(firstSubscription != secondSubscription);
            Assert.IsFalse(firstSubscription.Equals(secondSubscription));
        }
    }
}