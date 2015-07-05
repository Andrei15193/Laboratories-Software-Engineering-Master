using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tourist.Data;
using Tourist.ViewModels;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Navigation;
namespace Tourist
{
    public sealed partial class AttractionPage
        : Page
    {
        private readonly IDictionary<string, Func<Task>> _pivotLoadActions;

        public AttractionPage()
        {
            InitializeComponent();

            _pivotLoadActions = new Dictionary<string, Func<Task>>(StringComparer.OrdinalIgnoreCase);
            _pivotLoadActions.Add("comments", async () => await _ViewModel.SelectedAttraction.LoadComments.ExecuteAsync(null));
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var attractionTitle = e.Parameter as string;
            if (attractionTitle == null)
                await new MessageDialog("There is an error processing your request", "Error").ShowAsync();
            else
            {
                _attractionPivot.Title = "TOURIST - " + attractionTitle.ToUpper();
                await _ViewModel.SelectAttractionCommand.ExecuteAsync(attractionTitle);

                _map.Center = _ViewModel.SelectedAttraction.Coordinates;
                _map.MapElements.Clear();
                _map.MapElements.Add(new MapIcon
                                     {
                                         Title = _ViewModel.SelectedAttraction.Title.Value,
                                         Image = RandomAccessStreamReference.CreateFromUri(_ViewModel.SelectedAttraction.ImageUri),
                                         Location = _ViewModel.SelectedAttraction.Coordinates
                                     });
            }
        }

        private AttractionsRepository _Repository
        {
            get
            {
                return (AttractionsRepository)App.Current.Resources["AttractionsRepository"];
            }
        }

        private AttractionsViewModel _ViewModel
        {
            get
            {
                return (AttractionsViewModel)DataContext;
            }
        }

        private void _AddCommentButtonClick(object sender, RoutedEventArgs e)
        {
            _addCommentFlyout.Hide();
            _attractionPivot.SelectedIndex = 1;
        }
        private void _ViewPicturesButtonClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PicturesPage), _ViewModel.SelectedAttraction.Title.Value);
        }

        private async void _LoadPivot(Pivot sender, PivotItemEventArgs args)
        {
            Func<Task> pivotLoadAction;
            var pivotHeader = args.Item.Header as string;
            if (pivotHeader != null && _pivotLoadActions.TryGetValue(pivotHeader, out pivotLoadAction))
            {
                await pivotLoadAction();
                _pivotLoadActions.Remove(pivotHeader);
            }
        }
    }
}