using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BillPath.Modern.Commands
{
    public class GoToStateCommand
        : DependencyObject, ICommand
    {
        public static readonly DependencyProperty ControlProperty =
            DependencyProperty.Register(
                nameof(Control),
                typeof(Control),
                typeof(GoToStateCommand),
                new PropertyMetadata(null));
        public static readonly DependencyProperty UseTransitionsProperty =
            DependencyProperty.Register(
                nameof(UseTransitions),
                typeof(bool?),
                typeof(GoToStateCommand),
                new PropertyMetadata(null));

        public Control Control
        {
            get
            {
                return (Control)GetValue(ControlProperty);
            }
            set
            {
                SetValue(ControlProperty, value);
                OnCanExecuteChanged(EventArgs.Empty);
            }
        }
        public bool? UseTransitions
        {
            get
            {
                return (bool?)GetValue(UseTransitionsProperty);
            }
            set
            {
                SetValue(UseTransitionsProperty, value);
            }
        }

        public void Execute(object parameter)
        {
            VisualStateManager.GoToState(
                Control,
                parameter as string ?? Control.GetDefaultState(),
                UseTransitions.GetValueOrDefault(Control.GetDefaultUseTransitions()));
        }

        public bool CanExecute(object parameter)
        {
            return (Control != null && !string.IsNullOrWhiteSpace(parameter as string ?? Control.GetDefaultState()));
        }
        public event EventHandler CanExecuteChanged;

        protected virtual void OnCanExecuteChanged(EventArgs eventArgs)
        {
            CanExecuteChanged?.Invoke(this, eventArgs);
        }
    }
}