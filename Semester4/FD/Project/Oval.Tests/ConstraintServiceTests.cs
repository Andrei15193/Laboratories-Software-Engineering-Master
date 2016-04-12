using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Oval.Tests
{
    [TestClass]
    public class ConstraintServiceTests
    {
        private readonly ICollection<string> _constraintNamesToDeregister
            = new HashSet<string>(StringComparer.Ordinal);

        [TestInitialize]
        public void TestInitialize()
        {
            _constraintNamesToDeregister.Clear();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ConstraintService.DeregisterFor<object>();
            foreach (var constraintNameToDeregister in _constraintNamesToDeregister)
                ConstraintService.DeregisterFor<object>(constraintNameToDeregister);
        }

        [TestMethod]
        public void TestRegisteringAConstraintWillMakeItRetrievable()
        {
            var expectedConstraint = Constraint.From<object>(value => Enumerable.Empty<Requirement>());
            ConstraintService.RegisterFor(expectedConstraint);

            var actualConstraint = ConstraintService.GetFor<object>();

            Assert.AreSame(expectedConstraint, actualConstraint);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestTryingToRegisterAConstraintForTheSameTypeWithoutNameThrowsException()
        {
            var expectedConstraint = Constraint.From<object>(value => Enumerable.Empty<Requirement>());
            ConstraintService.RegisterFor(expectedConstraint);
            ConstraintService.RegisterFor(expectedConstraint);
        }

        [TestMethod]
        public void TestRegisteringAConstraintWithANameWillMakeItRetrievableByThatName()
        {
            var constraintName = "test";
            _constraintNamesToDeregister.Add(constraintName);

            var expectedConstraint = Constraint.From<object>(value => Enumerable.Empty<Requirement>());
            ConstraintService.RegisterFor(expectedConstraint, constraintName);

            var actualConstraint = ConstraintService.GetFor<object>(constraintName);

            Assert.AreSame(expectedConstraint, actualConstraint);
        }
        [TestMethod]
        public void TestRegisteringTwoConstraintsWithDifferentNamesForTheSameTypeMakeThemRetrievableAccordingly()
        {
            var constraintName1 = "test 1";
            _constraintNamesToDeregister.Add(constraintName1);

            var expectedConstraint1 = Constraint.From<object>(value => Enumerable.Empty<Requirement>());
            ConstraintService.RegisterFor(expectedConstraint1, constraintName1);

            var constraintName2 = "test 2";
            _constraintNamesToDeregister.Add(constraintName2);

            var expectedConstraint2 = Constraint.From<object>(value => Enumerable.Empty<Requirement>());
            ConstraintService.RegisterFor(expectedConstraint2, constraintName2);

            var actualConstraint1 = ConstraintService.GetFor<object>(constraintName1);
            var actualConstraint2 = ConstraintService.GetFor<object>(constraintName2);

            Assert.AreNotSame(expectedConstraint1, expectedConstraint2);
            Assert.AreSame(expectedConstraint1, actualConstraint1);
            Assert.AreSame(expectedConstraint2, actualConstraint2);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestTryingToRegisterAConstraintForTheSameTypeAndNameThrowsException()
        {
            var constraintName = "test";
            _constraintNamesToDeregister.Add(constraintName);

            var expectedConstraint = Constraint.From<object>(value => Enumerable.Empty<Requirement>());
            ConstraintService.RegisterFor(expectedConstraint, constraintName);
            ConstraintService.RegisterFor(expectedConstraint, constraintName);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNamesAreTrimmedWhenRegisteringConstraints()
        {
            var constraintName = "test";
            _constraintNamesToDeregister.Add(constraintName);

            var expectedConstraint = Constraint.From<object>(value => Enumerable.Empty<Requirement>());
            ConstraintService.RegisterFor(expectedConstraint, constraintName);
            ConstraintService.RegisterFor(expectedConstraint, constraintName + " ");
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void TestGettingAConstraintForATypeThatDoesNotExistsThrowsException()
        {
            ConstraintService.GetFor<object>();
        }
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void TestGettingAConstraintForATypeAndNameThatDoesNotExistsThrowsException()
        {
            var constraint = Constraint.From<object>(value => Enumerable.Empty<Requirement>());
            ConstraintService.RegisterFor(constraint);

            ConstraintService.GetFor<object>("test");
        }

        [TestMethod]
        public void TestTryingToGetAConstraintForATypeThatDoesNotExistsReturnsNull()
        {
            var expectedConstraint = ConstraintService.TryGetFor<object>();

            Assert.IsNull(expectedConstraint);
        }
        [TestMethod]
        public void TestTryingToGetAConstraintForATypeAndNameThatDoesNotExistsThrowsException()
        {
            var constraint = Constraint.From<object>(value => Enumerable.Empty<Requirement>());
            ConstraintService.RegisterFor(constraint);

            var expectedConstraint = ConstraintService.TryGetFor<object>("test");

            Assert.IsNull(expectedConstraint);
        }

        [TestMethod]
        public void TestDeregisteringAConstraintMakesItUnavailable()
        {
            var expectedConstraint = Constraint.From<object>(value => Enumerable.Empty<Requirement>());

            ConstraintService.RegisterFor(expectedConstraint);
            ConstraintService.DeregisterFor<object>();

            var actualConstraint = ConstraintService.TryGetFor<object>();

            Assert.IsNull(actualConstraint);
        }
        [TestMethod]
        public void TestDeregisteringAConstraintWithANameMakesItUnavailable()
        {
            var constraintName = "test";
            _constraintNamesToDeregister.Add(constraintName);
            var expectedConstraint = Constraint.From<object>(value => Enumerable.Empty<Requirement>());

            ConstraintService.RegisterFor(expectedConstraint, constraintName);
            ConstraintService.DeregisterFor<object>(constraintName);

            var actualConstraint = ConstraintService.TryGetFor<object>(constraintName);

            Assert.IsNull(actualConstraint);
        }
        [TestMethod]
        public void TestDeregisteringAConstraintAllowsADifferentOneToBeRegistered()
        {
            var deregisteredConstraint = Constraint.From<object>(value => Enumerable.Empty<Requirement>());
            var expectedConstraint = Constraint.From<object>(value => Enumerable.Empty<Requirement>());

            ConstraintService.RegisterFor(deregisteredConstraint);
            ConstraintService.DeregisterFor<object>();
            ConstraintService.RegisterFor(expectedConstraint);

            var actualConstraint = ConstraintService.TryGetFor<object>();

            Assert.AreNotSame(deregisteredConstraint, expectedConstraint);
            Assert.AreSame(expectedConstraint, actualConstraint);
        }
        [TestMethod]
        public void TestDeregisteringAConstraintWithANameAllowsADifferentOneWithTheSameNameToBeRegistered()
        {
            var constraintName = "test";
            _constraintNamesToDeregister.Add(constraintName);

            var deregisteredConstraint = Constraint.From<object>(value => Enumerable.Empty<Requirement>());
            var expectedConstraint = Constraint.From<object>(value => Enumerable.Empty<Requirement>());

            ConstraintService.RegisterFor(deregisteredConstraint, constraintName);
            ConstraintService.DeregisterFor<object>(constraintName);
            ConstraintService.RegisterFor(expectedConstraint, constraintName);

            var actualConstraint = ConstraintService.TryGetFor<object>(constraintName);

            Assert.AreNotSame(deregisteredConstraint, expectedConstraint);
            Assert.AreSame(expectedConstraint, actualConstraint);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestRegisteringANullConstraintThrowsException()
        {
            ConstraintService.RegisterFor<object>(null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestRegisteringANullConstraintWithNameThrowsException()
        {
            ConstraintService.RegisterFor<object>(null, "test");
        }
    }
}