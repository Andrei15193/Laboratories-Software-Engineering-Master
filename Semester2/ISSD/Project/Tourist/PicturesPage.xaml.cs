using System;
using System.Collections.ObjectModel;
using Tourist.Data;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
namespace Tourist
{
    public sealed partial class PicturesPage
        : Page
    {
        public PicturesPage()
        {
            ViewModel = new ObservableCollection<Uri>();
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var attractionTitle = e.Parameter as string;

            _picturesPivot.Title = "TOURIST - PICTURES OF " + attractionTitle.ToUpper();
            var attraction = await _Repository.GetAttractionByAsync(attractionTitle);

            foreach (var pictureUri in attraction.PictureUris)
                ViewModel.Add(pictureUri);
        }

        public ObservableCollection<Uri> ViewModel
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

        private void _PictureClicked(object sender, ItemClickEventArgs e)
        {
            var pictureUri = (Uri)e.ClickedItem;
            Frame.Navigate(typeof(PicturePage), pictureUri.ToString());
        }
    }
}