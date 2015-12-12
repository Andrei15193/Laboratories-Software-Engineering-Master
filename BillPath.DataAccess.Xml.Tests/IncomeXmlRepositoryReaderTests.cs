using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using BillPath.Models;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.DataAccess.Xml.Tests
{
    [TestClass]
    public class IncomeXmlRepositoryReaderTests
    {
        [TestMethod]
        public async Task TestCreatingReaderOverStreamsContainingOneIncomeLoadsItInCurrentAfterRead()
        {
            var expectedIncome =
                new Income
                {
                    Amount = new Amount(100, new Currency(new RegionInfo("en-US"))),
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 2), new TimeSpan())
                };
            using (var inputXmlStream = _GetStreamContaining(@"<income dateRealized=""2015/12/2 00:00:00:0000000 +00:00"">"
                                                               + @"<amount value=""100"" isoCode=""USD"" symbol=""$"" />"
                                                           + @"</income>"))
            using (var reader = new IncomeXmlRepository.Reader(inputXmlStream))
            {
                Assert.IsTrue(await reader.ReadAsync());
                Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(expectedIncome, reader.Current));
            }
        }
        [TestMethod]
        public async Task TestCreatingReaderOverStreamsContainingTwoIncomesLoadsThemIntoCurrentAfterRead()
        {
            var expectedIncome1 =
                new Income
                {
                    Amount = new Amount(99, new Currency(new RegionInfo("en-US"))),
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 2), new TimeSpan())
                };
            var expectedIncome2 =
                new Income
                {
                    Amount = new Amount(100, new Currency(new RegionInfo("en-US"))),
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 2), new TimeSpan())
                };
            using (var inputXmlStream = _GetStreamContaining(@"<income dateRealized=""2015/12/2 00:00:00:0000000 +00:00"">"
                                                               + @"<amount value=""99"" isoCode=""USD"" symbol=""$"" />"
                                                           + @"</income>"
                                                           + @"<income dateRealized=""2015/12/2 00:00:00:0000000 +00:00"">"
                                                               + @"<amount value=""100"" isoCode=""USD"" symbol=""$"" />"
                                                           + @"</income>"))
            using (var reader = new IncomeXmlRepository.Reader(inputXmlStream))
            {
                Assert.IsTrue(await reader.ReadAsync());
                Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(expectedIncome1, reader.Current));
                Assert.IsTrue(await reader.ReadAsync());
                Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(expectedIncome2, reader.Current));
            }
        }

        [TestMethod]
        public void TestExceptionIsThrownIfCurrentIsAccessedBeforeRead()
        {
            using (var emptyStream = new MemoryStream())
                Assert.ThrowsException<InvalidOperationException>(
                    () => new IncomeXmlRepository.Reader(emptyStream).Current);
        }

        private static Stream _GetStreamContaining(string content)
        {
            var stream = new MemoryStream();
            using (var streamWriter = new StreamWriter(stream, Encoding.UTF8, 2048, true))
                streamWriter.Write(content);
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }
    }
}