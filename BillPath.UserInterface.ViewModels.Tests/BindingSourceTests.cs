using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.UserInterface.ViewModels.Tests
{
    [TestClass]
    public class BindingSourceTests
    {
        private sealed class BindingSourceMock
            : BindingSource
        {
            new public void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
                => base.OnPropertyChanged(propertyChangedEventArgs);

            new public void OnPropertyChanged([CallerMemberName] string propertyName = null)
                => base.OnPropertyChanged(propertyName);
        }

        private BindingSourceMock _bindingSource;

        [TestInitialize]
        public void TestInitialize()
        {
            _bindingSource = new BindingSourceMock();
        }

        [TestMethod]
        public void TestNotificationIsSentWhenRaisingPropertyChanged()
        {
            var raiseCount = 0;

            _bindingSource.PropertyChanged += (sender, e) => raiseCount++;
            _bindingSource.OnPropertyChanged(new PropertyChangedEventArgs(null));

            Assert.AreEqual(1, raiseCount);
        }

        [TestMethod]
        public void TestNotificationIsSentWithBindingSourceAsSource()
        {
            object actualSender = null;
            _bindingSource.PropertyChanged += (sender, e) => actualSender = sender;

            _bindingSource.OnPropertyChanged(new PropertyChangedEventArgs(null));

            Assert.AreSame(_bindingSource, actualSender);
        }

        [TestMethod]
        public void TestNotificationIsSentWithTheProvidedEventArgs()
        {
            var expectedEventArgs = new PropertyChangedEventArgs(null);
            PropertyChangedEventArgs actualEventArgs = null;
            _bindingSource.PropertyChanged += (sender, e) => actualEventArgs = e;

            _bindingSource.OnPropertyChanged(expectedEventArgs);

            Assert.AreSame(expectedEventArgs, actualEventArgs);
        }

        [TestMethod]
        public void TestHelperMethodRaisesPropertyChangedWithProvidedPropertyName()
        {
            var expectedPropertyName = nameof(TestHelperMethodRaisesPropertyChangedWithProvidedPropertyName);
            string actualPropertyName = null;

            _bindingSource.PropertyChanged += (sender, e) => actualPropertyName = e.PropertyName;

            _bindingSource.OnPropertyChanged();

            Assert.AreEqual(expectedPropertyName, actualPropertyName, ignoreCase: false);
        }

        [TestMethod]
        public void TestExceptionIsThrownWhenEventArgsIsNull()
            => Assert.ThrowsException<ArgumentNullException>(() => _bindingSource.OnPropertyChanged((PropertyChangedEventArgs)null));
    }
}