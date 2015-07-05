using System.Diagnostics;
using Tourist.ViewModels;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
namespace Tourist
{
    public sealed partial class SearchPage
        : Page
    {
        public SearchPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;

            App.Current.Suspending += delegate { _CancelSearch(); };
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _CancelSearch();
            base.OnNavigatedFrom(e);
        }

        private async void _SearchTextBoxKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                e.Handled = true;
                _LoseFocus(sender as Control);

                _CancelSearch();
                await _SearchCommand.ExecuteAsync(_searchTextBox.Text);
            }
        }

        private void _AttractionClicked(object sender, ItemClickEventArgs e)
        {
            var attractionViewModel = e.ClickedItem as AttractionViewModel;
            if (attractionViewModel == null)
                Debug.WriteLine("List does not contain AttractionViewModels");
            else
                Frame.Navigate(typeof(AttractionPage), attractionViewModel.Title.Value);
        }
        private void _LoseFocus(Control control)
        {
            if (control != null)
            {
                var isTabStop = control.IsTabStop;

                control.IsTabStop = false;
                control.IsEnabled = false;
                control.IsEnabled = true;
                control.IsTabStop = isTabStop;
            }
        }
        private void _AddAttractionClicked(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddAttractionPage));
        }

        private AttractionSearchViewModel _ViewModel
        {
            get
            {
                return (AttractionSearchViewModel)DataContext;
            }
        }
        private AsyncCommand _SearchCommand
        {
            get
            {
                return _ViewModel.SearchCommand;
            }
        }
        private void _CancelSearch()
        {
            if (_SearchCommand.CancelCommand.CanExecute(null))
                _SearchCommand.CancelCommand.Execute(null);
        }
    }
}