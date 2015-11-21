﻿using System;
#if DEBUG
using System.Diagnostics;
using BillPath.Models;
#endif
using BillPath.Models.States.Providers;
using BillPath.UserInterface.ViewModels;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace BillPath.Modern
{
    sealed partial class App
        : Application
    {
        public App()
        {
            InitializeComponent();

            ModelStateProviders.Add(new AmountModelStateModelStateProvider<Income>());
            ModelStateProviders.Add(new AmountModelStateModelStateProvider<Expense>());

            Suspending += OnSuspending;
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (Debugger.IsAttached)
                DebugSettings.EnableFrameRateCounter = true;
#endif
            await this.GetResource<SettingsViewModel>("SettingsViewModel")?.LoadCommand.ExecuteAsync(null);

            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
                rootFrame.Navigate(typeof(DashboardPage), e.Arguments);

            Window.Current.Activate();
        }

        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            SettingsPane.GetForCurrentView().CommandsRequested += OnCommandsRequested;
        }

        private void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs settingsPaneCommandsRequestedEventArgs)
            => settingsPaneCommandsRequestedEventArgs
                .Request
                .ApplicationCommands
                .Add(new SettingsCommand(
                    "Currency",
                    Strings.SettingsFlyout.Currency_Title,
                    delegate
                    {
                        new CurrencySettingsFlyout().Show();
                    }));

        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}