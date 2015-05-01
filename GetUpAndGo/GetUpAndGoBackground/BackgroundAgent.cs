using Microsoft.Band;
using Microsoft.Band.Notifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments;
using Windows.ApplicationModel.Background;
using Windows.Storage;

namespace GetUpAndGoBackground
{
    public sealed class BackgroundAgent : IBackgroundTask
    {
      public async void Run(IBackgroundTaskInstance taskInstance)
      {
        var def = taskInstance.GetDeferral();
        EnsureSettings();
        IBandInfo[] pairedBands = await BandClientManager.Instance.GetBandsAsync();
        if (!IsInActiveTimeRange())
        {

        }
        if (pairedBands.Length > 0)
        {
          using (IBandClient bandClient = await BandClientManager.Instance.ConnectAsync(pairedBands[0]))
          {
            if (!bandClient.SensorManager.Pedometer.IsSupported)
            {
              def.Complete();
              return;
            }
            var appointmentStore = await AppointmentManager.RequestStoreAsync(AppointmentStoreAccessType.AllCalendarsReadOnly);
            var appts = (await appointmentStore.FindAppointmentsAsync(DateTime.Now - TimeSpan.FromMinutes(1), TimeSpan.FromHours(12))).Any(appt => IsDisruptiveAppointment(appt));
            Guid myTileId = new Guid("0D6CB82E-3206-43B6-BB7D-1B4E67A8ED43");
            // Send a notification.
            await bandClient.NotificationManager.SendMessageAsync(myTileId, "Background", "The background task just ran!", DateTimeOffset.Now, MessageFlags.ShowDialog);
          }
        }
        def.Complete();
      }

      bool IsDisruptiveAppointment(Appointment appt)
      {
        if (appt.AllDay) return false;
        if (appt.IsResponseRequested && appt.UserResponse == AppointmentParticipantResponse.Accepted) return false;
        if (appt.IsCanceledMeeting) return false;
        return true;
      }

      void EnsureSettings()
      {
        if (!ApplicationData.Current.LocalSettings.Containers.ContainsKey("MainContainer"))
        {
          ApplicationData.Current.LocalSettings.CreateContainer("MainContainer", ApplicationDataCreateDisposition.Always);
          ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values.Add("Version", 1);
          ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values.Add("Frequency", 15);
          ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values.Add("Threshold", 2000);
          ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values.Add("StartHour", 7);
          ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values.Add("StartMinute", 0);
          ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values.Add("EndHour", 21);
          ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values.Add("EndMinute", 0);
          ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values.Add("AvoidAppointments", true);
          ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values.Add("LastPrompt", (DateTime.Now - TimeSpan.FromHours(1)).ToString());
          ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values.Add("LastReading", -1);
        }
      }

      bool IsInActiveTimeRange()
      {
        var time = DateTime.Now;
        int sh = (int)ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["StartHour"];
        int sm = (int)ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["StartMinute"];
        int eh = (int)ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["EndHour"];
        int em = (int)ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["EndMinute"];
        if (time.Hour < sh) return false;
        if (time.Hour > eh) return false;
        if (time.Hour == sh && time.Minute < sm) return false;
        if (time.Hour == eh && time.Minute > em) return false;
        return true;
      }
    }
}
