using System.Diagnostics;
using System.Threading.Tasks;
using Tourist.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;
namespace Tourist
{
    public sealed partial class MainPage
        : Page
    {
        private readonly Task _attractionsToVisitLoadingTask;
        public MainPage()
        {
            InitializeComponent();

            var attractionsToVisitStopwatch = new Stopwatch();
            attractionsToVisitStopwatch.Start();
            _attractionsToVisitLoadingTask = ((AttractionsViewModel)DataContext).LoadAttractionsToVisitCommand.ExecuteAsync(null).ContinueWith(delegate
            {
                attractionsToVisitStopwatch.Stop();
                Debug.WriteLine("attractionsToVisitStopwatch: " + attractionsToVisitStopwatch.Elapsed);
            });

            NavigationCacheMode = NavigationCacheMode.Required;
            Loaded += async delegate { await _attractionsToVisitLoadingTask; };
        }

        private void _NavigateToSearchPage(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SearchPage));
        }
        private void _NavigateToAttractionPage(object sender, ItemClickEventArgs e)
        {
            var attractionViewModel = e.ClickedItem as AttractionViewModel;
            if (attractionViewModel == null)
                Debug.WriteLine("List does not contain AttractionViewModels");
            else
                Frame.Navigate(typeof(AttractionPage), attractionViewModel.Title.Value);
        }

        private void _HoldingAttractionToVisit(object sender, Windows.UI.Xaml.Input.HoldingRoutedEventArgs e)
        {
            var senderElement = (FrameworkElement)sender;

            FlyoutBase.GetAttachedFlyout(senderElement).ShowAt(senderElement);
        }
    }
}