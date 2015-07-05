using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tourist.Data;
using Tourist.Models;
using Windows.Devices.Geolocation;
namespace Tourist.ViewModels
{
    public class AttractionViewModel
        : DataViewModel<Attraction>
    {
        private readonly ObservableCollection<CommentViewModel> _comments;

        public AttractionViewModel()
            : this(new Attraction { Title = string.Empty, Visited = true, ContactDetails = new ContactDetails() })
        {
        }
        public AttractionViewModel(Attraction attraction)
            : base(attraction)
        {
            if (attraction == null)
                throw new ArgumentNullException("attraction");

            if (DataModel.ContactDetails != null)
                ContactDetails = new ContactDetailsViewModel(DataModel.ContactDetails);
            else
                ContactDetails = null;
            PictureUris = DataModel.PictureUris.Select(pictureUri => (Uri)pictureUri).ToList();

            Title = GetPropertyViewModel("Title",
                                         () => DataModel.Title,
                                         value => DataModel.Title = value);
            Description = GetPropertyViewModel("Description",
                                               () => DataModel.Description,
                                               value => DataModel.Description = value);
            Rating = GetPropertyViewModel("Rating",
                                          () => Convert.ToString(DataModel.Rating),
                                          value => DataModel.Rating = float.Parse(value),
                                          "The rating must be a decimal value");

            _comments = new ObservableCollection<CommentViewModel>();
            Comments = new ReadOnlyObservableCollection<CommentViewModel>(_comments);

            Tags = GetCollectionPropertyViewModel("Tags", DataModel.Tags);

            AddCommentCommand = new Commands.AddCommentCommand(this);
            PinToVisitCommand = new Commands.PinToVisitCommand(this);
            AddAttractionCommand = new Commands.AddAttractionCommand(this);
            LoadComments = new Commands.LoadCommentsCommand(this);
        }

        public bool Visited
        {
            get
            {
                return DataModel.Visited;
            }
            private set
            {
                DataModel.Visited = value;
                OnPropertyChanged();
            }
        }
        public IDataPropertyViewModel<string> Title
        {
            get;
            private set;
        }
        public IDataPropertyViewModel<string> Description
        {
            get;
            private set;
        }
        public Geopoint Coordinates
        {
            get
            {
                return new Geopoint(new BasicGeoposition
                                    {
                                        Altitude = DataModel.Coordinates.Altitude,
                                        Latitude = DataModel.Coordinates.Latitude,
                                        Longitude = DataModel.Coordinates.Longitude
                                    });
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Coordinates");

                DataModel.Coordinates.Altitude = value.Position.Altitude;
                DataModel.Coordinates.Latitude = value.Position.Latitude;
                DataModel.Coordinates.Longitude = value.Position.Longitude;

                OnPropertyChanged();
            }
        }
        public Uri ImageUri
        {
            get
            {
                return DataModel.ImageUri;
            }
            set
            {
                DataModel.ImageUri = value;
                OnPropertyChanged();
            }
        }
        public IDataPropertyViewModel<string> Rating
        {
            get;
            private set;
        }
        public ICollectionPropertyViewModel<string> Tags
        {
            get;
            private set;
        }

        public ContactDetailsViewModel ContactDetails
        {
            get;
            private set;
        }

        public ReadOnlyObservableCollection<CommentViewModel> Comments
        {
            get;
            private set;
        }

        public IReadOnlyList<Uri> PictureUris
        {
            get;
            private set;
        }

        public Commands.AddCommentCommand AddCommentCommand
        {
            get;
            private set;
        }
        public Commands.PinToVisitCommand PinToVisitCommand
        {
            get;
            private set;
        }
        public Commands.AddAttractionCommand AddAttractionCommand
        {
            get;
            private set;
        }
        public Commands.LoadCommentsCommand LoadComments
        {
            get;
            private set;
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
            public class AddCommentCommand
                : Command<object>
            {
                private readonly AttractionViewModel _viewModel;

                public AddCommentCommand(AttractionViewModel viewModel)
                {
                    if (viewModel == null)
                        throw new ArgumentNullException("viewModel");

                    _viewModel = viewModel;

                    Comment = new CommentViewModel(new Comment());
                    Comment.Author.PropertyChanged += _SetCanExecute;
                    Comment.Text.PropertyChanged += _SetCanExecute;

                    _SetCommentViewModel();
                }

                public CommentViewModel Comment
                {
                    get;
                    private set;
                }

                public override bool CanExecute(object parameter)
                {
                    return CanExecuteCommand;
                }

                public override async void Execute(object parameter)
                {
                    var addCommentStopwatch = new Stopwatch();
                    addCommentStopwatch.Start();

                    CanExecuteCommand = false;
                    var comment = new Comment
                                  {
                                      Text = Comment.Text.Value,
                                      Author = Comment.Author.Value,
                                      PostTime = Comment.PostTime
                                  };
                    await _viewModel._Repository.AddCommentToAttractionAsync(_viewModel.Title.Value, comment);
                    _viewModel._comments.Insert(0, new CommentViewModel(comment));
                    _SetCommentViewModel();

                    addCommentStopwatch.Stop();
                    Debug.WriteLine("AddComment: " + addCommentStopwatch.Elapsed);
                }

                private void _SetCanExecute(object sender, PropertyChangedEventArgs e)
                {
                    CanExecuteCommand = (!Comment.Author.Errors.Any() && !Comment.Text.Errors.Any());
                }
                private void _SetCommentViewModel()
                {
                    Comment.Author.Value = null;
                    Comment.Text.Value = null;
                    Comment.PostTime = DateTimeOffset.Now;
                }
            }
            public class PinToVisitCommand
                : Command<object>
            {
                private readonly AttractionViewModel _viewModel;

                public PinToVisitCommand(AttractionViewModel viewModel)
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
                    try
                    {
                        var addAttractionToVisitStopwatch = new Stopwatch();
                        addAttractionToVisitStopwatch.Start();

                        CanExecuteCommand = false;
                        await _viewModel._Repository.AddAttractionToVisitAsync(_viewModel.DataModel);

                        addAttractionToVisitStopwatch.Stop();
                        Debug.WriteLine("AddAttractionToVisit: " + addAttractionToVisitStopwatch.Elapsed);
                    }
                    finally
                    {
                        CanExecuteCommand = true;
                    }
                }
            }
            public class AddAttractionCommand
                : AsyncCommand
            {
                private readonly AttractionViewModel _viewModel;

                public AddAttractionCommand(AttractionViewModel viewModel)
                {
                    if (viewModel == null)
                        throw new ArgumentNullException("viewModel");

                    _viewModel = viewModel;
                    var dataErrorNotifier = _viewModel.DataModel as INotifyDataErrorInfo;
                    if (dataErrorNotifier != null)
                    {
                        dataErrorNotifier.ErrorsChanged += delegate { CanExecute = !dataErrorNotifier.HasErrors; };
                        CanExecute = !dataErrorNotifier.HasErrors;
                    }
                }

                protected override async Task ExecuteAsync(object parameter, CancellationToken cancellationToken)
                {
                    var addAttractionStopwatch = new Stopwatch();
                    addAttractionStopwatch.Start();

                    var geoposition = await new Geolocator { DesiredAccuracyInMeters = 50 }.GetGeopositionAsync().AsTask(cancellationToken);
                    _viewModel.Coordinates = geoposition.Coordinate.Point;

                    if (_viewModel.Tags.Value.Contains("Nature", StringComparer.OrdinalIgnoreCase))
                        _viewModel.ImageUri = new Uri("ms-appx:///Assets/Icons/Nature.png", UriKind.RelativeOrAbsolute);
                    else
                        _viewModel.ImageUri = new Uri("ms-appx:///Assets/Icons/Urban.png", UriKind.RelativeOrAbsolute);

                    await _viewModel._Repository.AddAttractionAsync(_viewModel.DataModel, cancellationToken);

                    addAttractionStopwatch.Stop();
                    Debug.WriteLine("AddAttraction: " + addAttractionStopwatch.Elapsed);
                }
            }
            public class LoadCommentsCommand
                : AsyncCommand
            {
                private readonly AttractionViewModel _viewModel;

                internal LoadCommentsCommand(AttractionViewModel viewModel)
                {
                    if (viewModel == null)
                        throw new ArgumentNullException("viewModel");

                    _viewModel = viewModel;
                }

                protected override async Task ExecuteAsync(object parameter, CancellationToken cancellationToken)
                {
                    var loadCommentsStopwatch = new Stopwatch();
                    loadCommentsStopwatch.Start();

                    _viewModel._comments.Clear();
                    foreach (var comment in await _viewModel._Repository.GetAttractionCommentsAsync(_viewModel.Title.Value))
                        _viewModel._comments.Add(new CommentViewModel(comment));

                    loadCommentsStopwatch.Stop();
                    Debug.WriteLine("LoadComments: " + loadCommentsStopwatch.Elapsed);
                }
            }
        }
    }
}