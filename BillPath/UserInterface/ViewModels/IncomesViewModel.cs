using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BillPath.DataAccess;
using BillPath.Models;

namespace BillPath.UserInterface.ViewModels
{
    public class IncomesViewModel
        : ViewModel
    {
        private readonly IIncomeRepository _repository;
        private readonly ObservableCollection<int> _pageRange;
        private IEnumerable<Income> _selectedPage;
        private int? _selectedPageNumber;

        public IncomesViewModel(IIncomeRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _repository = repository;
            _pageRange = new ObservableCollection<int>();
            _selectedPage = null;
            _selectedPageNumber = null;

            LoadPageInfoCommand = new DelegateAsyncCommand(
                (parameter, cancellationToken) => _LoadPageInfo(cancellationToken));
            SelectPageCommand = new DelegateAsyncCommand<int>(_SelectPage);
            AddIncomeCommand = new DelegateAsyncCommand<Income>(_AddIncome);

            PageRange = new ReadOnlyObservableCollection<int>(_pageRange);
            TotalAmounts = new Amount[0];
        }

        private async Task _LoadPageInfo(CancellationToken cancellationToken)
        {
            var pageCount = await _repository.GetPageCountAsync(cancellationToken);

            var amountsByCurrency = new ConcurrentDictionary<Currency, Amount>();
            await Task.WhenAll(
                from pageNumber in Enumerable.Range(1, pageCount)
                select _repository
                    .GetOnPageAsync(pageNumber, cancellationToken)
                    .ContinueWith(incomesTask =>
                    {
                        foreach (var amount in from income in incomesTask.Result
                                                     select income.Amount)
                            amountsByCurrency.AddOrUpdate(
                                amount.Currency,
                                amount,
                                (currency, totalAmount) => totalAmount + amount);
                    }));
            TotalAmounts = amountsByCurrency.Values.ToList();

            if (_pageRange.Count != pageCount)
            {
                while (_pageRange.Count < pageCount)
                    _pageRange.Add(_pageRange.Count + 1);
                while (_pageRange.Count > pageCount)
                    _pageRange.RemoveAt(_pageRange.Count - 1);
            }
        }
        private async Task _SelectPage(int pageNumber, CancellationToken cancellationToken)
        {
            SelectedPage = await _repository.GetOnPageAsync(pageNumber, cancellationToken);
            SelectedPageNumber = pageNumber;
        }
        private async Task _AddIncome(Income income, CancellationToken cancellationToken)
        {
            await _repository.SaveAsync(income, cancellationToken);
            await _LoadPageInfo(cancellationToken);
        }

        public IEnumerable<Amount> TotalAmounts { get; private set; }

        public IEnumerable<Income> SelectedPage
        {
            get
            {
                return (_selectedPage ?? Enumerable.Empty<Income>());
            }
            private set
            {
                _selectedPage = value;
                OnPropertyChanged();
            }
        }
        public int? SelectedPageNumber
        {
            get
            {
                return _selectedPageNumber;
            }
            private set
            {
                _selectedPageNumber = value;
                OnPropertyChanged();
            }
        }
        public AsyncCommand<int> SelectPageCommand
        {
            get;
        }

        public AsyncCommand LoadPageInfoCommand
        {
            get;
        }

        public AsyncCommand<Income> AddIncomeCommand
        {
            get;
        }

        public ReadOnlyObservableCollection<int> PageRange
        {
            get;
        }
    }
}