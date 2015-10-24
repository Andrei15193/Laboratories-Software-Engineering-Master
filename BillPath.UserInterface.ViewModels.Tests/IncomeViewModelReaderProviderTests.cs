using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BillPath.DataAccess;
using BillPath.Models;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.UserInterface.ViewModels.Tests
{
    [TestClass]
    public class IncomeViewModelReaderProviderTests
    {
        private sealed class NullReturningIncomeReaderProvider
            : IItemReaderProvider<Income>
        {
            public Task<int> GetItemCountAsync()
                => GetItemCountAsync(CancellationToken.None);
            public Task<int> GetItemCountAsync(CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public IItemReader<Income> GetReader()
                => null;
        }

        private sealed class LoggingIncomeReaderProvider
            : IItemReaderProvider<Income>
        {
            private readonly IList<string> _methodCalls = new List<string>();

            private sealed class LoggingReader
                : IItemReader<Income>
            {
                private LoggingIncomeReaderProvider _loggingIncomeReaderProvider;

                public LoggingReader(LoggingIncomeReaderProvider loggingIncomeReaderProvider)
                {
                    _loggingIncomeReaderProvider = loggingIncomeReaderProvider;
                }

                public Income Current
                {
                    get
                    {
                        _loggingIncomeReaderProvider._methodCalls.Add(nameof(Current));
                        return null;
                    }
                }

                public void Dispose()
                    => _loggingIncomeReaderProvider._methodCalls.Add(nameof(Dispose));

                public Task<bool> ReadAsync()
                    => ReadAsync(CancellationToken.None);

                public Task<bool> ReadAsync(CancellationToken cancellationToken)
                {
                    _loggingIncomeReaderProvider._methodCalls.Add(nameof(ReadAsync));
                    return Task.FromResult(false);
                }
            }

            public IEnumerable<string> MethodCalls
            {
                get
                {
                    return _methodCalls;
                }
            }

            public Task<int> GetItemCountAsync()
                => GetItemCountAsync(CancellationToken.None);

            public Task<int> GetItemCountAsync(CancellationToken cancellationToken)
            {
                _methodCalls.Add(nameof(GetItemCountAsync));
                return Task.FromResult(0);
            }

            public IItemReader<Income> GetReader()
            {
                _methodCalls.Add(nameof(GetReader));
                return new LoggingReader(this);
            }
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
        [DataRow(34)]
        [DataRow(55)]
        [DataRow(89)]
        [DataRow(144)]
        [DataRow(233)]
        public async Task TestProviderReturnsSameCountAsAdaptedIncomeReaderProvider(int incomeCount)
        {
            var incomeViewModelReaderProvider = _GetProvider(incomeCount);

            Assert.AreEqual(incomeCount, await incomeViewModelReaderProvider.GetItemCountAsync());
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(5)]
        [DataRow(8)]
        [DataRow(13)]
        [DataRow(21)]
        [DataRow(34)]
        [DataRow(55)]
        [DataRow(89)]
        [DataRow(144)]
        [DataRow(233)]
        public async Task TestReaderYieldsAsManyViewModelsAsThereAreIncomes(int incomeCount)
        {
            var incomeViewModelReader = _GetReader(incomeCount);
            for (int incomeNumber = 0; incomeNumber < incomeCount; incomeNumber++)
                Assert.IsTrue(await incomeViewModelReader.ReadAsync());

            Assert.IsFalse(await incomeViewModelReader.ReadAsync());
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(5)]
        [DataRow(8)]
        [DataRow(13)]
        [DataRow(21)]
        [DataRow(34)]
        [DataRow(55)]
        [DataRow(89)]
        [DataRow(144)]
        [DataRow(233)]
        public async Task TestReaderYieldsDistinctViewModelForEachIncome(int incomeCount)
        {
            var incomeViewModels = new List<IncomeViewModel>();

            var incomeViewModelReader = _GetReader(incomeCount);
            while (await incomeViewModelReader.ReadAsync())
                incomeViewModels.Add(incomeViewModelReader.Current);

            Assert.AreEqual(incomeViewModels.Count, incomeViewModels.Distinct().Count());
        }

        [TestMethod]
        public async Task TestReaderYieldsSameViewModelWhenCallingCurrentTwice()
        {
            var incomeViewModelReader = _GetReader(1);

            await incomeViewModelReader.ReadAsync();

            Assert.AreEqual(incomeViewModelReader.Current, incomeViewModelReader.Current);
        }

        [TestMethod]
        public void TestAccessingCurrentWithoutCallingPreviouslyCallingReadThrowsException()
            => Assert.ThrowsException<InvalidOperationException>(() => _GetReader(1).Current);

        [TestMethod]
        public async Task TestAccessingCurrentAfterReadReturnedFalseThrowsException()
        {
            var reader = _GetReader(1);

            await reader.ReadAsync();
            Assert.IsFalse(await reader.ReadAsync());

            Assert.ThrowsException<InvalidOperationException>(() => reader.Current);
        }

        [TestMethod]
        public void TestExceptionIsThrownWhenAdaptedReaderReturnsNullFromGetReader()
            => Assert.ThrowsException<ArgumentNullException>(
                () => new IncomeViewModelReaderProvider(new NullReturningIncomeReaderProvider()).GetReader());

        [TestMethod]
        public void TestProviderCannotUseNullForIncomeReaderProvider()
            => Assert.ThrowsException<ArgumentNullException>(() => new IncomeViewModelReaderProvider(null));

        [TestMethod]
        public void TestCallingDisposeOnReaderDisposesAdaptedIncomeReader()
        {
            var loggingProvider = new LoggingIncomeReaderProvider();
            var reader = new IncomeViewModelReaderProvider(loggingProvider).GetReader();

            reader.Dispose();

            Assert.IsTrue(loggingProvider.MethodCalls.Contains(nameof(reader.Dispose), StringComparer.OrdinalIgnoreCase));
        }

        private static IncomeViewModelReaderProvider _GetProvider(int incomeCount)
            => new IncomeViewModelReaderProvider(new IncomeReaderProviderMock(Enumerable.Repeat(
                new Income(),
                incomeCount)));
        private static IItemReader<IncomeViewModel> _GetReader(int incomeCount)
            => new IncomeViewModelReaderProvider(
                    new IncomeReaderProviderMock(
                        Enumerable.Repeat(
                            new Income(),
                            incomeCount)))
                .GetReader();
    }
}