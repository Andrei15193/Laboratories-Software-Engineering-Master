using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillPath.Models;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.DataAccess.Xml.Tests
{
    [TestClass]
    public class IncomeXmlRepositoryReaderTests
    {
        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(5)]
        [DataRow(8)]
        public async Task TestCreatingReaderOverStreamsContainingOneIncomeLoadsItInCurrentAfterRead(int streamRepeatCount)
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
            using (var reader = new IncomeXmlRepository.Reader(_RepeatCopy(inputXmlStream, streamRepeatCount).GetEnumerator()))
                for (var incomeIndex = 0; incomeIndex < streamRepeatCount; incomeIndex++)
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
            using (var reader = new IncomeXmlRepository.Reader(Enumerable.Repeat(inputXmlStream, 1).GetEnumerator()))
            {
                Assert.IsTrue(await reader.ReadAsync());
                Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(expectedIncome1, reader.Current));
                Assert.IsTrue(await reader.ReadAsync());
                Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(expectedIncome2, reader.Current));
            }
        }

        [TestMethod]
        public void TestExceptionIsThrownIfCurrentIsAccessedBeforeRead()
            => Assert.ThrowsException<InvalidOperationException>(
                () => new IncomeXmlRepository.Reader(Enumerable.Empty<Stream>().GetEnumerator()).Current);

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(5)]
        [DataRow(8)]
        public async Task TestExceptionIsThrownIfCurrentIsAccessedAfterReadReturnedFalse(int streamRepeatCount)
        {
            using (var inputXmlStream = _GetStreamContaining(@"<income dateRealized=""2015/12/2 00:00:00:0000000 +00:00"">"
                                                               + @"<amount value=""100"" isoCode=""USD"" symbol=""$"" />"
                                                           + @"</income>"))
            using (var reader = new IncomeXmlRepository.Reader(_RepeatCopy(inputXmlStream, streamRepeatCount).GetEnumerator()))
            {
                while (await reader.ReadAsync())
                    ;
                Assert.ThrowsException<InvalidOperationException>(() => reader.Current);
            }
        }

        private IEnumerable<Stream> _RepeatCopy(Stream stream, int count)
        {
            for (var streamIndex = 0; streamIndex < count; streamIndex++)
            {
                var streamCopy = new MemoryStream();
                stream.CopyTo(streamCopy);
                stream.Seek(0, SeekOrigin.Begin);
                streamCopy.Seek(0, SeekOrigin.Begin);

                yield return streamCopy;
            }
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