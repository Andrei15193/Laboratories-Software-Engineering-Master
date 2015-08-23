using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using BillPath.UserInterface.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BillPath.Tests
{
    [TestClass]
    public class AsyncCommandTests
    {
        private class WaitForResumeEventAsyncCommand
            : AsyncCommand
        {
            private readonly ManualResetEventSlim _resumeEvent;

            public WaitForResumeEventAsyncCommand(ManualResetEventSlim resumeEvent)
            {
                if (resumeEvent == null)
                    throw new ArgumentNullException(nameof(resumeEvent));

                _resumeEvent = resumeEvent;
            }

            protected override async Task OnExecuteAsync(object parameter)
            {
                await Task.Yield();
                _resumeEvent.Wait(CancellationToken);
            }
        }
        private class AssertAreSameAsyncCommand
            : AsyncCommand
        {
            private readonly object _value;

            public AssertAreSameAsyncCommand(object value)
            {
                _value = value;
            }

            protected override Task OnExecuteAsync(object parameter)
            {
                Assert.AreSame(_value, parameter);
                return Task.FromResult(default(object));
            }
        }
        private class PropertyChangedLoggingAsyncCommand
            : AsyncCommand
        {
            private readonly List<string> _propertyChanges = new List<string>();

            public IReadOnlyList<string> PropertyChanges
            {
                get
                {
                    return _propertyChanges;
                }
            }

            protected override Task OnExecuteAsync(object parameter)
            {
                return Task.FromResult(default(object));
            }

            protected override void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
            {
                base.OnPropertyChanged(propertyChangedEventArgs);
                _propertyChanges.Add(propertyChangedEventArgs?.PropertyName);
            }
        }
        private class CannotExecuteCommand
            : AsyncCommand
        {
            public CannotExecuteCommand()
            {
                CanExecute = false;
            }

            protected override Task OnExecuteAsync(object parameter)
            {
                return Task.FromResult(default(object));
            }
        }

        [TestMethod]
        public async Task TestExecutingPropertyIsTrueDuringCommandExecution()
        {
            using (var resumeEvent = new ManualResetEventSlim(false))
            {
                var asyncCommand = new WaitForResumeEventAsyncCommand(resumeEvent);

                Assert.IsFalse(asyncCommand.Executing);

                var executeTask = asyncCommand.ExecuteAsync(null);

                Assert.IsTrue(asyncCommand.Executing);

                resumeEvent.Set();
                await executeTask;

                Assert.IsFalse(asyncCommand.Executing);
            }
        }
        [TestMethod]
        public async Task TestCanExecutePropertyIsFalseDuringCommandExecution()
        {
            using (var resumeEvent = new ManualResetEventSlim(false))
            {
                var asyncCommand = new WaitForResumeEventAsyncCommand(resumeEvent);

                Assert.IsTrue(asyncCommand.CanExecute);

                var executeTask = asyncCommand.ExecuteAsync(null);

                Assert.IsFalse(asyncCommand.CanExecute);

                resumeEvent.Set();
                await executeTask;

                Assert.IsTrue(asyncCommand.CanExecute);
            }
        }

        [TestMethod]
        public async Task TestParameterIsPassedToExecuteCommand()
        {
            var value = new object();
            var asyncCommand = new AssertAreSameAsyncCommand(value);

            await asyncCommand.ExecuteAsync(value);
        }

        [TestMethod]
        public async Task TestPropertyChangedEventIsRaisedWhenExecutingCommand()
        {
            var asyncCommand = new PropertyChangedLoggingAsyncCommand();
            await asyncCommand.ExecuteAsync(null);

            Assert.AreEqual(4, asyncCommand.PropertyChanges.Count);
            Assert.AreEqual(nameof(AsyncCommand.CanExecute), asyncCommand.PropertyChanges[0]);
            Assert.AreEqual(nameof(AsyncCommand.Executing), asyncCommand.PropertyChanges[1]);
            Assert.AreEqual(nameof(AsyncCommand.Executing), asyncCommand.PropertyChanges[2]);
            Assert.AreEqual(nameof(AsyncCommand.CanExecute), asyncCommand.PropertyChanges[3]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task TestTryExecuteCommandWhileCanExecuteIsFalse()
        {
            await new CannotExecuteCommand().ExecuteAsync(null);
        }

        [TestMethod]
        [ExpectedException(typeof(OperationCanceledException))]
        public async Task TestCancelCommandSetsCancelingToTrue()
        {
            using (var resumeEvent = new ManualResetEventSlim(false))
            {
                var asyncCommand = new WaitForResumeEventAsyncCommand(resumeEvent);

                var executeTask = asyncCommand.ExecuteAsync(null);

                Assert.IsFalse(asyncCommand.Canceling);
                asyncCommand.CancelCommand.Execute(null);
                Assert.IsTrue(asyncCommand.Canceling);

                await executeTask;
            }
        }

        [TestMethod]
        public void TestCannotCallCancelIfAsyncCommandIsNotExecuting()
        {
            var asyncCommand = new PropertyChangedLoggingAsyncCommand();
            Assert.IsFalse(asyncCommand.CancelCommand.CanExecute);
        }
        [TestMethod]
        [ExpectedException(typeof(OperationCanceledException))]
        public async Task TestCannotCallCancelTwiceOnSameCommandExecution()
        {
            using (var resumeEvent = new ManualResetEventSlim(false))
            {
                var asyncCommand = new WaitForResumeEventAsyncCommand(resumeEvent);

                var executeTask = asyncCommand.ExecuteAsync(null);

                Assert.IsTrue(asyncCommand.CancelCommand.CanExecute);
                asyncCommand.CancelCommand.Execute(null);
                Assert.IsFalse(asyncCommand.CancelCommand.CanExecute);

                await executeTask;
            }
        }
        [TestMethod]
        public async Task CannotCallCancelAfterAsyncCommandExecuted()
        {
            using (var resumeEvent = new ManualResetEventSlim(false))
            {
                var asyncCommand = new WaitForResumeEventAsyncCommand(resumeEvent);
                var executeTask = asyncCommand.ExecuteAsync(null);

                resumeEvent.Set();
                await executeTask;

                Assert.IsFalse(asyncCommand.CancelCommand.CanExecute);
            }
        }
    }
}