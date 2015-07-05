using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
namespace Tourist
{
    public sealed partial class PicturePage
        : Page
    {
        public PicturePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataContext = new Uri(e.Parameter as string, UriKind.RelativeOrAbsolute);
        }
    }
}