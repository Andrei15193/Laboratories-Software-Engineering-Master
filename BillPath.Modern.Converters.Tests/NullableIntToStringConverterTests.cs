using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Modern.Converters.Tests
{
    [TestClass]
    public class NullableIntToStringConverterTests
    {
        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(5)]
        [DataRow(8)]
        [DataRow(13)]
        [DataRow(21)]
        public void TestConvertingIntValueReturnsItsStringRepresentation(int value)
        {
            var converter = new NullableIntToStringConverter();
            var expectedValue = value.ToString();

            var actualValue = converter.Convert(value, typeof(string), null, null);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestConvertingNullReturnsStringEmpty()
        {
            var converter = new NullableIntToStringConverter();

            var actualValue = (string)converter.Convert(null, typeof(string), null, null);

            Assert.AreEqual(string.Empty, actualValue);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(5)]
        [DataRow(8)]
        [DataRow(13)]
        [DataRow(21)]
        public void TestConvertingBackIntegerStringRepresentationReturnsValidIntValue(int value)
        {
            var converter = new NullableIntToStringConverter();
            var expectedValue = new int?(value);

            var actualValue = (int?)converter.ConvertBack(value.ToString(), typeof(int?), null, null);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestConvertingBackStringEmptyReturnsNull()
        {
            var converter = new NullableIntToStringConverter();
            var expectedValue = new int?();

            var actualValue = (int?)converter.ConvertBack(string.Empty, typeof(int?), null, null);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [DataTestMethod]
        [DataRow("0a")]
        [DataRow("1a")]
        [DataRow("2a")]
        [DataRow("3a")]
        [DataRow("5a")]
        [DataRow("8a")]
        [DataRow("13a")]
        [DataRow("21a")]
        public void TestConvertingBackInvalidIntegerStringRepresentationReturnsNull(string value)
        {
            var converter = new NullableIntToStringConverter();
            var expectedValue = new int?();

            var actualValue = (int?)converter.ConvertBack(value, typeof(int?), null, null);

            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}