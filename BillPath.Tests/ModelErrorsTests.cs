using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Tests
{
    [TestClass]
    public class ModelErrorsTests
    {
        public class ModelMock
        {
            [Required]
            public object Property
            {
                get;
                set;
            }
        }

        [TestMethod]
        public void TestCreateWithNullThrowsException()
            => Assert.ThrowsException<ArgumentNullException>(() => new ModelErrors(null));

        [TestMethod]
        public void TestCreateWithInvalidModelLoadsErrors()
        {
            dynamic modelErrors = new ModelErrors(ModelStates.GetFor(new ModelMock()));

            Assert.AreEqual(1, Enumerable.Count(modelErrors.EnumerateAll()));
            Assert.AreEqual(1, Enumerable.Count(modelErrors.Property));
            Assert.IsFalse(Enumerable.Any(modelErrors));
        }

        [TestMethod]
        public void TestCreateWithValidModelHasNoErrors()
        {
            dynamic modelErrors = new ModelErrors(ModelStates.GetFor(
                new ModelMock
                {
                    Property = new object()
                }));

            Assert.IsFalse(Enumerable.Any(modelErrors.EnumerateAll()));
        }

        [TestMethod]
        public void TestCreateWithInvalidModelAndThenChangingItThroughStateUpdatesValidationResults()
        {
            dynamic modelContext = ModelStates.GetFor(new ModelMock());
            dynamic modelErrors = new ModelErrors(modelContext);

            Assert.AreEqual(1, Enumerable.Count(modelErrors.EnumerateAll()));

            modelContext.Property = new object();

            Assert.IsFalse(Enumerable.Any(modelErrors.EnumerateAll()));
        }

        [TestMethod]
        public void TestCreateWithInvalidModelAndThenChangingItThroughModelDoesNotUpdateValidationResults()
        {
            var model = new ModelMock();
            dynamic modelErrors = new ModelErrors(ModelStates.GetFor(model));

            Assert.AreEqual(1, Enumerable.Count(modelErrors.EnumerateAll()));

            model.Property = new object();

            Assert.AreEqual(1, Enumerable.Count(modelErrors.EnumerateAll()));
        }

        private sealed class AggregateRootModel
        {
            public class ChildModel
            {
                [Required]
                public object Property
                {
                    get;
                    set;
                }
            }

            public ChildModel Property
            {
                get;
                set;
            }
        }
        [TestMethod]
        public void TestEnumerateAllReturnsErrorsFromAggregates()
        {
            var modelErrors = new ModelErrors(ModelStates.GetFor(new AggregateRootModel { Property = new AggregateRootModel.ChildModel() }));

            var errors = modelErrors.EnumerateAll();

            Assert.AreEqual(1, errors.Count());
        }

        private sealed class CircularReferenceAggregate
        {
            public CircularReferenceAggregate CircularReferenceProperty
            {
                get;
                set;
            }
        }
        [TestMethod]
        public void TestCircularReferenceDoesNotCrashWhenEnumeratingAllErrors()
        {
            var model = new CircularReferenceAggregate();
            model.CircularReferenceProperty = model;
            var modelErrors = new ModelErrors(ModelStates.GetFor(model));

            var errors = modelErrors.EnumerateAll();

            Assert.IsFalse(errors.Any());
        }
    }
}