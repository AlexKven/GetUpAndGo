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
        long? currentReading = null;
        IBandClient currentBandClient = null;
        Guid myTileId = new Guid("0D6CB82E-3206-43B6-BB7D-1B4E67A8ED43");

        async Task<long?> GetPedometerReading()
        {
            if (currentBandClient == null) return null;
            if (currentBandClient.SensorManager.Pedometer.IsSupported)
            {
                currentReading = null;
                currentBandClient.SensorManager.Pedometer.ReadingChanged += Pedometer_ReadingChanged;
                await currentBandClient.SensorManager.Pedometer.StartReadingsAsync();
                while (currentReading == null)
                    await Task.Delay(250);
                return currentReading;
            }
            else
                return null;
        }

        async Task<long?> ConnectGetPedometerReading()
        {
            try
            {
                IBandInfo[] pairedBands = await BandClientManager.Instance.GetBandsAsync();
                currentBandClient = await BandClientManager.Instance.ConnectAsync(pairedBands[0]);
            }
            catch (Exception ex)
            {
                SettingsManager.SetSetting<string>("LastError", DateTime.Now.ToString() + " " + ex.Message);
                currentBandClient = null;
            }
            if (currentBandClient == null) return null;
            if (currentBandClient.SensorManager.Pedometer.IsSupported)
            {
                currentReading = null;
                currentBandClient.SensorManager.Pedometer.ReadingChanged += Pedometer_ReadingChanged;
                await currentBandClient.SensorManager.Pedometer.StartReadingsAsync();
                while (currentReading == null)
                    await Task.Delay(250);
                return currentReading;
            }
            else
                return null;
        }

        async Task ConnectToBand()
        {
            try
            {
                IBandInfo[] pairedBands = await BandClientManager.Instance.GetBandsAsync();
                currentBandClient = await BandClientManager.Instance.ConnectAsync(pairedBands[0]);
            }
            catch (Exception ex)
            {
                SettingsManager.SetSetting<string>("LastError", DateTime.Now.ToString() + " " + ex.Message);
                currentBandClient = null;
            }
        }

        async Task SendMessage(string title, string msg)
        {
            if (currentBandClient != null)
                await currentBandClient.NotificationManager.SendMessageAsync(myTileId, title, msg, DateTimeOffset.Now, MessageFlags.ShowDialog);
        }

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var def = taskInstance.GetDeferral();
            try
            {
                SettingsManager.EnsureSettings();
                SettingsManager.IncrementSetting("BackgroundTaskRuns");
                var now = DateTime.Now;
                Task<long?> bandTask = ConnectGetPedometerReading();
                DateTime lastPrompt = DateTime.Parse(SettingsManager.GetSetting<string>("LastPrompt"));
                DateTime lastActive = DateTime.Parse(SettingsManager.GetSetting<string>("LastActive"));
                int lastReading = SettingsManager.GetSetting<int>("LastReading");
                int threshold = SettingsManager.GetSetting<int>("Threshold");
                int frequency = SettingsManager.GetSetting<int>("Frequency");
                bool trackSteps = IsInActiveTimeRange() && await NoDisruptiveAppointments();
                if (!SettingsManager.TrialExpired)
                {
                    if (!trackSteps)
                    {
                        SettingsManager.SetSetting<int>("LastReading", -1);
                        SettingsManager.SetSetting<string>("LastActive", DateTime.Now.ToString());
                    }
                    else
                    {
                        long? currentReading = await bandTask;
                        if (SettingsManager.GetSetting<int>("NumberOfPrompts") > 3 && !SettingsManager.GetSetting<bool>("ReviewMessageSent"))
                        {
                            await SendMessage("Like Walk Reminder?", "Then rate it on the Windows Phone store. Just go into the app and tap on \"Rate/Review\". (Don't worry, this is the only time I'll buzz your wrist about this)");
                            SettingsManager.SetSetting<bool>("ReviewMessageSent", true);
                        }
                        if (currentReading == null)
                            await SendMessage("Error", "Couldn't get step count.");
                        else
                        {
                            if (SettingsManager.GetSetting<int>("LastReading") < 0)
                            {
                                SettingsManager.SetSetting<int>("LastReading", (int)currentReading.Value);
                                SettingsManager.SetSetting<string>("LastPrompt", now.ToString());
                            }
                            else
                            {
                                if (currentReading - lastReading > threshold)
                                {
                                    SettingsManager.SetSetting<int>("LastReading", (int)currentReading.Value);
                                    SettingsManager.SetSetting<string>("LastPrompt", now.ToString());
                                    SettingsManager.SetSetting<string>("LastActive", now.ToString());
                                }
                                else
                                {
                                    if ((now - lastPrompt).TotalMinutes >= frequency - 10)
                                    {
                                        // Send a notification.
                                        await SendMessage("Go for a walk!", ((int)(now - lastActive).TotalMinutes).ToString() + " minutes since you last walked.");
                                        SettingsManager.SetSetting<int>("LastReading", (int)currentReading.Value);
                                        SettingsManager.SetSetting<string>("LastPrompt", now.ToString());
                                        SettingsManager.IncrementSetting("NumberOfPrompts");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (!SettingsManager.GetSetting<bool>("TrialExpiredMessageSent"))
                    {
                        await bandTask;
                        await SendMessage("Trial Expired!", "Your trial of Walk Reminder has expired. If you would like to keep using it, purchase it for $.99 from the store.");
                        SettingsManager.SetSetting<bool>("TrialExpiredMessageSent", true);
                    }
                }
            }
            catch (Exception ex)
            {
                SettingsManager.SetSetting<string>("LastError", DateTime.Now.ToString() + " " + ex.Message);
                currentBandClient = null;
            }
            finally
            {
                if (currentBandClient != null)
                    currentBandClient.Dispose();
                
            }
            def.Complete();
        }

        public async void RunOld(IBackgroundTaskInstance taskInstance)
        {
            var def = taskInstance.GetDeferral();
            var now = DateTime.Now;
            SettingsManager.EnsureSettings();
            ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["BackgroundTaskRuns"] =
                (int)ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values["BackgroundTaskRuns"] + 1;
            IBandInfo[] pairedBands = await BandClientManager.Instance.GetBandsAsync();
            bool trackSteps = IsInActiveTimeRange();
            trackSteps = trackSteps && await NoDisruptiveAppointments();
            if (!trackSteps)
            {
                SettingsManager.SetSetting<int>("LastReading", -1);
                SettingsManager.SetSetting<string>("LastActive", DateTime.Now.ToString());
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
                                            // Send a notification.
                                            await bandClient.NotificationManager.SendMessageAsync(myTileId, "Go for a walk!", ((int)(now - lastActive).TotalMinutes).ToString() + "minutes since you last walked.", DateTimeOffset.Now, MessageFlags.ShowDialog);
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
            currentReading = e.SensorReading.TotalSteps;
            await currentBandClient.SensorManager.Pedometer.StopReadingsAsync();
        }

        async Task<bool> NoDisruptiveAppointments()
        {
            if (!SettingsManager.GetSetting<bool>("AvoidAppointments"))
                return true;
            var appointmentStore = await AppointmentManager.RequestStoreAsync(AppointmentStoreAccessType.AllCalendarsReadOnly);
            if ((await appointmentStore.FindAppointmentsAsync(DateTime.Now - TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(2))).Any(appt => IsDisruptiveAppointment(appt)))
                return false;
            return true;
        }

        bool IsDisruptiveAppointment(Appointment appt)
        {
            if (appt.AllDay || appt.Duration.TotalDays >= 1) return false;
            if (appt.IsResponseRequested && appt.UserResponse != AppointmentParticipantResponse.Accepted) return false;
            if (appt.IsCanceledMeeting) return false;
            return true;
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