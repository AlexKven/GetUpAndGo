﻿

#pragma checksum "C:\Users\Alexander\Documents\GitHub\GetUpAndGo\GetUpAndGo\GetUpAndGo\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "CD663CBAD1F7FDC1F3AD6B26FAB3C808"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GetUpAndGo
{
    partial class MainPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 102 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.AboutButton_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 103 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.RateAndReviewButton_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 91 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target)).Checked += this.AvoidAppointmentsCheckBox_Checked;
                 #line default
                 #line hidden
                #line 91 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target)).Unchecked += this.AvoidAppointmentsCheckBox_Unchecked;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 94 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.RegisterBackgroundAgentButton_Click;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 87 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.TimePicker)(target)).TimeChanged += this.TimePicker_TimeChanged;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 89 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.TimePicker)(target)).TimeChanged += this.TimePicker_TimeChanged;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 67 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.ThresholdComboBox_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 8:
                #line 52 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.FrequencyComboBox_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 9:
                #line 34 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.RegisterBackgroundAgentButton_Click;
                 #line default
                 #line hidden
                break;
            case 10:
                #line 29 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.PinButton_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


