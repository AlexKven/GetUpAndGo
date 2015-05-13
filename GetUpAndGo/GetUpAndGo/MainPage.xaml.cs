using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Band;
using Microsoft.Band.Notifications;
using Microsoft.Band.Tiles;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.ApplicationModel.Background;
using Windows.Media;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace GetUpAndGo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            loadFromSettings();
        }

        const string bgTaskName = "GetUpAndGoBackgroundAgent";
        IBandInfo currentBand;
        IBandClient bandClient;
        IBackgroundTaskRegistration backgroundTask;
        bool tilePinned = true;
        bool loadingFromSettings = true;

        Guid bandTileId = new Guid("0D6CB82E-3206-43B6-BB7D-1B4E67A8ED43");
        BandTile bandTile;

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (currentBand == null)
            {
                await TrySetBackgroundTask();
                bandTile = new BandTile(bandTileId)
                {
                    Name = "Walk Reminder",
                    TileIcon = await LoadIcon("ms-appx:///Assets/Band/IconLarge.png"),
                    SmallIcon = await LoadIcon("ms-appx:///Assets/Band/IconSmall.png")
                };
                await CheckForBand();
                await CheckForPinnedBandTile();
                UpdateUI();
            }
        }

        void loadFromSettings()
        {
            loadingFromSettings = true;
            int sh = (int)ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["StartHour"];
            int sm = (int)ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["StartMinute"];
            int eh = (int)ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["EndHour"];
            int em = (int)ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["EndMinute"];
            int freq = (int)ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["Frequency"];
            int thresh = (int)ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["Threshold"];
            bool avoidAppts = (bool)ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["AvoidAppointments"];
            foreach (ComboBoxItem item in FrequencyComboBox.Items)
            {
                int curItem = int.Parse(item.Tag.ToString());
                if (freq >= curItem)
                    FrequencyComboBox.SelectedItem = item;
            }
            foreach (ComboBoxItem item in ThresholdComboBox.Items)
            {
                int curItem = int.Parse(item.Tag.ToString());
                if (thresh >= curItem)
                    ThresholdComboBox.SelectedItem = item;
            }
            TimePicker1.Time = new TimeSpan(sh, sm, 0);
            TimePicker2.Time = new TimeSpan(eh, em, 0);
            AvoidAppointmentsCheckBox.IsChecked = avoidAppts;
            loadingFromSettings = false;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            //if (bandClient != null) bandClient.Dispose();
        }

        void UpdateUI()
        {
            if (backgroundTask == null)
                BackgroundTaskErrorRow.Height = GridLength.Auto;
            else
                BackgroundTaskErrorRow.Height = new GridLength(0);

            FrequencyComboBox.IsEnabled = (tilePinned && backgroundTask != null && currentBand != null);

            PinButton.IsEnabled = true;
            if (currentBand == null)
            {
                PinButton.Content = "Try Again";
            }
            else
            {
                PinButton.Content = tilePinned ? "Unpin Band Tile" : "Pin Band Tile";
            }
        }

        async Task CheckForPinnedBandTile()
        {
            bool result = false;
            if (currentBand == null)
                tilePinned = false;
            else
            {
                try
                {
                    bandClient = await BandClientManager.Instance.ConnectAsync(currentBand);
                    foreach (var tile in await bandClient.TileManager.GetTilesAsync())
                    {
                        if (tile.Name == bandTile.Name)
                        {
                            result = true;
                        }
                    }
                    bandClient.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBlock.Text = ex.Message;
                }
            }
            tilePinned = result;
        }

        async Task CheckForBand()
        {
            if (currentBand != null) return;
            MessageBlock.Text = "Looking for a Microsoft Band...";
            PinButton.IsEnabled = false;
            IBandInfo[] pairedBands = await BandClientManager.Instance.GetBandsAsync();
            if (pairedBands.Length < 1)
            {
                MessageBlock.Text = "I can't find a Microsoft Band.";
            }
            else
            {
                currentBand = pairedBands[0];
                try
                {
                    bandClient = await BandClientManager.Instance.ConnectAsync(currentBand);
                    MessageBlock.Text = "Connected to " + currentBand.Name + ".";
                    PinButton.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    currentBand = null;
                    MessageBlock.Text = ex.Message;
                }
            }
            if (bandClient != null)
            {
                bandClient.Dispose();
                bandClient = null;
            }
            UpdateUI();
        }

        void RemoveBackgroundTask()
        {
            if (backgroundTask != null)
            {
                backgroundTask.Unregister(true);
                BackgroundExecutionManager.RemoveAccess();
            }
            UpdateUI();
        }

        async Task TrySetBackgroundTask()
        {
            backgroundTask = null;
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == bgTaskName)
                {
                    backgroundTask = task.Value;
                }
            }

            if (backgroundTask == null)
            {
                var status = await BackgroundExecutionManager.RequestAccessAsync();
                if (status == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity || status == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity)
                {
                    var builder = new BackgroundTaskBuilder();
                    builder.Name = bgTaskName;
                    builder.TaskEntryPoint = "GetUpAndGoBackground.BackgroundAgent";
                    builder.SetTrigger(new TimeTrigger(15, false));
                    backgroundTask = builder.Register();
                }
            }
            UpdateUI();
        }

        private async void PinButton_Click(object sender, RoutedEventArgs e)
        {
            PinButton.IsEnabled = false;
            if (currentBand == null)
            {
                await CheckForBand();
            }
            else if (tilePinned)
            {
                try
                {
                    bandClient = await BandClientManager.Instance.ConnectAsync(currentBand);
                    if (!(await bandClient.TileManager.RemoveTileAsync(bandTile)))
                        MessageBlock.Text = "Couldn't remove tile.";
                    bandClient.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBlock.Text = ex.Message;
                }
            }
            else
            {
                try
                {
                    // Create a Tile.
                    bandClient = await BandClientManager.Instance.ConnectAsync(currentBand);
                    if (!(await bandClient.TileManager.AddTileAsync(bandTile)))
                    {
                        if (await bandClient.TileManager.GetRemainingTileCapacityAsync() == 0)
                            MessageBlock.Text = "Too many tiles are pinned.";
                        else
                            MessageBlock.Text = "Couldn't pin tile.";
                    }
                    bandClient.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBlock.Text = ex.Message;
                }
            }
            await CheckForPinnedBandTile();
            UpdateUI();
            PinButton.IsEnabled = true;
        }

        private async Task<BandIcon> LoadIcon(string uri)
        {
            StorageFile imageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(uri));

            using (IRandomAccessStream fileStream = await imageFile.OpenAsync(FileAccessMode.Read))
            {
                WriteableBitmap bitmap = new WriteableBitmap(1, 1);
                //Windows.ApplicationModel.Appointments.AppointmentManager
                await bitmap.SetSourceAsync(fileStream);
                return bitmap.ToBandIcon();
            }
        }

        private async void RegisterBackgroundAgentButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveBackgroundTask();
            await TrySetBackgroundTask();
            UpdateUI();
        }

        private void FrequencyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["Frequency"] = int.Parse(((ComboBoxItem)FrequencyComboBox.SelectedItem).Tag.ToString());
        }

        private void TimePicker_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            if (!loadingFromSettings)
            {
                TimeSpan start;
                TimeSpan end;
                if (TimePicker1.Time < TimePicker2.Time)
                {
                    start = TimePicker1.Time;
                    end = TimePicker2.Time;
                }
                else
                {
                    start = TimePicker2.Time;
                    end = TimePicker1.Time;
                }
                ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["StartHour"] = start.Hours;
                ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["StartMinute"] = start.Minutes;
                ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["EndHour"] = end.Hours;
                ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["EndMinute"] = end.Minutes;
            }
        }

        private void ThresholdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["Threshold"] = int.Parse(((ComboBoxItem)ThresholdComboBox.SelectedItem).Tag.ToString());
        }

        private void AvoidAppointmentsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["AvoidAppointments"] = true;
        }

        private void AvoidAppointmentsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["AvoidAppointments"] = false;
        }

        private void FrequencyComboBox_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (FrequencyComboBox.IsEnabled)
            {
                ErrorPopup.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            else
            {
                ErrorPopup.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }
    }
}