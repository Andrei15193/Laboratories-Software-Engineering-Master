using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Oval.Tests
{
    [TestClass]
    public class ConstraintTests
    {
        [TestMethod]
        public async Task TestCreateInlineConstraint()
        {
            var callCount = 0;
            var constraint = Constraint.From<object>(
                value =>
                {
                    callCount++;
                    return Enumerable.Empty<Requirement>();
                });

            await constraint.CheckAsync(new object());

            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCreateInlineConstraintWithNull()
        {
            Constraint.From((Constraint.Callback<object>)null);
        }

        [TestMethod]
        public async Task TestCreateAsyncConstraint()
        {
            var callCount = 0;
            var constraint = Constraint.From<object>(
                (value, cancellationToken) =>
                {
                    callCount++;
                    return Task.FromResult(Enumerable.Empty<Requirement>());
                });

            await constraint.CheckAsync(new object());

            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCreateAsyncConstraintWithNull()
        {
            Constraint.From((Constraint.AsyncCallback<object>)null);
        }
    }
}