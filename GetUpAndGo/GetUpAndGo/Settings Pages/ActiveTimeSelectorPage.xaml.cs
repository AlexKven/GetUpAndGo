using GetUpAndGo.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace GetUpAndGo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ActiveTimeSelectorPage : SettingsPageBase
    {
        public ActiveTimeSelectorPage()
        {
            this.InitializeComponent();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Func<int, GridLength> whatsTheHeight;
            switch (((RadioButton)sender).Name)
            {
                case "RadioButton0":
                    whatsTheHeight = index => index == 10 ? GridLength.Auto : new GridLength(0);
                    break;
                case "RadioButton1":
                    whatsTheHeight = index => index == 9 || index == 8 ? GridLength.Auto : new GridLength(0);
                    break;
                default:
                    whatsTheHeight = index => index < 8 ? GridLength.Auto : new GridLength(0);
                    break;
            }
            for (int i = 1; i < 11; i++)
                HoursGrid.RowDefinitions[i].Height = whatsTheHeight(i);
            if (!loading)
                SetSetting();
        }

        private void SetSetting()
        {
            int[] activeTimes = new int[28];
            Action<int, TimePicker, TimePicker> setInterval = delegate(int day, TimePicker picker1, TimePicker picker2)
            {

                TimeSpan start;
                TimeSpan end;
                if (picker1.Time < picker2.Time)
                {
                    start = picker1.Time;
                    end = picker2.Time;
                }
                else
                {
                    start = picker2.Time;
                    end = picker1.Time;
                }
                activeTimes[4 * day] = start.Hours;
                activeTimes[4 * day + 1] = start.Minutes;
                activeTimes[4 * day + 2] = end.Hours;
                activeTimes[4 * day + 3] = end.Minutes;
            };
            if (RadioButton0.IsChecked.Value)
            {
                for (int i = 0; i < 7; i++)
                    setInterval(i, StartTimePicker9, EndTimePicker9);
            }
            else if (RadioButton1.IsChecked.Value)
            {
                for (int i = 1; i < 6; i++)
                {
                    setInterval(i, StartTimePicker7, EndTimePicker7);
                }
                setInterval(0, StartTimePicker8, EndTimePicker8);
                setInterval(6, StartTimePicker8, EndTimePicker8);
            }
            else
            {
                setInterval(0, StartTimePicker0, EndTimePicker0);
                setInterval(1, StartTimePicker1, EndTimePicker1);
                setInterval(2, StartTimePicker2, EndTimePicker2);
                setInterval(3, StartTimePicker3, EndTimePicker3);
                setInterval(4, StartTimePicker4, EndTimePicker4);
                setInterval(5, StartTimePicker5, EndTimePicker5);
                setInterval(6, StartTimePicker6, EndTimePicker6);
            }
            SettingsManager.SetSetting<int[]>("ActiveIntervals", activeTimes);
        }

        private void TimePicker_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            if (!loading)
                SetSetting();
        }

        protected override void LoadSettings()
        {
            int[] activeTimes = SettingsManager.GetSetting<int[]>("ActiveIntervals");
            bool everyday = true;
            bool weekdayWeekend = true;
            for (int i = 0; i < 28; i++)
            {
                everyday &= activeTimes[i] == activeTimes[i % 4];
                if (i >= 4 && i < 24)
                    weekdayWeekend &= activeTimes[i] == activeTimes[4 + i % 4];
                else
                    weekdayWeekend &= activeTimes[i] == activeTimes[i % 4];
            }
            if (everyday)
                RadioButton0.IsChecked = true;
            else if (weekdayWeekend)
                RadioButton1.IsChecked = true;
            else
                RadioButton2.IsChecked = true;
            StartTimePicker0.Time = new TimeSpan(activeTimes[0], activeTimes[1], 0);
            EndTimePicker0.Time = new TimeSpan(activeTimes[2], activeTimes[3], 0);
            StartTimePicker1.Time = new TimeSpan(activeTimes[4], activeTimes[5], 0);
            EndTimePicker1.Time = new TimeSpan(activeTimes[6], activeTimes[7], 0);
            StartTimePicker2.Time = new TimeSpan(activeTimes[8], activeTimes[9], 0);
            EndTimePicker2.Time = new TimeSpan(activeTimes[10], activeTimes[11], 0);
            StartTimePicker3.Time = new TimeSpan(activeTimes[12], activeTimes[13], 0);
            EndTimePicker3.Time = new TimeSpan(activeTimes[14], activeTimes[15], 0);
            StartTimePicker4.Time = new TimeSpan(activeTimes[16], activeTimes[17], 0);
            EndTimePicker4.Time = new TimeSpan(activeTimes[18], activeTimes[19], 0);
            StartTimePicker5.Time = new TimeSpan(activeTimes[20], activeTimes[21], 0);
            EndTimePicker5.Time = new TimeSpan(activeTimes[22], activeTimes[23], 0);
            StartTimePicker6.Time = new TimeSpan(activeTimes[24], activeTimes[25], 0);
            EndTimePicker6.Time = new TimeSpan(activeTimes[26], activeTimes[27], 0);

            StartTimePicker7.Time = new TimeSpan(activeTimes[4], activeTimes[5], 0);
            EndTimePicker7.Time = new TimeSpan(activeTimes[6], activeTimes[7], 0);
            StartTimePicker8.Time = new TimeSpan(activeTimes[0], activeTimes[1], 0);
            EndTimePicker8.Time = new TimeSpan(activeTimes[2], activeTimes[3], 0);

            StartTimePicker9.Time = new TimeSpan(activeTimes[4], activeTimes[5], 0);
            EndTimePicker9.Time = new TimeSpan(activeTimes[6], activeTimes[7], 0);
        }
    }
}
