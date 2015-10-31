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
            dynamic modelErrors = new ModelErrors(new ModelState(new ModelMock()));

            Assert.AreEqual(1, Enumerable.Count(modelErrors.EnumerateAll()));
            Assert.AreEqual(1, Enumerable.Count(modelErrors.Property));
            Assert.IsFalse(Enumerable.Any(modelErrors));
        }

        [TestMethod]
        public void TestCreateWithValidModelHasNoErrors()
        {
            dynamic modelErrors = new ModelErrors(new ModelState(
                new ModelMock
                {
                    Property = new object()
                }));

            Assert.IsFalse(Enumerable.Any(modelErrors.EnumerateAll()));
        }

        [TestMethod]
        public void TestCreateWithInvalidModelAndThenChangingItThroughStateUpdatesValidationResults()
        {
            dynamic modelContext = new ModelState(new ModelMock());
            dynamic modelErrors = new ModelErrors(modelContext);

            Assert.AreEqual(1, Enumerable.Count(modelErrors.EnumerateAll()));

            modelContext.Property = new object();

            Assert.IsFalse(Enumerable.Any(modelErrors.EnumerateAll()));
        }

        [TestMethod]
        public void TestCreateWithInvalidModelAndThenChangingItThroughModelDoesNotUpdateValidationResults()
        {
            var model = new ModelMock();
            dynamic modelErrors = new ModelErrors(new ModelState(model));

            Assert.AreEqual(1, Enumerable.Count(modelErrors.EnumerateAll()));

            model.Property = new object();

            Assert.AreEqual(1, Enumerable.Count(modelErrors.EnumerateAll()));
        }
    }
}