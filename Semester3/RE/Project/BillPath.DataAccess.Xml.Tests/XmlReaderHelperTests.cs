using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.DataAccess.Xml.Tests
{
    [TestClass]
    public class XmlReaderHelperTests
    {
        [TestMethod]
        public void TestToXmlNameLowersFirstCharacter()
        {
            var name = "TestName";
            var expectedXmlName = "testName";
            var actualXmlName = XmlReaderHelper.ToXmlName(name);

            Assert.AreEqual(expectedXmlName, actualXmlName);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\t")]
        [DataRow("\r")]
        [DataRow("\n")]
        public void TestToXmlNameReturnsSameValueForNullEmptyOrWhiteSpaceName(string name)
            => Assert.AreSame(name, XmlReaderHelper.ToXmlName(name));

        [TestMethod]
        public void TestIsOnNodeReturnsTrueIfReaderIsOnSpecifiedNode()
        {
            var xmlFragment = @"<root><node /></root>";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
            {
                Assert.IsTrue(xmlReader.ReadToFollowing("root"));
                Assert.IsTrue(XmlReaderHelper.IsOnNode(xmlReader, "root"));
            }
        }
        [TestMethod]
        public void TestIsOnNodeReturnsFalseIfReaderIsNotOnSpecifiedNode()
        {
            var xmlFragment = @"<root><node /></root>";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
            {
                Assert.IsTrue(xmlReader.ReadToFollowing("root"));
                Assert.IsFalse(XmlReaderHelper.IsOnNode(xmlReader, "node"));
            }
        }
        [TestMethod]
        public void TestIsOnNodeThrowsExceptionForNullXmlReaderButValidName()
            => Assert.ThrowsException<ArgumentNullException>(() => XmlReaderHelper.IsOnNode(null, "node"));
        [TestMethod]
        public void TestIsOnNodeThrowsExceptionForNullName()
        {
            var xmlFragment = @"<root><node /></root>";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
                Assert.ThrowsException<ArgumentNullException>(() => XmlReaderHelper.IsOnNode(xmlReader, null));
        }
        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\t")]
        [DataRow("\r")]
        [DataRow("\n")]
        public void TestIsOnNodeThrowsExceptionForEmptyOrWhiteSpaceName(string name)
        {
            var xmlFragment = @"<root><node /></root>";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
                Assert.ThrowsException<ArgumentException>(() => XmlReaderHelper.IsOnNode(xmlReader, name));
        }

        [TestMethod]
        public void TestIsOnNodeReturnsTrueIfReaderIsOnSpecifiedNodeWithNamespace()
        {
            var xmlFragment = @"<root xmlns=""test""><node /></root>";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
            {
                Assert.IsTrue(xmlReader.ReadToFollowing("root", "test"));
                Assert.IsTrue(XmlReaderHelper.IsOnNode(xmlReader, "root", "test"));
            }
        }
        [TestMethod]
        public void TestIsOnNodeReturnsFalseIfReaderIsNotOnSpecifiedNodeWithNamespace()
        {
            var xmlFragment = @"<root xmlns=""test""><node /></root>";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
            {
                Assert.IsTrue(xmlReader.ReadToFollowing("root", "test"));
                Assert.IsFalse(XmlReaderHelper.IsOnNode(xmlReader, "root", "test2"));
            }
        }
        [TestMethod]
        public void TestIsOnNodeThrowsExceptionForNullXmlReaderWithValidNameAndNamespace()
            => Assert.ThrowsException<ArgumentNullException>(() => XmlReaderHelper.IsOnNode(null, "node", "test"));
        [TestMethod]
        public void TestIsOnNodeThrowsExceptionForNullNameButValidNamespace()
        {
            var xmlFragment = @"<root xmlns=""test""><node /></root>";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
                Assert.ThrowsException<ArgumentNullException>(() => XmlReaderHelper.IsOnNode(xmlReader, null, "test"));
        }
        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\t")]
        [DataRow("\r")]
        [DataRow("\n")]
        public void TestIsOnNodeThrowsExceptionForEmptyOrWhiteSpaceNameButValidNamespace(string name)
        {
            var xmlFragment = @"<root xmlns=""test""><node /></root>";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
                Assert.ThrowsException<ArgumentException>(() => XmlReaderHelper.IsOnNode(xmlReader, name, "test"));
        }
        [TestMethod]
        public void TestIsOnNodeThrowsExceptionForNullNamespaceButValidName()
        {
            var xmlFragment = @"<root xmlns=""test""><node /></root>";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
                Assert.ThrowsException<ArgumentNullException>(() => XmlReaderHelper.IsOnNode(xmlReader, "test", null));
        }
        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\t")]
        [DataRow("\r")]
        [DataRow("\n")]
        public void TestIsOnNodeThrowsExceptionForEmptyOrWhiteSpaceNamespaceButValidName(string @namespace)
        {
            var xmlFragment = @"<root xmlns=""test""><node /></root>";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
                Assert.ThrowsException<ArgumentException>(() => XmlReaderHelper.IsOnNode(xmlReader, "root", @namespace));
        }

        [TestMethod]
        public void TestReadUntilDoesNotAdvanceTheReaderIfItAlreadyIsOnTheDesiredElement()
        {
            var xmlFragment = @"<root><node /></root>";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
            {
                Assert.IsTrue(xmlReader.ReadToFollowing("root"));
                Assert.IsTrue(XmlReaderHelper.ReadUntil(xmlReader, "root"));
                Assert.IsTrue(xmlReader.IsOnNode("root"));
            }
        }
        [TestMethod]
        public void TestReadUntilAdvancesTheReaderToTheDesiredNode()
        {
            var xmlFragment = @"<root><node /></root>";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
            {
                Assert.IsTrue(XmlReaderHelper.ReadUntil(xmlReader, "root"));
                Assert.IsTrue(xmlReader.IsOnNode("root"));
            }
        }
        [TestMethod]
        public void TestReadUntilDoesNotAdvanceTheReaderIfItAlreadyIsOnTheDesiredElementWithNamespace()
        {
            var xmlFragment = @"<root xmlns=""test""><node /></root>";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
            {
                Assert.IsTrue(xmlReader.ReadToFollowing("root", "test"));
                Assert.IsTrue(XmlReaderHelper.ReadUntil(xmlReader, "root", "test"));
                Assert.IsTrue(xmlReader.IsOnNode("root", "test"));
            }
        }
        [TestMethod]
        public void TestReadUntilWithNullReaderThrowsException()
            => Assert.ThrowsException<ArgumentNullException>(() => XmlReaderHelper.ReadUntil(null, "root"));
        [TestMethod]
        public void TestReadUntilWithNullNameThrowsException()
        {
            var xmlFragment = @"<root><node /></root>";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
                Assert.ThrowsException<ArgumentNullException>(() => XmlReaderHelper.ReadUntil(xmlReader, null));
        }
        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\t")]
        [DataRow("\r")]
        [DataRow("\n")]
        public void TestReadUntilWithEmptyOrWhiteSpaceNameThrowsException(string name)
        {
            var xmlFragment = @"<root><node /></root>";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
                Assert.ThrowsException<ArgumentException>(() => XmlReaderHelper.ReadUntil(xmlReader, name));
        }

        [TestMethod]
        public void TestReadUntilAdvancesTheReaderToTheDesiredNodeWithNamespace()
        {
            var xmlFragment = @"<root xmlns=""test""><node /></root>";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
            {
                Assert.IsTrue(XmlReaderHelper.ReadUntil(xmlReader, "root", "test"));
                Assert.IsTrue(xmlReader.IsOnNode("root", "test"));
            }
        }
        [TestMethod]
        public void TestReadUntilWithNullReaderForNamespaceOverloadThrowsException()
        {
            var xmlFragment = @"<root xmlns=""test"" />";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
                Assert.ThrowsException<ArgumentNullException>(() => XmlReaderHelper.ReadUntil(null, "root", "test"));
        }
        [TestMethod]
        public void TestReadUntilWithNullNameButValidNamespaceThrowsException()
        {
            var xmlFragment = @"<root xmlns=""test"" />";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
                Assert.ThrowsException<ArgumentNullException>(() => XmlReaderHelper.ReadUntil(xmlReader, null, "test"));
        }
        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\t")]
        [DataRow("\r")]
        [DataRow("\n")]
        public void TestReadUntilWithEmptyOrWhiteSpaceNameButValidNamespaceThrowsException(string name)
        {
            var xmlFragment = @"<root xmlns=""test"" />";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
                Assert.ThrowsException<ArgumentException>(() => XmlReaderHelper.ReadUntil(xmlReader, name, "test"));
        }
        [TestMethod]
        public void TestReadUntilWithNullNamespaceButValidNameThrowsException()
        {
            var xmlFragment = @"<root xmlns=""test"" />";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
                Assert.ThrowsException<ArgumentNullException>(() => XmlReaderHelper.ReadUntil(xmlReader, "root", null));
        }
        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\t")]
        [DataRow("\r")]
        [DataRow("\n")]
        public void TestReadUntilWithEmptyOrWhiteSpaceNamespaceButValidNameThrowsException(string @namespace)
        {
            var xmlFragment = @"<root xmlns=""test"" />";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
                Assert.ThrowsException<ArgumentException>(() => XmlReaderHelper.ReadUntil(xmlReader, "root", @namespace));
        }

        [TestMethod]
        public async Task TestReadUntilAsyncDoesNotAdvanceTheReaderIfItAlreadyIsOnTheDesiredElement()
        {
            var xmlFragment = @"<root><node /></root>";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader, new XmlReaderSettings { Async = true }))
            {
                Assert.IsTrue(xmlReader.ReadToFollowing("root"));
                Assert.IsTrue(await XmlReaderHelper.ReadUntilAsync(xmlReader, "root"));
                Assert.IsTrue(xmlReader.IsOnNode("root"));
            }
        }
        [TestMethod]
        public async Task TestReadUntilAsyncAdvancesTheReaderToTheDesiredNode()
        {
            var xmlFragment = @"<root><node /></root>";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader, new XmlReaderSettings { Async = true }))
            {
                Assert.IsTrue(await XmlReaderHelper.ReadUntilAsync(xmlReader, "root"));
                Assert.IsTrue(xmlReader.IsOnNode("root"));
            }
        }
        [TestMethod]
        public async Task TestReadUntilAsyncDoesNotAdvanceTheReaderIfItAlreadyIsOnTheDesiredElementWithNamespace()
        {
            var xmlFragment = @"<root xmlns=""test""><node /></root>";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
            {
                Assert.IsTrue(xmlReader.ReadToFollowing("root", "test"));
                Assert.IsTrue(await XmlReaderHelper.ReadUntilAsync(xmlReader, "root", "test"));
                Assert.IsTrue(xmlReader.IsOnNode("root", "test"));
            }
        }
        [TestMethod]
        public async Task TestReadUntilAsyncWithNullReaderThrowsException()
        {
            var exception = await XmlReaderHelper.ReadUntilAsync(null, "root")
                .ContinueWith(task => task.Exception.InnerException);

            Assert.ThrowsException<ArgumentNullException>(() => { throw exception; });
        }
        [TestMethod]
        public async Task TestReadUntilAsyncWithNullNameThrowsException()
        {
            var xmlFragment = @"<root><node /></root>";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
            {
                var exception = await XmlReaderHelper.ReadUntilAsync(xmlReader, null)
                    .ContinueWith(task => task.Exception.InnerException);
                Assert.ThrowsException<ArgumentNullException>(() => { throw exception; });
            }
        }
        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\t")]
        [DataRow("\r")]
        [DataRow("\n")]
        public async Task TestReadUntilAsyncWithEmptyOrWhiteSpaceNameThrowsException(string name)
        {
            var xmlFragment = @"<root><node /></root>";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
            {
                var exception = await XmlReaderHelper.ReadUntilAsync(xmlReader, name)
                    .ContinueWith(task => task.Exception.InnerException);
                Assert.ThrowsException<ArgumentException>(() => { throw exception; });
            }
        }

        [TestMethod]
        public async Task TestReadUntilAsyncAdvancesTheReaderToTheDesiredNodeWithNamespace()
        {
            var xmlFragment = @"<root xmlns=""test""><node /></root>";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader, new XmlReaderSettings { Async = true }))
            {
                Assert.IsTrue(await XmlReaderHelper.ReadUntilAsync(xmlReader, "root", "test"));
                Assert.IsTrue(xmlReader.IsOnNode("root", "test"));
            }
        }
        [TestMethod]
        public async Task TestReadUntilAsyncWithNullReaderForNamespaceOverloadThrowsException()
        {
            var xmlFragment = @"<root xmlns=""test"" />";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
            {
                var exception = await XmlReaderHelper.ReadUntilAsync(null, "root", "test")
                    .ContinueWith(task => task.Exception.InnerException);
                Assert.ThrowsException<ArgumentNullException>(() => { throw exception; });
            }
        }
        [TestMethod]
        public async Task TestReadUntilAsyncWithNullNameButValidNamespaceThrowsException()
        {
            var xmlFragment = @"<root xmlns=""test"" />";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
            {
                var exception = await XmlReaderHelper.ReadUntilAsync(xmlReader, null, "test")
                    .ContinueWith(task => task.Exception.InnerException);
                Assert.ThrowsException<ArgumentNullException>(() => { throw exception; });
            }
        }
        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\t")]
        [DataRow("\r")]
        [DataRow("\n")]
        public async Task TestReadUntilAsyncWithEmptyOrWhiteSpaceNameButValidNamespaceThrowsException(string name)
        {
            var xmlFragment = @"<root xmlns=""test"" />";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
            {
                var exception = await XmlReaderHelper.ReadUntilAsync(xmlReader, name, "test")
                    .ContinueWith(task => task.Exception.InnerException);
                Assert.ThrowsException<ArgumentException>(() => { throw exception; });
            }
        }
        [TestMethod]
        public async Task TestReadUntilAsyncWithNullNamespaceButValidNameThrowsException()
        {
            var xmlFragment = @"<root xmlns=""test"" />";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
            {
                var exception = await XmlReaderHelper.ReadUntilAsync(xmlReader, "root", null)
                    .ContinueWith(task => task.Exception.InnerException);
                Assert.ThrowsException<ArgumentNullException>(() => { throw exception; });
            }
        }
        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\t")]
        [DataRow("\r")]
        [DataRow("\n")]
        public async Task TestReadUntilAsyncWithEmptyOrWhiteSpaceNamespaceButValidNameThrowsException(string @namespace)
        {
            var xmlFragment = @"<root xmlns=""test"" />";
            using (var stringReader = new StringReader(xmlFragment))
            using (var xmlReader = XmlReader.Create(stringReader))
            {
                var exception = await XmlReaderHelper.ReadUntilAsync(xmlReader, "root", @namespace)
                       .ContinueWith(task => task.Exception.InnerException);
                Assert.ThrowsException<ArgumentException>(() => { throw exception; });
            }
        }
    }
}