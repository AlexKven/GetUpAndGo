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
    public sealed partial class CalendarSettingsPage : SettingsPageBase
    {
        public CalendarSettingsPage()
        {
            this.InitializeComponent();
        }

        protected override void LoadSettings()
        {
            AvoidAppointmentsCheckBox.IsChecked = SettingsManager.GetSetting<bool>("AvoidAppointments");
        }

        private void AvoidAppointmentsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SettingsManager.SetSetting<bool>("AvoidAppointments", true);
        }

        private void AvoidAppointmentsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SettingsManager.SetSetting<bool>("AvoidAppointments", false);
        }
    }
}
