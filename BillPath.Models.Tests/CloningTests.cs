using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Models.Tests
{
    [TestClass]
    public abstract class CloningTests<T>
        where T : ICloneable<T>
    {
        protected static ModelValidator ModelValidator { get; } = new ModelValidator();

        [TestInitialize]
        public virtual void TestInitialize()
        {
            Instance = GetNewInstance();
            SetValidTestDataToInstance();

            if (ModelValidator.Validate(Instance).Any())
                Assert.Inconclusive("The instance is in an invalid state.");
        }

        protected virtual T GetNewInstance()
        {
            return Activator.CreateInstance<T>();
        }
        protected abstract void SetValidTestDataToInstance();
        protected T Instance
        {
            get;
            private set;
        }
        protected abstract void AssertInstanceIsEqualTo(T other);

        [TestCleanup]
        public virtual void TestCleanup()
        {
            Instance = default(T);
        }

        [TestMethod]
        public void TestCloneIsNotTheSameAsOriginal()
        {
            Assert.AreNotSame(Instance, Instance.Clone());
        }
        [TestMethod]
        public void TestCloneIsEqualToOriginal()
        {
            AssertInstanceIsEqualTo(Instance.Clone());
        }
    }
}