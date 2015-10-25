using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Modern.Converters.Tests
{
    [TestClass]
    public class IndexerConverterTests
    {
        [TestMethod]
        public void TestConverterExtractsValueHavingParameterKey()
        {
            var converter = new IndexerConverter();
            var dictionary =
                new Dictionary<object, object>
                {
                    { new object(), new object() },
                    { new object(), new object() },
                    { new object(), new object() },
                    { new object(), new object() },
                    { new object(), new object() }
                };

            foreach (var key in dictionary.Keys)
                Assert.AreSame(dictionary[key], converter.Convert(
                    dictionary,
                    null,
                    key,
                    null));
        }

        [TestMethod]
        public void TestConvertBackThrowsException()
            => Assert.ThrowsException<NotImplementedException>(
                () => new IndexerConverter().ConvertBack(
                    null,
                    null,
                    null,
                    null));
    }
}