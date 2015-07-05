using System;
using System.Linq;
using Tourist.ViewModels;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
namespace Tourist
{
    public sealed partial class AddAttractionPage
        : Page
    {
        public AddAttractionPage()
        {
            InitializeComponent();
            Loaded += delegate
                      {
                          _ViewModel.AddAttractionCommand.PropertyChanged += (sender, e) =>
                              {
                                  if ("Executing".Equals(e.PropertyName, StringComparison.OrdinalIgnoreCase)
                                      && !_ViewModel.AddAttractionCommand.Executing
                                      && Frame.CanGoBack)
                                      Frame.GoBack();
                              };

                          _ViewModel.AddAttractionCommand.Faulted += async (sender, e) =>
                                                                     {
                                                                         await new MessageDialog(e.Exception.Message, "Error").ShowAsync();
                                                                     };
                      };
        }

        private void _TagsMenuFlyoutOpening(object sender, object e)
        {
            var tagsMenuFlyout = (MenuFlyout)sender;
            tagsMenuFlyout.Items.Clear();

            if (!_ViewModel.Tags.Value.Any())
            {
                var natureMenuItem = new MenuFlyoutItem { Text = "Nature" };
                natureMenuItem.Click += delegate
                                        {
                                            _ViewModel.Tags.AddItemCommand.Execute("Nature");
                                        };
                var urbanMenuItem = new MenuFlyoutItem { Text = "Urban" };
                urbanMenuItem.Click += delegate
                                       {
                                           _ViewModel.Tags.AddItemCommand.Execute("Urban");
                                       };

                tagsMenuFlyout.Items.Add(natureMenuItem);
                tagsMenuFlyout.Items.Add(urbanMenuItem);
            }
            else
            {
                if (_ViewModel.Tags.Value.Contains("Nature", StringComparer.OrdinalIgnoreCase))
                {
                    var waterfallMenuItem = new MenuFlyoutItem { Text = "Waterfall" };
                    waterfallMenuItem.Click += delegate
                                               {
                                                   _ViewModel.Tags.AddItemCommand.Execute("Waterfall");
                                               };

                    var gardensMenuItem = new MenuFlyoutItem { Text = "Botanical Garden" };
                    gardensMenuItem.Click += delegate
                                             {
                                                 _ViewModel.Tags.AddItemCommand.Execute("Botanical Garden");
                                             };

                    if (!_ViewModel.Tags.Value.Contains(waterfallMenuItem.Text, StringComparer.OrdinalIgnoreCase))
                        tagsMenuFlyout.Items.Add(waterfallMenuItem);
                    if (!_ViewModel.Tags.Value.Contains(gardensMenuItem.Text, StringComparer.OrdinalIgnoreCase))
                        tagsMenuFlyout.Items.Add(gardensMenuItem);
                }
                else
                {
                    var mirrorBuildingsMenuItem = new MenuFlyoutItem { Text = "Mirror Buildings" };
                    mirrorBuildingsMenuItem.Click += delegate
                                                     {
                                                         _ViewModel.Tags.AddItemCommand.Execute("Mirror Buildings");
                                                     };

                    var statueMenuItem = new MenuFlyoutItem { Text = "Statue" };
                    statueMenuItem.Click += delegate
                                            {
                                                _ViewModel.Tags.AddItemCommand.Execute("Statue");
                                            };

                    if (!_ViewModel.Tags.Value.Contains(mirrorBuildingsMenuItem.Text, StringComparer.OrdinalIgnoreCase))
                        tagsMenuFlyout.Items.Add(mirrorBuildingsMenuItem);
                    if (!_ViewModel.Tags.Value.Contains(statueMenuItem.Text, StringComparer.OrdinalIgnoreCase))
                        tagsMenuFlyout.Items.Add(statueMenuItem);
                }
            }
        }

        private AttractionViewModel _ViewModel
        {
            get
            {
                return (AttractionViewModel)DataContext;
            }
        }

        private void _ClearTagsClicked(object sender, RoutedEventArgs e)
        {
            _ViewModel.Tags.ClearItemsCommand.Execute(null);
        }
    }
}