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
using Windows.UI.Popups;
using GetUpAndGo;
using Microsoft.Band.Sensors;

namespace GetUpAndGoBackground
{
    public sealed class BackgroundAgent : IBackgroundTask
    {
        int? currentReading = null;
        IBandClient currentBandClient = null;

        async Task<IBandClient> GetBandClient()
        {
            try
            {
                IBandInfo[] pairedBands = await BandClientManager.Instance.GetBandsAsync();
                if (pairedBands.Length > 0)
                {
                    return await BandClientManager.Instance.ConnectAsync(pairedBands[0]);
                }
            }
            catch (Exception) { }
            return null;
        }

        async Task<long?> GetPedometerReading()
        {
            if (currentBandClient == null) return null;
            if (currentBandClient.SensorManager.Pedometer.IsSupported)
            {
                long? result = null;
                EventHandler<BandSensorReadingEventArgs<IBandPedometerReading>> eventHandler = null;
                eventHandler = async (s, e) =>
                {
                    await currentBandClient.SensorManager.Pedometer.StopReadingsAsync();
                    result = e.SensorReading.TotalSteps;
                    currentBandClient.SensorManager.Pedometer.ReadingChanged -= eventHandler;
                };
                currentBandClient.SensorManager.Pedometer.ReadingChanged += eventHandler;
                while (result == null)
                    await Task.Delay(250);
                return result;
            }
            else
                return null;
        }

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var def = taskInstance.GetDeferral();
            MessageDialog d = new MessageDialog("Test");
            //d.ShowAsync();
            //Debug.WriteLine("Background task started.");
            var now = DateTime.Now;
            EnsureSettings();
            ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["BackgroundTaskRuns"] =
                (int)ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["BackgroundTaskRuns"] + 1;
            IBandInfo[] pairedBands = await BandClientManager.Instance.GetBandsAsync();
            bool trackSteps = IsInActiveTimeRange();
            if ((bool)ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["AvoidAppointments"])
            {
                var appointmentStore = await AppointmentManager.RequestStoreAsync(AppointmentStoreAccessType.AllCalendarsReadOnly);
                if ((await appointmentStore.FindAppointmentsAsync(DateTime.Now - TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(2))).Any(appt => IsDisruptiveAppointment(appt)))
                    trackSteps = false;
            }
            if (!trackSteps)
            {
                ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["LastReading"] = -1;
            }
            else
            {
                if (pairedBands.Length > 0)
                {
                    using (IBandClient bandClient = await BandClientManager.Instance.ConnectAsync(pairedBands[0]))
                    {
                        //Debug.WriteLine("Band found in background.");
                        currentBandClient = bandClient;
                        if (bandClient.SensorManager.Pedometer.IsSupported)
                        {
                            bandClient.SensorManager.Pedometer.ReadingChanged += Pedometer_ReadingChanged;
                            if (await bandClient.SensorManager.Pedometer.StartReadingsAsync())
                            {
                                while (currentReading == null)
                                {
                                    await Task.Delay(250);
                                }
                                if ((int)ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["LastReading"] < 0)
                                {
                                    ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["LastReading"] = currentReading.Value;
                                    ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["LastPrompt"] = now.ToString();
                                }
                                else
                                {
                                    DateTime lastPrompt = DateTime.Parse((string)ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["LastPrompt"]);
                                    DateTime lastActive = DateTime.Parse((string)ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["LastActive"]);
                                    int lastReading = (int)ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["LastReading"];
                                    int threshold = (int)ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["Threshold"];
                                    int frequency = (int)ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["Frequency"];
                                    if (currentReading - lastReading > threshold)
                                    {
                                        ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["LastReading"] = currentReading.Value;
                                        ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["LastPrompt"] = now.ToString();
                                        ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["LastActive"] = now.ToString();
                                    }
                                    else
                                    {
                                        if ((now - lastPrompt).TotalMinutes >= frequency - 10)
                                        {
                                            Guid myTileId = new Guid("0D6CB82E-3206-43B6-BB7D-1B4E67A8ED43");
                                            // Send a notification.
                                            await bandClient.NotificationManager.SendMessageAsync(myTileId, "Go for a walk!", "You haven't moved in " + ((int)(now - lastActive).TotalMinutes).ToString() + " minutes.", DateTimeOffset.Now, MessageFlags.ShowDialog);
                                            ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["LastReading"] = currentReading.Value;
                                            ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["LastPrompt"] = now.ToString();
                                            ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["NumberOfPrompts"] =
                                                (int)ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["NumberOfPrompts"] + 1;
                                        }
                                    }
                                }
                            }
                        }

                        currentBandClient = null;
                    }
                }
            }
            //Debug.WriteLine("Background task exited.");
            def.Complete();
        }

        async void Pedometer_ReadingChanged(object sender, Microsoft.Band.Sensors.BandSensorReadingEventArgs<Microsoft.Band.Sensors.IBandPedometerReading> e)
        {
            currentReading = (int)e.SensorReading.TotalSteps;
            await currentBandClient.SensorManager.Pedometer.StopReadingsAsync();
            currentBandClient.SensorManager.Pedometer.ReadingChanged -= Pedometer_ReadingChanged;
        }

        bool IsDisruptiveAppointment(Appointment appt)
        {
            if (appt.AllDay || appt.Duration.TotalDays >= 1) return false;
            if (appt.IsResponseRequested && appt.UserResponse != AppointmentParticipantResponse.Accepted) return false;
            if (appt.IsCanceledMeeting) return false;
            return true;
        }

        public static void EnsureSettings()
        {
            //ApplicationData.Current.LocalSettings.DeleteContainer("MainContainer");
            //SetSetting<double>("Version", 1.0);
            if (!ApplicationData.Current.LocalSettings.Containers.ContainsKey("MainContainer"))
            {
                ApplicationData.Current.LocalSettings.CreateContainer("MainContainer", ApplicationDataCreateDisposition.Always);
                SettingsManager.SetSetting<double>("Version", 1.0);
                SettingsManager.SetSetting<int>("Frequency", 30);
                SettingsManager.SetSetting<int>("Threshold", 30);
                SettingsManager.SetSetting<int>("StartHour", 7);
                SettingsManager.SetSetting<int>("StartMinute", 0);
                SettingsManager.SetSetting<int>("EndHour", 21);
                SettingsManager.SetSetting<int>("EndMinute", 0);
                SettingsManager.SetSetting<bool>("AvoidAppointments", true);
                SettingsManager.SetSetting<string>("LastPrompt", DateTime.Now.ToString());
                SettingsManager.SetSetting<string>("LastActive", DateTime.Now.ToString());
                //ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values.Add("Version", 1.0);
                //ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values.Add("Frequency", 30);
                //ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values.Add("Threshold", 30);
                //ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values.Add("StartHour", 7);
                //ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values.Add("StartMinute", 0);
                //ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values.Add("EndHour", 21);
                //ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values.Add("EndMinute", 0);
                //ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values.Add("AvoidAppointments", true);
                //ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values.Add("LastPrompt", DateTime.Now.ToString());
                //ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values.Add("LastActive", DateTime.Now.ToString());
                //ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values.Add("LastReading", -1);
            }
            if (SettingsManager.GetSetting<double>("Version") < 1.1)
            {
                SettingsManager.SetSetting<double>("Version", 1.1);
                SettingsManager.SetSetting<int>("ApplicationRuns", 0);
                SettingsManager.SetSetting<int>("BackgroundTaskRuns", 0);
                SettingsManager.SetSetting<int>("NumberOfPrompts", 0);
                SettingsManager.SetSetting<double>("LastVersionRun", 1.0);
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