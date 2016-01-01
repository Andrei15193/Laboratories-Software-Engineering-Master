using System;
using System.Collections.Generic;
using System.Linq;
using BillPath.Models;
using Windows.UI.Xaml.Controls;

namespace BillPath.Modern.Controls.Edit
{
    public sealed partial class ExpenseView
        : UserControl
    {
        public ExpenseView()
        {
            this.InitializeComponent();
        }

        private void _SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var categoryCombo = (ComboBox)sender;

            var index = (categoryCombo.ItemsSource as IEnumerable<ModelState>)
                .TakeWhile(modelState => string.Equals(
                    (string)((ModelState)categoryCombo.SelectedItem)[nameof(ExpenseCategory.Name)],
                    (string)modelState[nameof(ExpenseCategory.Name)],
                    StringComparison.OrdinalIgnoreCase))
                .Count();

            if (index < (categoryCombo.ItemsSource as IEnumerable<ModelState>).Count())
                categoryCombo.SelectedIndex = index;
        }
    }
}