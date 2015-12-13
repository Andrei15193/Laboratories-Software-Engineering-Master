using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.DataAccess.Xml.Tests
{
    [TestClass]
    public class IncomeXmlMockRepositoryTests
        : IncomeXmlRepositoryTests
    {
        protected override IIncomeXmlRepository CreateRepository()
            => new IncomeXmlMockRepository();
    }
}