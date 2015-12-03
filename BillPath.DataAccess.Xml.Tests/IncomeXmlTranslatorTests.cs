using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BillPath.Models;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.DataAccess.Xml.Tests
{
    [TestClass]
    public class IncomeXmlTranslatorTests
    {
        private static readonly IncomeXmlTranslator _translator = new IncomeXmlTranslator();

        [TestMethod]
        public async Task TestWritingFullyInitializedIncome()
        {
            var income =
                new Income
                {
                    Amount = new Amount(100, new Currency(new RegionInfo("en-US"))),
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 1), new TimeSpan()),
                    Description = "Test description"
                };
            var expectedResult = @"<income dateRealized=""2015/12/1 +00:00"" description=""Test description"">"
                                   + @"<amount value=""100"" isoCode=""USD"" symbol=""$"" />"
                               + @"</income>";

            var actualResult = await _GetXmlStringFromAsync(income);

            Assert.AreEqual(expectedResult, actualResult, ignoreCase: false);
        }

        [TestMethod]
        public async Task TestCannotWriteToNullXmlWriter()
        {
            var exception = await _translator
                .WriteToAsync(null, new Income())
                .ContinueWith(task => task.Exception.InnerException);

            Assert.ThrowsException<ArgumentNullException>(() => { throw exception; });
        }
        [TestMethod]
        public async Task TestCannotWriteNullIncome()
        {
            var exception = await _translator
                .WriteToAsync(XmlWriter.Create(new StringBuilder()), null)
                .ContinueWith(task => task.Exception.InnerException);

            Assert.ThrowsException<ArgumentNullException>(() => { throw exception; });
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\t")]
        [DataRow("\r")]
        [DataRow("\n")]
        public async Task TestWritingIncomeWithoutDescription(string description)
        {
            var income =
                new Income
                {
                    Amount = new Amount(100, new Currency(new RegionInfo("en-US"))),
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 1), new TimeSpan()),
                    Description = description
                };
            var expectedResult = @"<income dateRealized=""2015/12/1 +00:00"">"
                                    + @"<amount value=""100"" isoCode=""USD"" symbol=""$"" />"
                               + @"</income>";

            string actualResult = await _GetXmlStringFromAsync(income);

            Assert.AreEqual(expectedResult, actualResult, ignoreCase: false);
        }

        [TestMethod]
        public async Task TestReadIncomeFromXml()
        {
            var incomeXmlString = @"<income dateRealized=""2015/12/1 +00:00"" description=""Test description"">"
                                    + @"<amount value=""100"" isoCode=""USD"" symbol=""$"" />"
                                + @"</income>";
            var expectedIncome =
                new Income
                {
                    Amount = new Amount(100, new Currency(new RegionInfo("en-US"))),
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 1), new TimeSpan()),
                    Description = "Test description"
                };

            var actualIncome = await _GetIncomeFrom(incomeXmlString);

            Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(expectedIncome, actualIncome));
        }

        [TestMethod]
        public async Task TestTryingToReadIncomeFromXmlThatDoesNotContainsOneReturnsNull()
        {
            var incomeXmlString = @"<notAnIncome dateRealized=""2015/12/1 +00:00"" description=""Test description"">"
                                    + @"<amount value=""100"" isoCode=""USD"" symbol=""$"" />"
                                + @"</notAnIncome>";
            var income = await _GetIncomeFrom(incomeXmlString);

            Assert.IsNull(income);
        }

        [TestMethod]
        public async Task TestWritingAndReadingAnIncomeProvidesAnInstanceEqualWithTheOriginal()
        {
            var expectedIncome =
                new Income
                {
                    Amount = new Amount(100, new Currency(new RegionInfo("en-US"))),
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 1), new TimeSpan()),
                    Description = "Test description"
                };
            var incomeXmlString = await _GetXmlStringFromAsync(expectedIncome);
            var actualIncome = await _GetIncomeFrom(incomeXmlString);

            Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(expectedIncome, actualIncome));
        }

        private static async Task<string> _GetXmlStringFromAsync(Income income)
        {
            var resultBuilder = new StringBuilder();
            using (var xmlWriter = XmlWriter.Create(
                resultBuilder,
                new XmlWriterSettings
                {
                    Async = true,
                    Indent = false,
                    ConformanceLevel = ConformanceLevel.Fragment
                }))
                await _translator.WriteToAsync(xmlWriter, income);

            return resultBuilder.ToString();
        }
        private static async Task<Income> _GetIncomeFrom(string xmlString)
        {
            using (var xmlStringReader = new StringReader(xmlString))
            using (var xmlReader = XmlReader.Create(xmlStringReader, new XmlReaderSettings { Async = true }))
                return await _translator.ReadFromAsync(xmlReader);
        }
    }
}