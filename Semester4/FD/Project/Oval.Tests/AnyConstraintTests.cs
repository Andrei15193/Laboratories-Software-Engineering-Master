using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Oval.Tests
{
    [TestClass]
    public class AnyConstraintTests
    {
        [TestMethod]
        public async Task TestCreateAnyConstraintReturnsRequirementThatMustFulfil()
        {
            var expectedRequirement = new Requirement("test requirement");
            var anyConstraint = AnyConstraint
                .From(Constraint.From((object value) => new[] { new Requirement("constraint") }))
                .Fulfils(expectedRequirement)
                .AsOneConstraint();

            var actualRequirements = await anyConstraint.CheckAsync(new object());

            Assert.AreSame(expectedRequirement, actualRequirements.Single());
        }
        [TestMethod]
        public async Task TestCreateStartingWithPredicate()
        {
            var expectedRequirement = new Requirement("test requirement");
            var anyConstraint = AnyConstraint
                .From((object value) => false)
                .Fulfils(expectedRequirement)
                .AsOneConstraint();

            var actualRequirements = await anyConstraint.CheckAsync(new object());

            Assert.AreSame(expectedRequirement, actualRequirements.Single());
        }
        [TestMethod]
        public async Task TestCreateStartingWithAsyncPredicate()
        {
            var expectedRequirement = new Requirement("test requirement");
            var anyConstraint = AnyConstraint
                .From((object value, CancellationToken cancellationToken) => Task.FromResult(false))
                .Fulfils(expectedRequirement)
                .AsOneConstraint();

            var actualRequirements = await anyConstraint.CheckAsync(new object());

            Assert.AreSame(expectedRequirement, actualRequirements.Single());
        }
        [TestMethod]
        public async Task TestCreateContinuingConstraint()
        {
            var expectedRequirement = new Requirement("test requirement");
            var anyConstraint = AnyConstraint
                .From((object value) => false)
                .Or(Constraint.From((object value) => new[] { new Requirement("test requirement") }))
                .Fulfils(expectedRequirement)
                .AsOneConstraint();

            var actualRequirements = await anyConstraint.CheckAsync(new object());

            Assert.AreSame(expectedRequirement, actualRequirements.Single());
        }
        [TestMethod]
        public async Task TestCreateContinuingWithPredicate()
        {
            var expectedRequirement = new Requirement("test requirement");
            var anyConstraint = AnyConstraint
                .From((object value) => false)
                .Or(value => false)
                .Fulfils(expectedRequirement)
                .AsOneConstraint();

            var actualRequirements = await anyConstraint.CheckAsync(new object());

            Assert.AreSame(expectedRequirement, actualRequirements.Single());
        }
        [TestMethod]
        public async Task TestCreateContinuingWithAsyncPredicate()
        {
            var expectedRequirement = new Requirement("test requirement");
            var anyConstraint = AnyConstraint
                .From((object value) => false)
                .Or((value, cancellationToken) => Task.FromResult(false))
                .Fulfils(expectedRequirement)
                .AsOneConstraint();

            var actualRequirements = await anyConstraint.CheckAsync(new object());

            Assert.AreSame(expectedRequirement, actualRequirements.Single());
        }
        [TestMethod]
        public async Task TestCreatingWithRequirementProvider()
        {
            var expectedRequirement = new Requirement("test requirement");
            var anyConstraint = AnyConstraint
                .From((object value) => false)
                .Fulfils(() => expectedRequirement)
                .AsOneConstraint();

            var actualRequirements = await anyConstraint.CheckAsync(new object());

            Assert.AreSame(expectedRequirement, actualRequirements.Single());
        }
        [TestMethod]
        public async Task TestCreatingWithRequirementsProvider()
        {
            var expectedRequirements =
                new[]
                {
                    new Requirement("test requirement 1"),
                    new Requirement("test requirement 2")
                };
            var anyConstraint = AnyConstraint
                .From((object value) => false)
                .Fulfils(() => expectedRequirements)
                .AsOneConstraint();

            var actualRequirements = await anyConstraint.CheckAsync(new object());

            Assert.IsTrue(expectedRequirements.SequenceEqual(actualRequirements));
        }
        [TestMethod]
        public async Task TestContinuingWithRequirement()
        {
            var requirement1 = new Requirement("test requirement 1");
            var requirement2 = new Requirement("test requirement 2");
            var anyConstraint = AnyConstraint
                .From((object value) => false)
                .Fulfils(requirement1)
                .And(requirement2)
                .AsOneConstraint();
            var expectedRequirements =
                new[]
                {
                    requirement1,
                    requirement2
                };

            var actualRequirements = await anyConstraint.CheckAsync(new object());

            Assert.IsTrue(expectedRequirements.SequenceEqual(actualRequirements));
        }
        [TestMethod]
        public async Task TestContinuingWithRequirementProvider()
        {
            var requirement1 = new Requirement("test requirement 1");
            var requirement2 = new Requirement("test requirement 2");
            var anyConstraint = AnyConstraint
                .From((object value) => false)
                .Fulfils(requirement1)
                .And(() => requirement2)
                .AsOneConstraint();
            var expectedRequirements =
                new[]
                {
                    requirement1,
                    requirement2
                };

            var actualRequirements = await anyConstraint.CheckAsync(new object());

            Assert.IsTrue(expectedRequirements.SequenceEqual(actualRequirements));
        }
        [TestMethod]
        public async Task TestContinuingWithRequirementsProvider()
        {
            var requirement1 = new Requirement("test requirement 1");
            var otherRequirements =
                new[]
                {
                    new Requirement("test requirement 2"),
                    new Requirement("test requirement 3")
                };
            var anyConstraint = AnyConstraint
                .From((object value) => false)
                .Fulfils(requirement1)
                .And(() => otherRequirements)
                .AsOneConstraint();
            var expectedRequirements =
                new[]
                {
                    requirement1
                }.Concat(otherRequirements);

            var actualRequirements = await anyConstraint.CheckAsync(new object());

            Assert.IsTrue(expectedRequirements.SequenceEqual(actualRequirements));
        }

        [TestMethod]
        public async Task TestAnyConstraintIsNotSatisfiedIfAllConstraintsAreNotSatisfied()
        {
            var expectedRequirement = new Requirement("test requirement");
            var anyConstraint = AnyConstraint
                .From(Constraint.From((object value) => new[] { new Requirement("constraint") }))
                .Or((object value) => false)
                .Or((object value, CancellationToken cancellationToken) => Task.FromResult(false))
                .Fulfils(expectedRequirement)
                .AsOneConstraint();

            var actualRequirements = await anyConstraint.CheckAsync(new object());

            Assert.AreSame(expectedRequirement, actualRequirements.Single());
        }
        [TestMethod]
        public async Task TestAnyConstraintIsSatisfiedIfAtLeastOneIsSatisfied()
        {
            var requirement = new Requirement("test requirement");
            var anyConstraint = AnyConstraint
                .From(Constraint.From((object value) => Enumerable.Empty<Requirement>()))
                .Or((object value) => false)
                .Or((object value, CancellationToken cancellationToken) => Task.FromResult(false))
                .Fulfils(requirement)
                .AsOneConstraint();

            var actualRequirements = await anyConstraint.CheckAsync(new object());

            Assert.IsFalse(actualRequirements.Any());
        }

        [TestMethod]
        public async Task RequirementsAreReturnedInTheSameOrderTheyAreProvided()
        {
            var requirement1 = new Requirement("test requirement 1");
            var requirement2 = new Requirement("test requirement 2");
            var requirement3 = new Requirement("test requirement 3");
            var requirement4 = new Requirement("test requirement 4");
            var requirement5 = new Requirement("test requirement 5");
            var requirement6 = new Requirement("test requirement 6");
            var requirement7 = new Requirement("test requirement 7");
            var requirement8 = new Requirement("test requirement 8");
            var requirement9 = new Requirement("test requirement 9");
            var constraint = AnyConstraint
                .From((object value) => false)
                .Fulfils(requirement1)
                .And(() => requirement2)
                .And(() => new[] { requirement3, requirement4 })
                .And(requirement5)
                .And(() => new[] { requirement6, requirement7 })
                .And(() => requirement8)
                .And(() => requirement9)
                .AsOneConstraint();
            var expectedRequirements =
                new[]
                {
                    requirement1,
                    requirement2,
                    requirement3,
                    requirement4,
                    requirement5,
                    requirement6,
                    requirement7,
                    requirement8,
                    requirement9
                };

            var actualRequirements = await constraint.CheckAsync(new object());

            Assert.IsTrue(expectedRequirements.SequenceEqual(actualRequirements));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCreateStartingWithNullConstraint()
        {
            AnyConstraint.From((IConstraint<object>)null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCreateStartingWithNullPredicate()
        {
            AnyConstraint.From((AnyConstraint.Predicate<object>)null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCreateStartingWithNullAsyncPredicate()
        {
            AnyConstraint.From((AnyConstraint.AsyncPredicate<object>)null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCreateContinuingWithNullConstraint()
        {
            AnyConstraint
                .From((object value) => false)
                .Or((IConstraint<object>)null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCreateContinuingWithNullPredicate()
        {
            AnyConstraint
                .From((object value) => false)
                .Or((AnyConstraint.Predicate<object>)null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCreateContinuingWithNullAsyncPredicate()
        {
            AnyConstraint
                .From((object value) => false)
                .Or((AnyConstraint.AsyncPredicate<object>)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCreatingWithNullRequirement()
        {
            AnyConstraint
                .From((object value) => false)
                .Fulfils((Requirement)null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCreatingWithNullRequirementProvider()
        {
            AnyConstraint
                .From((object value) => false)
                .Fulfils((AnyConstraint.RequirementProvider)null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCreatingWithNullRequirementsProvider()
        {
            AnyConstraint
                .From((object value) => false)
                .Fulfils((AnyConstraint.RequirementsProvider)null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestContinuingWithNullRequirement()
        {
            AnyConstraint
                .From((object value) => false)
                .Fulfils(new Requirement("test requirement"))
                .And((Requirement)null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestContinuingWithNullRequirementProvider()
        {
            AnyConstraint
                .From((object value) => false)
                .Fulfils(new Requirement("test requirement"))
                .And((AnyConstraint.RequirementProvider)null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestContinuingWithNullRequirementsProvider()
        {
            AnyConstraint
                .From((object value) => false)
                .Fulfils(new Requirement("test requirement"))
                .And((AnyConstraint.RequirementsProvider)null);
        }
    }
}