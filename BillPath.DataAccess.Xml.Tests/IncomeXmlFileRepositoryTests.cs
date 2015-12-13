using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.Storage;

namespace BillPath.DataAccess.Xml.Tests
{
    [TestClass]
    public class IncomeXmlFileRepositoryTests
        : IncomeXmlRepositoryTests
    {
        private string _fileName;

        protected override async Task OnTestInitializingAsync()
        {
            await Task.Yield();
            _fileName = Guid.NewGuid().ToString() + ".xml";
        }

        protected override IIncomeXmlRepository CreateRepository()
            => new IncomeXmlFileRepository(_fileName);

        protected override async Task OnTestCleanedUpAsync()
        {
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(_fileName, CreationCollisionOption.OpenIfExists);
            await file.DeleteAsync();
        }

        [TestMethod]
        public void TestExceptionIsThrownWhenTryingToCreateRepositoryWithNullFileName()
            => Assert.ThrowsException<ArgumentNullException>(() => new IncomeXmlFileRepository(null));
        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\t")]
        [DataRow("\n")]
        [DataRow("\r")]
        public void TestExceptionIsThrownWhenTryingToCreateRepositoryWithEmptyOrWhiteSpaceFileName(string fileName)
            => Assert.ThrowsException<ArgumentException>(() => new IncomeXmlFileRepository(fileName));
    }
}