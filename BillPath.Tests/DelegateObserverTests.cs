using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Tests
{
    [TestClass]
    public class DelegateObserverTests
    {
        [TestMethod]
        public void TestProvidedMethodForOnNextNotificationIsInvoked()
        {
            var invocationCount = 0;
            var observer = new DelegateObserver<object>(onNext: delegate { invocationCount++; });

            observer.OnNext(null);

            Assert.AreEqual(1, invocationCount);
        }
        [TestMethod]
        public void TestOnNextNotificationDelegateCanBeNull()
            => new DelegateObserver<object>(onNext: null);

        [TestMethod]
        public void TestProvidedMethodForOnErrorNotificationIsInvoked()
        {
            var invocationCount = 0;
            var observer = new DelegateObserver<object>(onError: delegate { invocationCount++; });

            observer.OnError(null);

            Assert.AreEqual(1, invocationCount);
        }
        [TestMethod]
        public void TestOnErrorNotificationDelegateCanBeNull()
            => new DelegateObserver<object>(onError: null);

        [TestMethod]
        public void TestProvidedMethodForOnCompletedNotificationIsInvoked()
        {
            var invocationCount = 0;
            var observer = new DelegateObserver<object>(onCompleted: delegate { invocationCount++; });

            observer.OnCompleted();

            Assert.AreEqual(1, invocationCount);
        }
        [TestMethod]
        public void TestOnCompletedNotificationDelegateCanBeNull()
            => new DelegateObserver<object>(onCompleted: null);
    }
}