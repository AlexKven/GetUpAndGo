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
using GetUpAndGo.Common;
using Windows.ApplicationModel.Store;

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


            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            loadFromSettings();
        }

        #region Fields
        const string bgTaskName = "GetUpAndGoBackgroundAgent";
        IBandInfo currentBand;
        IBackgroundTaskRegistration backgroundTask;
        bool tilePinned = false;
        bool loadingFromSettings = true;
        IBandClient bandClient;

        Guid bandTileId = new Guid("0D6CB82E-3206-43B6-BB7D-1B4E67A8ED43");
        const string noTileMessage = "Click the \"Pin Band Tile\" button above to pin the tile to your Band before you continue.";
        BandTile bandTile;
        #endregion

        #region Navigation
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            if (bandClient == null)
            {
                loadingMessage = true;
                await DetectBandAndUI();
                await RefreshBackgroundTaskAndUI();
                await RefreshPinnedTileAndUI();
                loadingMessage = false;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
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
        #endregion

        #region Button Press Handlers
        private async void PinButton_Click(object sender, RoutedEventArgs e)
        {
            await TryPinTileAndUI();
        }

        private async void RefreshBandButton_Click(object sender, RoutedEventArgs e)
        {
            loadingMessage = true;
            await DetectBandAndUI();
            loadingMessage = false;
        }

        private async void RegisterBackgroundAgentButton_Click(object sender, RoutedEventArgs e)
        {
            loadingMessage = true;
            RemoveBackgroundTask();
            await RefreshBackgroundTaskAndUI();
            loadingMessage = false;
        }
        #endregion

        #region Settings Control Event Handlers
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
        #endregion

        #region UI Modifiers
        private bool _loadingMessage = false;
        private bool loadingMessage
        {
            get { return _loadingMessage; }
            set
            {
                _loadingMessage = value;
                setErrorMessage();
            }
        }

        private string _bandErrorMessage;
        private string bandErrorMessage
        {
            get { return _bandErrorMessage; }
            set
            {
                _bandErrorMessage = value;
                setErrorMessage();
            }
        }

        private string _backgroundTaskErrorMessage;
        private string backgroundTaskErrorMessage
        {
            get { return _backgroundTaskErrorMessage; }
            set
            {
                _backgroundTaskErrorMessage = value;
                setErrorMessage();
            }
        }

        private string _tileErrorMessage = noTileMessage;
        private string tileErrorMessage
        {
            get { return _tileErrorMessage; }
            set
            {
                _tileErrorMessage = value;
                setErrorMessage();
            }
        }

        private void setErrorMessage()
        {
            bool message = false;
            if (loadingMessage)
            {
                ErrorBlock.Text = "Loading...";
                message = true;
            }
            else if (bandErrorMessage != null)
            {
                ErrorBlock.Text = bandErrorMessage;
                message = true;
            }
            else if (backgroundTaskErrorMessage != null)
            {
                ErrorBlock.Text = backgroundTaskErrorMessage;
                message = true;
            }
            else if (tileErrorMessage != null)
            {
                ErrorBlock.Text = tileErrorMessage;
                message = true;
            }
            FrequencyComboBox.IsEnabled = !message;
            ErrorPopup.Visibility = message ? Visibility.Visible : Visibility.Collapsed;
            SetPinButtonFunctionality();
        }

        private async Task RefreshBackgroundTaskAndUI()
        {

            backgroundTaskErrorMessage = await TrySetBackgroundTask();
            BackgroundTaskErrorRow.Height = (backgroundTaskErrorMessage == null) ? new GridLength(0) : GridLength.Auto;
        }

        private async Task DetectBandAndUI()
        {
            string msg = await detectBand();
            if (msg == null)
                MessageBlock.Text = "Connected to " + currentBand.Name;
            else
                MessageBlock.Text = "Not connected to a Band.";
            bandErrorMessage = msg;
        }

        private async Task RefreshPinnedTileAndUI()
        {
            try
            {
                using (bandClient = await BandClientManager.Instance.ConnectAsync(currentBand))
                {
                    bandTile = (await bandClient.TileManager.GetTilesAsync()).First(t => t.TileId == bandTileId);
                    tilePinned = bandTile != null;
                    tileErrorMessage = tilePinned ? null : noTileMessage;
                }
                bandClient = null;
            }
            catch (Exception) { }
        }

        private async Task TryPinTileAndUI()
        {
            loadingMessage = true;
            tileErrorMessage = await TryPinTile();
            
            await RefreshPinnedTileAndUI();
            loadingMessage = false;
        }

        private void SetPinButtonFunctionality()
        {
            if (loadingMessage) PinButton.IsEnabled = false;
            else
            {
                PinButton.IsEnabled = true;
                if (bandErrorMessage != null)
                {
                    PinButton.Content = "Retry Finding Band";
                    PinButton.Click -= new RoutedEventHandler(PinButton_Click);
                    PinButton.Click += new RoutedEventHandler(RefreshBandButton_Click);
                }
                else if (tilePinned)
                {
                    PinButton.Content = "Tile Already Pinned";
                    PinButton.Click -= new RoutedEventHandler(PinButton_Click);
                    PinButton.Click -= new RoutedEventHandler(RefreshBandButton_Click);
                    PinButton.IsEnabled = false;
                }
                else
                {
                    PinButton.Content = "Pin Band Tile";
                    PinButton.Click -= new RoutedEventHandler(RefreshBandButton_Click);
                    PinButton.Click += new RoutedEventHandler(PinButton_Click);
                }
            }
        }
        #endregion

        #region Band Interfacing Functions
        private async Task<string> detectBand()
        {
            IBandInfo[] pairedBands = await BandClientManager.Instance.GetBandsAsync();
            if (pairedBands.Length == 0) return "No Microsoft Band is set up for this phone.";
            try
            {
                using (bandClient = await BandClientManager.Instance.ConnectAsync(pairedBands[0]))
                {
                    
                }
                bandClient = null;
            }
            catch (Exception ex)
            {
                return "Error connecting to your Microsoft Band: " + ex.Message;
            }
            currentBand = pairedBands[0];
            return null;
        }
        #endregion

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

        void RemoveBackgroundTask()
        {
            if (backgroundTask != null)
            {
                backgroundTask.Unregister(true);
                BackgroundExecutionManager.RemoveAccess();
            }
            //UpdateUI();
        }

        async Task<string> TrySetBackgroundTask()
        {
            if (backgroundTask != null)
            {
                backgroundTask.Unregister(true);
            }
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
                else
                {
                    return "Background task access status is " + status.ToString() + ". Try going into the Battery Saver app and turning off some background apps and setting Walk Reminder to \"Allowed\".";
                }
            }
            return null;
        }

        async Task<string> TryPinTile()
        {
            try
            {
                using (bandClient = await BandClientManager.Instance.ConnectAsync(currentBand))
                {
                    if (await bandClient.TileManager.GetRemainingTileCapacityAsync() == 0)
                        return "You already have the maximum number of tiles pinned to the Band. Unpin a tile in the Microsoft Health app first.";

                    // Create a Tile.
                    BandTile myTile = new BandTile(bandTileId)
                    {
                        Name = "Walk Reminder",
                        TileIcon = await LoadIcon("ms-appx:///Assets/Band/IconLarge.png"),
                        SmallIcon = await LoadIcon("ms-appx:///Assets/Band/IconSmall.png")
                    };
                    await bandClient.TileManager.AddTileAsync(myTile);
                }
                bandClient = null;
            }
            catch (Exception) { }
            return "Please wait...\nIf you didn't get a screen asking you if you want to pin a tile, try again.";
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AboutPage));
        }

        private async void RateAndReviewButton_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + CurrentApp.AppId));
        }
    }
}