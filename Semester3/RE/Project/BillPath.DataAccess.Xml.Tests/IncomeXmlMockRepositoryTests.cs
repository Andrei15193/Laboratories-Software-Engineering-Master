using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.DataAccess.Xml.Tests
{
    [TestClass]
    public class IncomeXmlMockRepositoryTests
        : IncomeXmlRepositoryTests
    {
        protected override IIncomeRepository CreateRepository()
            => new IncomeXmlMockRepository();
    }
}