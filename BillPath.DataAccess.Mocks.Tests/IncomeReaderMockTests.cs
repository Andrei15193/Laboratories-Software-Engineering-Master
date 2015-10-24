using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BillPath.Models;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.DataAccess.Mocks.Tests
{
    [TestClass]
    public class IncomeReaderMockTests
    {
        [TestMethod]
        public void TestExceptionIsThrownWhenUsingNullIncomeEnumeratorConstructorParamter()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new IncomeReaderMock(null));
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(5)]
        [DataRow(8)]
        [DataRow(13)]
        [DataRow(21)]
        public async Task TestReaderReturnsIncomesReturnedByEnumeator(int incomeCount)
        {
            var incomes = Enumerable
                .Range(
                    0,
                    incomeCount)
                .Select(incomeIndex => new Income())
                .ToList()
                .AsEnumerable();

            using (var incomeEnumerator = incomes.GetEnumerator())
            using (var incomeReader = new IncomeReaderMock(incomeEnumerator))
                while (await incomeReader.ReadAsync())
                    Assert.AreSame(
                        incomeEnumerator.Current,
                        incomeReader.Current);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(5)]
        [DataRow(8)]
        [DataRow(13)]
        [DataRow(21)]
        public void TestAdvancingEnumeratorAdvancesReader(int incomeCount)
        {
            var incomes = Enumerable
                .Range(
                    0,
                    incomeCount)
                .Select(incomeIndex => new Income())
                .ToList()
                .AsEnumerable();

            using (var incomeEnumerator = incomes.GetEnumerator())
            using (var incomeReader = new IncomeReaderMock(incomeEnumerator))
                while (incomeEnumerator.MoveNext())
                    Assert.AreSame(
                        incomeEnumerator.Current,
                        incomeReader.Current);
        }

        [TestMethod]
        public void TestCallingDisposeOnReaderCallsDisposeOnEnumerator()
        {
            var loggingEnumerator = new LoggingEnumerator();
            var reader = new IncomeReaderMock(loggingEnumerator);

            Assert.IsFalse(loggingEnumerator.DisposeCalled);

            reader.Dispose();

            Assert.IsTrue(loggingEnumerator.DisposeCalled);
        }

        private sealed class LoggingEnumerator
            : IEnumerator<Income>
        {
            public LoggingEnumerator()
            {
                DisposeCalled = false;
            }

            public Income Current
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public void Dispose()
            {
                DisposeCalled = true;
            }
            public bool DisposeCalled
            {
                get;
                private set;
            }

            public bool MoveNext()
            {
                throw new NotImplementedException();
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }
        }
    }
}