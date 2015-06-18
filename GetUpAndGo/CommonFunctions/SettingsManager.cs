using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;

namespace GetUpAndGo
{
    internal class SettingsManager
    {
        public static T GetSetting<T>(string settingName)
        {
            object result = ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values[settingName];
            if (result == null) return default(T);
            return (T)result;
        }

        public static void SetSetting<T>(string settingName, T value)
        {
            if (!ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values.ContainsKey(settingName))
                ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values.Add(settingName, value);
            else
                ApplicationData.Current.LocalSettings.Containers["MainContainer"].Values[settingName] = value;
        }

        public static void IncrementSetting(string settingName)
        {
            SetSetting<int>(settingName, GetSetting<int>(settingName) + 1);
        }

        public static void EnsureSettings()
        {
            //ApplicationData.Current.LocalSettings.DeleteContainer("MainContainer");
            //SetSetting<double>("Version", 1.0);
            if (!ApplicationData.Current.LocalSettings.Containers.ContainsKey("MainContainer"))
            {
                ApplicationData.Current.LocalSettings.CreateContainer("MainContainer", ApplicationDataCreateDisposition.Always);
                SetSetting<double>("Version", 1.0);
                SetSetting<int>("Frequency", 30);
                SetSetting<int>("Threshold", 30);
                SetSetting<int>("StartHour", 7);
                SetSetting<int>("StartMinute", 0);
                SetSetting<int>("EndHour", 21);
                SetSetting<int>("EndMinute", 0);
                SetSetting<bool>("AvoidAppointments", true);
                SetSetting<string>("LastPrompt", DateTime.Now.ToString());
                SetSetting<string>("LastActive", DateTime.Now.ToString());
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
            if (GetSetting<double>("Version") < 1.1)
            {
                SetSetting<double>("Version", 1.1);
                SetSetting<int>("ApplicationRuns", 0);
                SetSetting<int>("BackgroundTaskRuns", 0);
                SetSetting<int>("NumberOfPrompts", 0);
                SetSetting<double>("LastVersionRun", 1.0);
            }
        }
    }
}
