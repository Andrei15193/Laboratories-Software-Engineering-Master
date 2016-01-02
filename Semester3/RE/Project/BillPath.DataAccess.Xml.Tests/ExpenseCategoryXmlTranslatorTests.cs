using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BillPath.Models;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.DataAccess.Xml.Tests
{
    [TestClass]
    public class ExpenseCategoryXmlTranslatorTests
    {
        private static readonly XmlTranslator<ExpenseCategory> _xmlTranslator = new ExpenseCategoryXmlTranslator();
        private static readonly XmlWriterSettings _xmlWriterSettings =
            new XmlWriterSettings
            {
                Async = true,
                OmitXmlDeclaration = true,
                ConformanceLevel = ConformanceLevel.Fragment,
                Indent = false
            };
        private static readonly XmlReaderSettings _xmlReaderSettings =
            new XmlReaderSettings
            {
                Async = true
            };

        [TestMethod]
        public async Task TestTranslateExpenseCategoryToXml()
        {
            var expectedOutput = @"<expenseCategory name=""test"" color=""#AAFFEEBB"" />";
            var outputStringBuilder = new StringBuilder();
            using (var outputXmlWriter = XmlWriter.Create(outputStringBuilder, _xmlWriterSettings))
                await _xmlTranslator.WriteToAsync(
                    outputXmlWriter,
                    new ExpenseCategory
                    {
                        Name = "test",
                        Color = new ArgbColor(0xAA, 0xFF, 0xEE, 0xBB)
                    });

            Assert.AreEqual(
                expectedOutput,
                outputStringBuilder.ToString(),
                ignoreCase: false);
        }
        [TestMethod]
        public async Task TestTranslateExpenseCategoryFromXml()
        {
            var expenseCategoryXmlString = @"<expenseCategory name=""test"" color=""#AAFFEEBB"" />";
            var expectedExpenseCategory =
                new ExpenseCategory
                {
                    Name = "test",
                    Color = new ArgbColor(0xAA, 0xFF, 0xEE, 0xBB)
                };
            ExpenseCategory actualExpenseCategory;

            using (var expenseCategoryStringReader = new StringReader(expenseCategoryXmlString))
            using (var expenseCategoryXmlReader = XmlReader.Create(
                expenseCategoryStringReader,
                _xmlReaderSettings))
                actualExpenseCategory = await _xmlTranslator.ReadFromAsync(expenseCategoryXmlReader);

            _AssertAreEqual(
                expectedExpenseCategory,
                actualExpenseCategory);
        }
        [TestMethod]
        public async Task TestTranslatingToAndFromXmlReturnsEqualExpenseCategoryAsOriginal()
        {
            var expectedExpenseCategory =
                new ExpenseCategory
                {
                    Name = "test",
                    Color = new ArgbColor(0xAA, 0xFF, 0xEE, 0xBB)
                };
            ExpenseCategory actualExpenseCategory;

            var expenseCategoryStringBuilder = new StringBuilder();
            using (var expenseCategoryXmlWriter = XmlWriter.Create(expenseCategoryStringBuilder, _xmlWriterSettings))
                await _xmlTranslator.WriteToAsync(
                    expenseCategoryXmlWriter,
                    expectedExpenseCategory);

            using (var expenseCategoryStringReader = new StringReader(expenseCategoryStringBuilder.ToString()))
            using (var expenseCategoryXmlReader = XmlReader.Create(
                expenseCategoryStringReader,
                _xmlReaderSettings))
                actualExpenseCategory = await _xmlTranslator.ReadFromAsync(expenseCategoryXmlReader);

            _AssertAreEqual(
                expectedExpenseCategory,
                actualExpenseCategory);
        }
        [TestMethod]
        public async Task TestTryingToReadFromNullXmlReaderThrowsException()
        {
            var exception = await _xmlTranslator.ReadFromAsync(null).ContinueWith(task => task.Exception.InnerException);

            Assert.ThrowsException<ArgumentNullException>(delegate { throw exception; });
        }
        [TestMethod]
        public async Task TestTryingToWriteToNullXmlWriterThrowsException()
        {
            var exception = await _xmlTranslator
                .WriteToAsync(null, new ExpenseCategory())
                .ContinueWith(task => task.Exception.InnerException);

            Assert.ThrowsException<ArgumentNullException>(delegate { throw exception; });
        }
        [TestMethod]
        public async Task TestTryingToWriteNullExpenseCategoryThrowsException()
        {
            Exception exception;
            using (var stream = new MemoryStream())
            using (var xmlWriter = XmlWriter.Create(stream))
                exception = await _xmlTranslator
                    .WriteToAsync(xmlWriter, null)
                    .ContinueWith(task => task.Exception.InnerException);

            Assert.ThrowsException<ArgumentNullException>(delegate { throw exception; });
        }

        private void _AssertAreEqual(ExpenseCategory expected, ExpenseCategory actual)
        {
            Assert.AreEqual(
                expected.Name,
                actual.Name);
            Assert.AreEqual(
                expected.Color,
                actual.Color);
        }
    }
}