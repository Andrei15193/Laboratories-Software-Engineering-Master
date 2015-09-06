using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using BillPath.DataAccess.Xml;
using BillPath.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BillPath.Tests.IncomeManagement
{
    [TestClass]
    public class IncomeRepositoryTests
    {
        private OsFileProvider _FilePorvider
        {
            get;
            set;
        }

        public TestContext TestContext
        {
            get;
            set;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _FilePorvider = new OsFileProvider
            {
                FileName = Path.Combine(TestContext.TestDir, "testFile")
            };
        }
        [TestCleanup]
        public void TestCleanup()
        {
            File.Delete(_FilePorvider.FileName);
            _FilePorvider = null;
        }

        [TestMethod]
        public async Task TestAddIncome()
        {
            var income = new Income
            {
                Amount = 10.2m,
                Currency = new Currency(new RegionInfo("RO")),
                DateRealized = DateTimeOffset.Now,
                Description = "Test description"
            };
            var incomeRepository = new XmlIncomeRepository
            {
                FileProvider = _FilePorvider
            };

            await incomeRepository.SaveAsync(income);

            using (var fileStream = await _FilePorvider.GetReadStreamAsync())
            {
                var incomeSerializer = new DataContractSerializer(typeof(Income), new[] { typeof(List<Income>) });

                Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(
                    income,
                    ((IEnumerable<Income>)incomeSerializer.ReadObject(fileStream)).Single()));
            }

        }
        [TestMethod]
        public async Task TestAddTwoIcomes()
        {
            var income1 = new Income
            {
                Amount = 10.2m,
                Currency = new Currency(new RegionInfo("RO")),
                DateRealized = DateTimeOffset.Now,
                Description = "Test description 1"
            };
            var income2 = new Income
            {
                Amount = 3.2m,
                Currency = new Currency(new RegionInfo("RO")),
                DateRealized = DateTimeOffset.Now,
                Description = "Test description 2"
            };
            var incomeRepository = new XmlIncomeRepository
            {
                FileProvider = _FilePorvider
            };

            await incomeRepository.SaveAsync(income1);
            await incomeRepository.SaveAsync(income2);

            using (var fileStream = await _FilePorvider.GetReadStreamAsync())
            {
                var incomeSerializer = new DataContractSerializer(typeof(Income), new[] { typeof(List<Income>) });
                var savedIncomes = (IEnumerable<Income>)incomeSerializer.ReadObject(fileStream);

                Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(income1, savedIncomes.ElementAt(0)));
                Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(income2, savedIncomes.ElementAt(1)));
            }
        }
    }
}