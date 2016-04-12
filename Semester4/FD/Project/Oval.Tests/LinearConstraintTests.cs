using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Oval.Tests
{
    [TestClass]
    public class LinearConstraintTests
    {
        [TestMethod]
        public async Task TestLinearConstraintStartingWithAnUnsatisfiedConstraintAndEndedInASatisfiedOne()
        {
            var expectedRequirement = new Requirement("test requirement");
            var constraint = LinearConstraint
                .StartingWith(Constraint.From((object value) => new[] { expectedRequirement }))
                .AndEndedWith(Constraint.From((object value) => new Requirement[0]));

            var actualRequirements = await constraint.CheckAsync(new object());

            Assert.AreSame(expectedRequirement, actualRequirements.Single());
        }
        [TestMethod]
        public async Task TestLinearConstraintStartingAndEndedWithAnUnsatisfiedConstraint()
        {
            var requirement1 = new Requirement("test requirement 1");
            var requirement2 = new Requirement("test requirement 2");
            var constraint = LinearConstraint
                .StartingWith(Constraint.From((object value) => new[] { requirement1 }))
                .AndEndedWith(Constraint.From((object value) => new[] { requirement2 }));
            var expectedRequirements =
                new[]
                {
                    requirement1,
                    requirement2
                };

            var actualRequirements = await constraint.CheckAsync(new object());

            Assert.IsTrue(expectedRequirements.SequenceEqual(actualRequirements));
        }
        [TestMethod]
        public async Task TestLinearConstraintReturnsRequirementsForAllUnsatisfiedConstraintsInABatch()
        {
            var requirement1 = new Requirement("test requirement 1");
            var requirement2 = new Requirement("test requirement 2");
            var requirement3 = new Requirement("test requirement 3");
            var constraint = LinearConstraint
                .StartingWith(Constraint.From((object value) => new[] { requirement1 }))
                .FollowedBy(Constraint.From((object value) => new[] { requirement2 }))
                .AndEndedWith(Constraint.From((object value) => new[] { requirement3 }));
            var expectedRequirements =
                new[]
                {
                    requirement1,
                    requirement2,
                    requirement3
                };

            var actualRequirements = await constraint.CheckAsync(new object());

            Assert.IsTrue(expectedRequirements.SequenceEqual(actualRequirements));
        }
        [TestMethod]
        public async Task TestLinearConstraintReturnsRequirementsOnlyUntilFirstCheckWhenThereAreAny()
        {
            var requirement1 = new Requirement("test requirement 1");
            var requirement2 = new Requirement("test requirement 2");
            var requirement3 = new Requirement("test requirement 3");
            var constraint = LinearConstraint
                .StartingWith(Constraint.From((object value) => new[] { requirement1 }))
                .FollowedBy(Constraint.From((object value) => new[] { requirement2 }))
                .CheckedAndEndedWith(Constraint.From((object value) => new[] { requirement3 }));
            var expectedRequirements =
                new[]
                {
                    requirement1,
                    requirement2
                };

            var actualRequirements = await constraint.CheckAsync(new object());

            Assert.IsTrue(expectedRequirements.SequenceEqual(actualRequirements));
        }
        [TestMethod]
        public async Task TestLinearConstraintReturnsRequirementsFromSecondBatchWhenTheFirstOneHasNone()
        {
            var requirement1 = new Requirement("test requirement 1");
            var requirement2 = new Requirement("test requirement 2");
            var constraint = LinearConstraint
                .StartingWith(Constraint.From((object value) => new Requirement[] { }))
                .FollowedBy(Constraint.From((object value) => new Requirement[] { }))
                .CheckedAndFollowedBy(Constraint.From((object value) => new[] { requirement1 }))
                .AndEndedWith(Constraint.From((object value) => new[] { requirement2 }));
            var expectedRequirements =
                new[]
                {
                    requirement1,
                    requirement2
                };

            var actualRequirements = await constraint.CheckAsync(new object());

            Assert.IsTrue(expectedRequirements.SequenceEqual(actualRequirements));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestLinearConstraintStartingWithNullConstraint()
        {
            LinearConstraint.StartingWith((IConstraint<object>)null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestLinearConstraintStartingWithNullCallback()
        {
            LinearConstraint.StartingWith((Constraint.Callback<object>)null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestLinearConstraintStartingWithNullAsyncCallback()
        {
            LinearConstraint.StartingWith((Constraint.AsyncCallback<object>)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestLinearConstraintFollowedByNullConstraint()
        {
            LinearConstraint
                .StartingWith(Constraint.From((object value) => Enumerable.Empty<Requirement>()))
                .FollowedBy((IConstraint<object>)null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestLinearConstraintFollowedByNullCallback()
        {
            LinearConstraint
                .StartingWith((object value) => Enumerable.Empty<Requirement>())
                .FollowedBy((Constraint.Callback<object>)null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestLinearConstraintFollowedByNullAsyncCallback()
        {
            LinearConstraint
                .StartingWith((object value) => Enumerable.Empty<Requirement>())
                .FollowedBy((Constraint.AsyncCallback<object>)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestLinearConstraintCheckedAndFollowedByNullConstraint()
        {
            LinearConstraint
                .StartingWith(Constraint.From((object value) => Enumerable.Empty<Requirement>()))
                .CheckedAndFollowedBy((IConstraint<object>)null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestLinearConstraintCheckedAndFollowedByNullCallback()
        {
            LinearConstraint
                .StartingWith(Constraint.From((object value) => Enumerable.Empty<Requirement>()))
                .CheckedAndFollowedBy((Constraint.Callback<object>)null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestLinearConstraintCheckedAndFollowedByNullAsyncCallback()
        {
            LinearConstraint
                .StartingWith(Constraint.From((object value) => Enumerable.Empty<Requirement>()))
                .CheckedAndFollowedBy((Constraint.AsyncCallback<object>)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestLinearConstraintEndedWithNullConstraint()
        {
            LinearConstraint
                .StartingWith(Constraint.From((object value) => Enumerable.Empty<Requirement>()))
                .CheckedAndEndedWith((IConstraint<object>)null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestLinearConstraintEndedWithNullCallback()
        {
            LinearConstraint
                .StartingWith(Constraint.From((object value) => Enumerable.Empty<Requirement>()))
                .CheckedAndEndedWith((Constraint.Callback<object>)null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestLinearConstraintEndedWithNullAsyncCallback()
        {
            LinearConstraint
                .StartingWith(Constraint.From((object value) => Enumerable.Empty<Requirement>()))
                .CheckedAndEndedWith((Constraint.AsyncCallback<object>)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestLinearConstraintCheckedAndEndedWithNullConstraint()
        {
            LinearConstraint
                .StartingWith(Constraint.From((object value) => Enumerable.Empty<Requirement>()))
                .CheckedAndEndedWith((IConstraint<object>)null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestLinearConstraintCheckedAndEndedWithNullCallback()
        {
            LinearConstraint
                .StartingWith(Constraint.From((object value) => Enumerable.Empty<Requirement>()))
                .CheckedAndEndedWith((Constraint.Callback<object>)null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestLinearConstraintCheckedAndEndedWithNullAsyncCallback()
        {
            LinearConstraint
                .StartingWith(Constraint.From((object value) => Enumerable.Empty<Requirement>()))
                .CheckedAndEndedWith((Constraint.AsyncCallback<object>)null);
        }
    }
}