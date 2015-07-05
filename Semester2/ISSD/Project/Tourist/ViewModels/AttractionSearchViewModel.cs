using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Tourist.Data;
namespace Tourist.ViewModels
{
    public class AttractionSearchViewModel
        : ViewModel
    {
        private bool _searchTitle;
        private bool _searchTags;
        private bool _searchDescription;
        private readonly ObservableCollection<AttractionViewModel> _searchResults;

        public AttractionSearchViewModel()
        {
            _searchResults = new ObservableCollection<AttractionViewModel>();
            SearchResults = new ReadOnlyObservableCollection<AttractionViewModel>(_searchResults);
            SearchCommand = new Commands.SearchCommand(this);

            SearchTitle = true;
            SearchTags = true;
            SearchDescription = false;
        }

        public ReadOnlyObservableCollection<AttractionViewModel> SearchResults
        {
            get;
            private set;
        }

        public Commands.SearchCommand SearchCommand
        {
            get;
            private set;
        }

        public bool SearchTitle
        {
            get
            {
                return _searchTitle;
            }
            set
            {
                _searchTitle = value;
                OnPropertyChanged();
            }
        }
        public bool SearchTags
        {
            get
            {
                return _searchTags;
            }
            set
            {
                _searchTags = value;
                OnPropertyChanged();
            }
        }
        public bool SearchDescription
        {
            get
            {
                return _searchDescription;
            }
            set
            {
                _searchDescription = value;
                OnPropertyChanged();
            }
        }

        private AttractionsRepository _Repository
        {
            get
            {
                return (AttractionsRepository)App.Current.Resources["AttractionsRepository"];
            }
        }

        public static class Commands
        {
            public class SearchCommand
                : AsyncCommand
            {
                private readonly AttractionSearchViewModel _viewModel;

                public SearchCommand(AttractionSearchViewModel viewModel)
                {
                    if (viewModel == null)
                        throw new ArgumentNullException("viewModel");

                    _viewModel = viewModel;
                    _viewModel.PropertyChanged += delegate
                                                  {
                                                      CanExecute = (_viewModel.SearchTitle
                                                                    || _viewModel.SearchTags
                                                                    || _viewModel.SearchDescription);
                                                  };
                }

                protected override async Task ExecuteAsync(object parameter, CancellationToken cancellationToken)
                {
                    var searchStopwatch = new Stopwatch();
                    searchStopwatch.Start();
                    var searchPhrase = Regex.Replace(Convert.ToString(parameter), @"\W+", " ");
                    var keywords = searchPhrase.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    _viewModel._searchResults.Clear();
                    foreach (var attraction in await _viewModel._Repository.SearchForAttractionsAsync(keywords, _GetSearchOptions(), cancellationToken))
                        _viewModel._searchResults.Add(new AttractionViewModel(attraction));

                    searchStopwatch.Stop();
                    Debug.WriteLine("searchStopwatch: " + searchStopwatch.Elapsed);
                }
                private SearchOptions _GetSearchOptions()
                {
                    return new SearchOptions(_viewModel.SearchTitle,
                                             _viewModel.SearchTags,
                                             _viewModel.SearchDescription);
                }
            }
        }
    }
}