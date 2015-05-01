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
        }

        const string bgTaskName = "GetUpAndGoBackgroundAgent";
        IBandInfo currentBand;
        bool taskRegistered = false;
        

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
          CheckForBand();
        }

        async void CheckForBand()
        {
          MessageBlock.Text = "Looking for a Microsoft Band...";
          PinButton.IsEnabled = false;
          IBandInfo[] pairedBands = await BandClientManager.Instance.GetBandsAsync();
          if (pairedBands.Length < 1)
          {
            MessageBlock.Text = "I can't find a Microsoft Band.";
            PinButton.Content = "Try Again";
            PinButton.IsEnabled = true;
          }
          else
          {
            currentBand = pairedBands[0];
            MessageBlock.Text = "Connected to " + currentBand.Name + ".";
            PinButton.Content = "Pin Band Tile";
            PinButton.IsEnabled = true;
          }
        }

        private async void PinButton_Click(object sender, RoutedEventArgs e)
        {
          if (currentBand == null)
          {
            CheckForBand();
          }
          else
          {
            try
            {

              // Connect to Microsoft Band.
              using (IBandClient bandClient = await BandClientManager.Instance.ConnectAsync(currentBand))
              {
                // Create a Tile.
                Guid myTileId = new Guid("0D6CB82E-3206-43B6-BB7D-1B4E67A8ED43");
                BandTile myTile = new BandTile(myTileId)
                {
                  Name = "My Tile",
                  TileIcon = await LoadIcon("ms-appx:///Assets/Band/IconLarge.png"),
                  SmallIcon = await LoadIcon("ms-appx:///Assets/Band/IconSmall.png")
                };
                await bandClient.TileManager.AddTileAsync(myTile);

                // Send a notification.
                await bandClient.NotificationManager.SendMessageAsync(myTileId, "Hello", "Helly World !", DateTimeOffset.Now, MessageFlags.ShowDialog);
              }
            }
            catch (Exception ex)
            {
              //this.textBlock.Text = ex.ToString();
            }
          }
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
          foreach (var task in BackgroundTaskRegistration.AllTasks)
          {
            if (task.Value.Name == bgTaskName)
            {
              task.Value.Unregister(true);
              BackgroundExecutionManager.RemoveAccess();
              taskRegistered = true;
            }
          }
          await BackgroundExecutionManager.RequestAccessAsync();

          var builder = new BackgroundTaskBuilder();
          builder.Name = bgTaskName;
          builder.TaskEntryPoint = "GetUpAndGoBackground.BackgroundAgent";
          builder.SetTrigger(new TimeTrigger(15, false));
          var r = builder.Register();
          taskRegistered = true;
          
        }
    }
}
