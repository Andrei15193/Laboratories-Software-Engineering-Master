using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Models.Tests
{
    [TestClass]
    public abstract class CloningTests<T>
        : SerializationTests<T>
        where T : ICloneable<T>
    {
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