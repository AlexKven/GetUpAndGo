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
    public sealed partial class StepQuotaPage : SettingsPageBase
    {
        public StepQuotaPage()
        {
            this.InitializeComponent();
        }

        private void ThresholdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SettingsManager.SetSetting<int>("Threshold", int.Parse(((ComboBoxItem)ThresholdComboBox.SelectedItem).Tag.ToString()));
        }

        protected override void LoadSettings()
        {
            int thresh = SettingsManager.GetSetting<int>("Threshold");
            foreach (ComboBoxItem item in ThresholdComboBox.Items)
            {
                int curItem = int.Parse(item.Tag.ToString());
                if (thresh >= curItem)
                    ThresholdComboBox.SelectedItem = item;
            }
        }
    }
}
