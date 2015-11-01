using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Tests
{
    [TestClass]
    public class TypeHelperTests
    {
        private class ObjectMock
        {
            public static object PublicStaticProperty
            {
                get;
                set;
            }
            public object PublicProperty
            {
                get;
                set;
            }

            internal static object InternalStaticProperty
            {
                get;
                set;
            }
            internal object InternalProperty
            {
                get;
                set;
            }

            protected static object ProtectedStaticProperty
            {
                get;
                set;
            }
            protected object ProtectedProperty
            {
                get;
                set;
            }

            protected internal static object ProtectedInternalStaticProperty
            {
                get;
                set;
            }
            protected internal object ProtectedInternalProperty
            {
                get;
                set;
            }

            private static object PrivateStaticProperty
            {
                get;
                set;
            }
            private object PrivateProperty
            {
                get;
                set;
            }
        }

        [TestMethod]
        public void TestGetPublicRuntimePropertiesReturnsPublicInstanceAndStaticProperties()
        {
            Assert.Inconclusive();
            //var publicProperties = TypeHelper.GetPublicRuntimeProperties(typeof(ObjectMock));

            //Assert.AreEqual(2, publicProperties.Count());
            //Assert.IsTrue(publicProperties
            //    .Any(publicRuntimeProperty => nameof(ObjectMock.PublicStaticProperty).Equals(
            //        publicRuntimeProperty.Name,
            //        StringComparison.Ordinal)));
            //Assert.IsTrue(publicProperties
            //    .Any(publicRuntimeProperty => nameof(ObjectMock.PublicProperty).Equals(
            //        publicRuntimeProperty.Name,
            //        StringComparison.Ordinal)));
        }
    }
}