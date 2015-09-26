using System;
using BillPath.UserInterface.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Tests
{
    [TestClass]
    public class DelegateCommandTests
    {
        [TestMethod]
        public void TestCannotCreateDelegateAsyncCommandWithNullCallback()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => new DelegateCommand(null));
        }

        [TestMethod]
        public void TestParameterIsPassedToCallback()
        {
            var parameterValue = new object();
            var asyncCommand = new DelegateCommand(
                parameter => Assert.AreSame(parameterValue, parameter));

            asyncCommand.Execute(parameterValue);
        }
        [TestMethod]
        public void TestParameterIsPassedToStronglyTypedCallback()
        {
            var parameterValue = string.Empty;
            var asyncCommand = new DelegateCommand<string>(
                parameter => Assert.AreSame(parameterValue, parameter));

            asyncCommand.Execute(parameterValue);
        }
    }
}