﻿using GetUpAndGo.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace GetUpAndGo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AboutPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public AboutPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
            string err = SettingsManager.GetSetting<string>("LastError");
            ErrorBlock.Text = err == null ? "" : "Last error in background task: " + err;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        private async void ResetBackgroundTaskButton_Click(object sender, RoutedEventArgs e)
        {
            IBackgroundTaskRegistration backgroundTask = null;
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == MainPage.bgTaskName)
                {
                    backgroundTask = task.Value;
                }
            }
            if (backgroundTask != null)
            {
                backgroundTask.Unregister(true);
                BackgroundExecutionManager.RemoveAccess();
            } 
            var status = await BackgroundExecutionManager.RequestAccessAsync();
            if (status == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity || status == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity)
            {
                var builder = new BackgroundTaskBuilder();
                builder.Name = MainPage.bgTaskName;
                builder.TaskEntryPoint = "GetUpAndGoBackground.BackgroundAgent";
                builder.SetTrigger(new TimeTrigger(15, false));
                backgroundTask = builder.Register();
            }
        }
    }
}