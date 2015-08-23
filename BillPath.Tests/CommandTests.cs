using System;
using System.Collections.Generic;
using System.ComponentModel;
using BillPath.UserInterface.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BillPath.Tests
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
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestCannotExecuteCommandIfCanExecuteIsFalse()
        {
            var command = new CannotExecuteCommand();
            command.Execute(null);
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
    }
}