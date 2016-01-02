using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BillPath.Modern
{
    public static class VisualStateHelper
    {
        public static readonly DependencyProperty DefaultUseTransitionsProperty =
            DependencyProperty.RegisterAttached(
                "DefaultUseTransitions",
                typeof(bool),
                typeof(Control),
                new PropertyMetadata(true));
        public static bool GetDefaultUseTransitions(this Control control)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            return (bool)control.GetValue(DefaultUseTransitionsProperty);
        }
        public static void SetDefaultUseTransitions(this Control control, bool useTransitions)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            control.SetValue(DefaultUseTransitionsProperty, useTransitions);
        }

        public static readonly DependencyProperty DefaultStateProperty =
            DependencyProperty.RegisterAttached(
                "DefaultState",
                typeof(string),
                typeof(Control),
                new PropertyMetadata(null));
        public static string GetDefaultState(this Control control)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            return (string)control.GetValue(DefaultStateProperty);
        }
        public static void SetDefaultState(this Control control, string visualState)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            if (!VisualStateManager.GoToState(control, visualState, GetDefaultUseTransitions(control)))
                control.Loaded += delegate
                {
                    VisualStateManager.GoToState(control, visualState, GetDefaultUseTransitions(control));
                };

            control.SetValue(DefaultStateProperty, visualState);
        }
    }
}