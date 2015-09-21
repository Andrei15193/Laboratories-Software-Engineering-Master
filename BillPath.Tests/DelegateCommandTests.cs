using System;
using BillPath.UserInterface.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BillPath.Tests
{
    [TestClass]
    public class DelegateCommandTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCannotCreateDelegateAsyncCommandWithNullCallback()
        {
            new DelegateCommand(null);
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