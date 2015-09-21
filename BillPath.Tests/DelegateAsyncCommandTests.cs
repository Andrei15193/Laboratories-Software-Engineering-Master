using System;
using System.Threading;
using System.Threading.Tasks;
using BillPath.UserInterface.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BillPath.Tests
{
    [TestClass]
    public class DelegateAsyncCommandTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCannotCreateDelegateAsyncCommandWithNullCallback()
        {
            new DelegateAsyncCommand(null);
        }

        [TestMethod]
        public async Task TestParameterIsPassedToCallback()
        {
            var parameterValue = new object();
            var asyncCommand = new DelegateAsyncCommand(
                (parameter, cancellationToken) =>
                {
                    Assert.AreSame(parameterValue, parameter);
                    return Task.FromResult(default(object));
                });

            await asyncCommand.ExecuteAsync(parameterValue);
        }
        [TestMethod]
        public async Task TestParameterIsPassedToStronglyTypedCallback()
        {
            var parameterValue = string.Empty;
            var asyncCommand = new DelegateAsyncCommand<string>(
                (parameter, cancellationToken) =>
                {
                    Assert.AreSame(parameterValue, parameter);
                    return Task.FromResult(default(object));
                });

            await asyncCommand.ExecuteAsync(parameterValue);
        }

        [TestMethod]
        public async Task TestCacellationTokenIsSignaledWhenCallingCancelCommand()
        {
            using (var canceledEvent = new ManualResetEventSlim(false))
            using (var checkpointReachedEvent = new ManualResetEventSlim(false))
            {
                var asyncCommand = new DelegateAsyncCommand(
                    async (parameter, cancellationToken) =>
                    {
                        Assert.IsFalse(cancellationToken.IsCancellationRequested);

                        checkpointReachedEvent.Set();
                        await Task.Yield();
                        canceledEvent.Wait();

                        Assert.IsTrue(cancellationToken.IsCancellationRequested);
                    });

                var executeCommandTask = asyncCommand.ExecuteAsync(null);
                checkpointReachedEvent.Wait();

                asyncCommand.CancelCommand.Execute(null);
                canceledEvent.Set();

                await executeCommandTask;
            }
        }
    }
}