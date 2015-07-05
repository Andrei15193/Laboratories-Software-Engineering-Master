using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tourist.Data;
namespace Tourist.ViewModels
{
    public class AttractionsViewModel
        : ViewModel
    {
        private AttractionViewModel _selectedAttraction;
        private readonly ObservableCollection<AttractionViewModel> _attractionsToVisit;

        public AttractionsViewModel()
        {
            _Repository = (AttractionsRepository)App.Current.Resources["AttractionsRepository"];
            _Repository.AddedAttractionToVisit += (sender, e) => { _attractionsToVisit.Add(new AttractionViewModel(e.Attraction)); };
            _Repository.RemovedAttractionToVisit +=
                (sender, e) =>
                {
                    var attractionViewModel = _attractionsToVisit
                                             .FirstOrDefault(attractionToVisit => attractionToVisit
                                                                                 .Title
                                                                                 .Value
                                                                                 .Equals(e.Attraction.Title,
                                                                                         StringComparison.OrdinalIgnoreCase));
                    if (attractionViewModel != null)
                        _attractionsToVisit.Remove(attractionViewModel);
                };

            _selectedAttraction = null;

            _attractionsToVisit = new ObservableCollection<AttractionViewModel>();
            AttractionsToVisit = new ReadOnlyObservableCollection<AttractionViewModel>(_attractionsToVisit);

            NearByAttractions = new ReadOnlyObservableCollection<AttractionViewModel>(new ObservableCollection<AttractionViewModel>());

            UnpinToVisitCommand = new Commands.UnpinToVisitCommand(this);
            SelectAttractionCommand = new Commands.SelectAttractionCommand(this);
            LoadAttractionsToVisitCommand = new Commands.LoadAttractionsToVisitCommand(this);
            LoadNearByAttractionsCommand = new Commands.LoadNearByAttractionsCommand(this);
        }

        public AttractionViewModel SelectedAttraction
        {
            get
            {
                return _selectedAttraction;
            }
            private set
            {
                _selectedAttraction = value;
                OnPropertyChanged();
            }
        }
        public ReadOnlyObservableCollection<AttractionViewModel> AttractionsToVisit
        {
            get;
            private set;
        }
        public ReadOnlyObservableCollection<AttractionViewModel> NearByAttractions
        {
            get;
            private set;
        }

        public Commands.UnpinToVisitCommand UnpinToVisitCommand
        {
            get;
            private set;
        }
        public Commands.SelectAttractionCommand SelectAttractionCommand
        {
            get;
            private set;
        }
        public Commands.LoadAttractionsToVisitCommand LoadAttractionsToVisitCommand
        {
            get;
            private set;
        }
        public Commands.LoadNearByAttractionsCommand LoadNearByAttractionsCommand
        {
            get;
            private set;
        }

        private AttractionsRepository _Repository
        {
            get;
            set;
        }

        public static class Commands
        {
            public class UnpinToVisitCommand
                : Command<object>
            {
                private readonly AttractionsViewModel _viewModel;

                public UnpinToVisitCommand(AttractionsViewModel viewModel)
                {
                    if (viewModel == null)
                        throw new ArgumentNullException("viewModel");

                    _viewModel = viewModel;
                }

                public override bool CanExecute(object parameter)
                {
                    return true;
                }

                public override async void Execute(object parameter)
                {
                    var unpinToVisitStopwatch = new Stopwatch();
                    unpinToVisitStopwatch.Start();
                    try
                    {
                        CanExecuteCommand = false;
                        await _viewModel._Repository.RemoveAttractionToVisitAsync(((AttractionViewModel)parameter).Title.Value);
                    }
                    finally
                    {
                        CanExecuteCommand = true;
                        unpinToVisitStopwatch.Stop();
                        Debug.WriteLine("unpinToVisitStopwatch: " + unpinToVisitStopwatch.Elapsed);
                    }
                }
            }
            public class SelectAttractionCommand
                : AsyncCommand<string>
            {
                private readonly AttractionsViewModel _viewModel;

                public SelectAttractionCommand(AttractionsViewModel viewModel)
                {
                    if (viewModel == null)
                        throw new ArgumentNullException("viewModel");

                    _viewModel = viewModel;
                }

                protected override async Task ExecuteAsync(string attractionTitle, CancellationToken cancellationToken)
                {
                    var selectAttractionStopwatch = new Stopwatch();
                    selectAttractionStopwatch.Start();

                    _viewModel.SelectedAttraction = new AttractionViewModel(await _viewModel._Repository.GetAttractionByAsync(attractionTitle, cancellationToken));

                    selectAttractionStopwatch.Stop();
                    Debug.WriteLine("SelectAttraction: " + selectAttractionStopwatch.Elapsed);
                }
            }

            public class LoadAttractionsToVisitCommand
                : AsyncCommand
            {
                private readonly AttractionsViewModel _viewModel;

                public LoadAttractionsToVisitCommand(AttractionsViewModel viewModel)
                {
                    if (viewModel == null)
                        throw new ArgumentNullException("viewModel");

                    _viewModel = viewModel;
                }

                protected override async Task ExecuteAsync(object parameter, CancellationToken cancellationToken)
                {
                    var loadAttractionsToVisitStopwatch = new Stopwatch();
                    loadAttractionsToVisitStopwatch.Start();

                    _viewModel._attractionsToVisit.Clear();

                    foreach (var attractionToVisit in await _viewModel._Repository.GetAttractionsToVisitAsync(cancellationToken))
                        _viewModel._attractionsToVisit.Add(new AttractionViewModel(attractionToVisit));

                    loadAttractionsToVisitStopwatch.Stop();
                    Debug.WriteLine("LoadAttractionsToVisit: " + loadAttractionsToVisitStopwatch.Elapsed);
                }
            }
            public class LoadNearByAttractionsCommand
                : AsyncCommand
            {
                private readonly AttractionsViewModel _viewModel;

                public LoadNearByAttractionsCommand(AttractionsViewModel viewModel)
                {
                    if (viewModel == null)
                        throw new ArgumentNullException("viewModel");

                    _viewModel = viewModel;
                }

                protected override Task ExecuteAsync(object parameter, CancellationToken cancellationToken)
                {
                    return Task.FromResult(default(object));
                }
            }
        }
    }
}