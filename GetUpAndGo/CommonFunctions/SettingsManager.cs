﻿using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Store;
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

        public static void SetRoamingSetting<T>(string settingName, T value)
        {
            if (!ApplicationData.Current.RoamingSettings.Containers["MainContainer"].Values.ContainsKey(settingName))
                ApplicationData.Current.RoamingSettings.Containers["MainContainer"].Values.Add(settingName, value);
            else
                ApplicationData.Current.RoamingSettings.Containers["MainContainer"].Values[settingName] = value;
        }
        public static T GetRoamingSetting<T>(string settingName)
        {
            object result = ApplicationData.Current.RoamingSettings.Containers["MainContainer"].Values[settingName];
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
            //SetSetting<double>("Version", 1.1);
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
            if (GetSetting<double>("Version") < 1.2)
            {
                SetSetting<double>("Version", 1.2);
                SetSetting<bool>("TrialExpiredMessageSent", false);
                SetSetting<bool>("ReviewMessageSent", false);
            }
            if (!ApplicationData.Current.RoamingSettings.Containers.ContainsKey("MainContainer"))
                ApplicationData.Current.RoamingSettings.CreateContainer("MainContainer", ApplicationDataCreateDisposition.Always);
            if (GetRoamingSetting<string>("TrialExpiration") == null)
                SetRoamingSetting<string>("TrialExpiration", new DateTime(1, 1, 1).ToString());
        }

        public static bool TrialExpired
        {
            get
            {
                return DateTime.Parse(SettingsManager.GetRoamingSetting<string>("TrialExpiration")) < DateTime.Now;
            }
        }

        public static void RefreshTrial()
        {
            if (CurrentAppSimulator.LicenseInformation.IsTrial)
            {
                if (DateTime.Parse(SettingsManager.GetRoamingSetting<string>("TrialExpiration")).Year == 1)
                    SettingsManager.SetRoamingSetting<string>("TrialExpiration", (DateTime.Now + TimeSpan.FromDays(3)).ToString());
            }
            else
            {
                SettingsManager.SetRoamingSetting<string>("TrialExpiration", new DateTime(9999, 12, 31).ToString());
            }
        }
    }
}