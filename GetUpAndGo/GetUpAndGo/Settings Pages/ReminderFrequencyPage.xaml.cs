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
    public sealed partial class ReminderFrequencyPage : SettingsPageBase
    {
        public ReminderFrequencyPage()
        {
            this.InitializeComponent();
        }

        protected override void LoadSettings()
        {
            int freq = SettingsManager.GetSetting<int>("Frequency");
            foreach (ComboBoxItem item in FrequencyComboBox.Items)
            {
                int curItem = int.Parse(item.Tag.ToString());
                if (freq >= curItem)
                    FrequencyComboBox.SelectedItem = item;
            }
            NagModeCheckBox.IsChecked = SettingsManager.GetSetting<bool>("NagMode");
        }

        private void FrequencyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SettingsManager.SetSetting<int>("Frequency", int.Parse(((ComboBoxItem)FrequencyComboBox.SelectedItem).Tag.ToString()));
        }

        private void NagModeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!loading)
                SettingsManager.SetSetting<bool>("NagMode", true);
        }

        private void NagModeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!loading)
                SettingsManager.SetSetting<bool>("NagMode", false);
        }
    }
}
