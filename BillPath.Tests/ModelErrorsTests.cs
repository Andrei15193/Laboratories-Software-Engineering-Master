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
            var modelErrors = new ModelErrors(ModelStates.GetFor(new ModelMock()));

            Assert.AreEqual(1, modelErrors.EnumerateAll().Count());
            Assert.AreEqual(1, modelErrors[nameof(ModelMock.Property)].Count);
            Assert.IsFalse(modelErrors.Any<string>());
        }

        [TestMethod]
        public void TestCreateWithValidModelHasNoErrors()
        {
            var modelErrors = new ModelErrors(ModelStates.GetFor(
                new ModelMock
                {
                    Property = new object()
                }));

            Assert.IsFalse(modelErrors.EnumerateAll().Any());
        }

        [TestMethod]
        public void TestCreateWithInvalidModelAndThenChangingItThroughStateUpdatesValidationResults()
        {
            var modelContext = ModelStates.GetFor(new ModelMock());
            var modelErrors = new ModelErrors(modelContext);

            Assert.AreEqual(1, modelErrors.EnumerateAll().Count());

            modelContext[nameof(ModelMock.Property)] = new object();

            Assert.IsFalse(modelErrors.EnumerateAll().Any());
        }

        [TestMethod]
        public void TestCreateWithInvalidModelAndThenChangingItThroughModelDoesNotUpdateValidationResults()
        {
            var model = new ModelMock();
            var modelErrors = new ModelErrors(ModelStates.GetFor(model));

            Assert.AreEqual(1, modelErrors.EnumerateAll().Count());

            model.Property = new object();

            Assert.AreEqual(1, modelErrors.EnumerateAll().Count());
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