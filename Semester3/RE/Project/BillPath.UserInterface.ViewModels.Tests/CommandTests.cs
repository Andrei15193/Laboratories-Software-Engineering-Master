using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.UserInterface.ViewModels.Tests
{
    [TestClass]
    public class CommandTests
    {
        private class CannotExecuteCommand
            : Command
        {
            public CannotExecuteCommand()
            {
                CanExecute = false;
            }

            protected override void OnExecute(object parameter)
            {
            }
        }
        private class AssertAreSameCommand
            : Command
        {
            private readonly object _value;

            public AssertAreSameCommand(object value)
            {
                _value = value;
            }

            protected override void OnExecute(object parameter)
            {
                Assert.AreSame(_value, parameter);
            }
        }
        private class DisableCommand
            : Command
        {
            private readonly List<string> _propertyChanges = new List<string>();

            public IReadOnlyList<string> PropertyChanges
            {
                get
                {
                    return _propertyChanges;
                }
            }

            protected override void OnExecute(object parameter)
            {
                CanExecute = false;
            }

            protected override void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
            {
                base.OnPropertyChanged(propertyChangedEventArgs);
                _propertyChanges.Add(propertyChangedEventArgs?.PropertyName);
            }
        }

        [TestMethod]
        public void TestCannotExecuteCommandIfCanExecuteIsFalse()
        {
            Assert.ThrowsException<InvalidOperationException>(
                () => new CannotExecuteCommand().Execute(null));
        }

        [TestMethod]
        public void TestParameterIsPassedToExecuteMethod()
        {
            var value = new object();
            var command = new AssertAreSameCommand(value);
            command.Execute(value);
        }

        [TestMethod]
        public void TestPropertyChangedIsRaisedWhenCanExecuteChanges()
        {
            var command = new DisableCommand();
            command.Execute(null);

            Assert.AreEqual(1, command.PropertyChanges.Count);
            Assert.AreEqual(nameof(Command.CanExecute), command.PropertyChanges[0]);
        }

        [TestMethod]
        public void TestStronglyTypedParameterAsyncCommandConvertsStringOfNumericValueToIntImplicitly()
        {
            var value = 10;
            var command = new AssertEqualsIntCommand(value);

            command.Execute(value.ToString());
        }
        [TestMethod]
        public void TestStronglyTypedParameterAsyncCommandWorksWithParameterOfSameType()
        {
            var value = 10;
            var command = new AssertEqualsIntCommand(value);

            command.Execute(value);
        }
        [TestMethod]
        public void TestStronglyTypedIntParameterAsyncCommandFailsWhenTryingtoCastNonNumericValue()
        {
            Assert.ThrowsException<FormatException>(
                () => new AssertEqualsIntCommand(10).Execute("abc"));
        }
        [TestMethod]
        public void TestStronglyTypedAsyncComandFailsWhenParameterTypeIsNotConvertibleAndMismatches()
        {
            Assert.ThrowsException<InvalidCastException>(
                () => new NonConvertibleParameterCommand().Execute(string.Empty));
        }
        private sealed class AssertEqualsIntCommand
            : Command<int>
        {
            private readonly int _valueToCompare;

            public AssertEqualsIntCommand(int valueToCompare)
            {
                _valueToCompare = valueToCompare;
            }

            protected override void OnExecute(int parameter)
            {
                Assert.AreEqual(_valueToCompare, parameter);
            }
        }
        private sealed class NonConvertibleParameterCommand
            : Command<NonConvertibleParameterCommand.ParameterType>
        {
            public class ParameterType
            {
            }

            protected override void OnExecute(ParameterType parameter)
            {
            }
        }
    }
}