using System;
using System.Globalization;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Modern.Converters.Tests
{
    [TestClass]
    public class DecimalToStringConverterTests
    {
        [DataTestMethod]
        [DataRow(0, null, null)]
        [DataRow(1.2, null, null)]
        [DataRow(3.5, null, null)]
        [DataRow(8.13, null, null)]
        [DataRow(21.44, null, null)]
        [DataRow(65.109, null, null)]
        [DataRow(174.283, null, null)]
        [DataRow(457.74, null, null)]
        [DataRow(1197.1937, null, null)]
        [DataRow(0, "N", null)]
        [DataRow(1.2, "N", null)]
        [DataRow(3.5, "N", null)]
        [DataRow(8.13, "N", null)]
        [DataRow(21.44, "N", null)]
        [DataRow(65.109, "N", null)]
        [DataRow(174.283, "N", null)]
        [DataRow(457.74, "N", null)]
        [DataRow(1197.1937, "N", null)]
        [DataRow(0, null, "ro-RO")]
        [DataRow(1.2, null, "ro-RO")]
        [DataRow(3.5, null, "ro-RO")]
        [DataRow(8.13, null, "ro-RO")]
        [DataRow(21.44, null, "ro-RO")]
        [DataRow(65.109, null, "ro-RO")]
        [DataRow(174.283, null, "ro-RO")]
        [DataRow(457.74, null, "ro-RO")]
        [DataRow(1197.1937, null, "ro-RO")]
        [DataRow(0, "N", "ro-RO")]
        [DataRow(1.2, "N", "ro-RO")]
        [DataRow(3.5, "N", "ro-RO")]
        [DataRow(8.13, "N", "ro-RO")]
        [DataRow(21.44, "N", "ro-RO")]
        [DataRow(65.109, "N", "ro-RO")]
        [DataRow(174.283, "N", "ro-RO")]
        [DataRow(457.74, "N", "ro-RO")]
        [DataRow(1197.1937, "N", "ro-RO")]
        public void TestConvertingDecimalToStringReturnsItsStringRepresentation(double value, string format, string language)
        {
            var decimalValue = (decimal)value;
            var expectedValue = decimalValue.ToString(
                format,
                language == null ? CultureInfo.CurrentCulture : new CultureInfo(language));
            var converter = new DecimalToStringConverter();

            var actualValue = (string)converter.Convert(decimalValue, typeof(string), format, language);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [DataTestMethod]
        [DataRow(0, null, null)]
        [DataRow(1.2, null, null)]
        [DataRow(3.5, null, null)]
        [DataRow(8.13, null, null)]
        [DataRow(21.44, null, null)]
        [DataRow(65.109, null, null)]
        [DataRow(174.283, null, null)]
        [DataRow(457.74, null, null)]
        [DataRow(1197.1937, null, null)]
        [DataRow(0, "N", null)]
        [DataRow(1.2, "N", null)]
        [DataRow(3.5, "N", null)]
        [DataRow(8.13, "N", null)]
        [DataRow(21.44, "N", null)]
        [DataRow(65.10, "N", null)]
        [DataRow(174.28, "N", null)]
        [DataRow(457.74, "N", null)]
        [DataRow(1197.19, "N", null)]
        [DataRow(0, null, "ro-RO")]
        [DataRow(1.2, null, "ro-RO")]
        [DataRow(3.5, null, "ro-RO")]
        [DataRow(8.13, null, "ro-RO")]
        [DataRow(21.44, null, "ro-RO")]
        [DataRow(65.109, null, "ro-RO")]
        [DataRow(174.283, null, "ro-RO")]
        [DataRow(457.74, null, "ro-RO")]
        [DataRow(1197.1937, null, "ro-RO")]
        [DataRow(0, "N", "ro-RO")]
        [DataRow(1.2, "N", "ro-RO")]
        [DataRow(3.5, "N", "ro-RO")]
        [DataRow(8.13, "N", "ro-RO")]
        [DataRow(21.44, "N", "ro-RO")]
        [DataRow(65.10, "N", "ro-RO")]
        [DataRow(174.28, "N", "ro-RO")]
        [DataRow(457.74, "N", "ro-RO")]
        [DataRow(1197.19, "N", "ro-RO")]
        public void TestConvertingBackStringToDecimalFromItsStringRepresentationReturnsValidDecimal(double value, string format, string language)
        {
            var expectedValue = (decimal)value;
            var stringValue = expectedValue.ToString(
                format,
                language == null ? CultureInfo.CurrentCulture : new CultureInfo(language));
            var converter = new DecimalToStringConverter();

            var actualValue = (decimal)converter.ConvertBack(stringValue, typeof(string), format, language);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [DataTestMethod]
        [DataRow("0a")]
        [DataRow("1.2a")]
        [DataRow("3.5a")]
        [DataRow("8.13a")]
        [DataRow("21.44a")]
        [DataRow("65.10a")]
        [DataRow("174.28a")]
        [DataRow("457.74a")]
        [DataRow("1197.19a")]
        public void TestTryingToConvertBackAnInvalidStringToDecimalThrowsExpcetion(string invalidDecimal)
            => Assert.ThrowsException<FormatException>(
                () => new DecimalToStringConverter().ConvertBack(
                    invalidDecimal,
                    typeof(decimal),
                    null,
                    null));
    }
}