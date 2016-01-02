using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Modern.Converters.Tests
{
    [TestClass]
    public class BooleanNotConverterTests
    {
        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestConverterReturnsNegatedBooleanValueOnConvert(bool value)
        {
            var converter = new BooleanNotConverter();
            var expectedValue = !value;

            var actualValue = (bool)converter.Convert(value, typeof(bool), null, null);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TestConverterReturnsNegatedBooleanValueOnConvertBack(bool value)
        {
            var converter = new BooleanNotConverter();
            var expectedValue = !value;

            var actualValue = (bool)converter.ConvertBack(value, typeof(bool), null, null);

            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}