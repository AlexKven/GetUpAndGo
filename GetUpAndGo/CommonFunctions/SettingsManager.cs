using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;

namespace GetUpAndGo
{
    static class SettingsManager
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
    }
}
