﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------



namespace GetUpAndGo
{
    public partial class App : global::Windows.UI.Xaml.Markup.IXamlMetadataProvider
    {
        private global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlTypeInfoProvider _provider;

        public global::Windows.UI.Xaml.Markup.IXamlType GetXamlType(global::System.Type type)
        {
            if(_provider == null)
            {
                _provider = new global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlTypeInfoProvider();
            }
            return _provider.GetXamlTypeByType(type);
        }

        public global::Windows.UI.Xaml.Markup.IXamlType GetXamlType(string fullName)
        {
            if(_provider == null)
            {
                _provider = new global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlTypeInfoProvider();
            }
            return _provider.GetXamlTypeByName(fullName);
        }

        public global::Windows.UI.Xaml.Markup.XmlnsDefinition[] GetXmlnsDefinitions()
        {
            return new global::Windows.UI.Xaml.Markup.XmlnsDefinition[0];
        }
    }
}

namespace GetUpAndGo.GetUpAndGo_XamlTypeInfo
{
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks", "4.0.0.0")]    
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    internal partial class XamlTypeInfoProvider
    {
        public global::Windows.UI.Xaml.Markup.IXamlType GetXamlTypeByType(global::System.Type type)
        {
            global::Windows.UI.Xaml.Markup.IXamlType xamlType;
            if (_xamlTypeCacheByType.TryGetValue(type, out xamlType))
            {
                return xamlType;
            }
            int typeIndex = LookupTypeIndexByType(type);
            if(typeIndex != -1)
            {
                xamlType = CreateXamlType(typeIndex);
            }
            var userXamlType = xamlType as global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlUserType;
            if(xamlType == null || (userXamlType != null && userXamlType.IsReturnTypeStub && !userXamlType.IsLocalType))
            {
                global::Windows.UI.Xaml.Markup.IXamlType libXamlType = CheckOtherMetadataProvidersForType(type);
                if (libXamlType != null)
                {
                    if(libXamlType.IsConstructible || xamlType == null)
                    {
                        xamlType = libXamlType;
                    }
                }
            }
            if (xamlType != null)
            {
                _xamlTypeCacheByName.Add(xamlType.FullName, xamlType);
                _xamlTypeCacheByType.Add(xamlType.UnderlyingType, xamlType);
            }
            return xamlType;
        }

        public global::Windows.UI.Xaml.Markup.IXamlType GetXamlTypeByName(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                return null;
            }
            global::Windows.UI.Xaml.Markup.IXamlType xamlType;
            if (_xamlTypeCacheByName.TryGetValue(typeName, out xamlType))
            {
                return xamlType;
            }
            int typeIndex = LookupTypeIndexByName(typeName);
            if(typeIndex != -1)
            {
                xamlType = CreateXamlType(typeIndex);
            }
            var userXamlType = xamlType as global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlUserType;
            if(xamlType == null || (userXamlType != null && userXamlType.IsReturnTypeStub && !userXamlType.IsLocalType))
            {
                global::Windows.UI.Xaml.Markup.IXamlType libXamlType = CheckOtherMetadataProvidersForName(typeName);
                if (libXamlType != null)
                {
                    if(libXamlType.IsConstructible || xamlType == null)
                    {
                        xamlType = libXamlType;
                    }
                }
            }
            if (xamlType != null)
            {
                _xamlTypeCacheByName.Add(xamlType.FullName, xamlType);
                _xamlTypeCacheByType.Add(xamlType.UnderlyingType, xamlType);
            }
            return xamlType;
        }

        public global::Windows.UI.Xaml.Markup.IXamlMember GetMemberByLongName(string longMemberName)
        {
            if (string.IsNullOrEmpty(longMemberName))
            {
                return null;
            }
            global::Windows.UI.Xaml.Markup.IXamlMember xamlMember;
            if (_xamlMembers.TryGetValue(longMemberName, out xamlMember))
            {
                return xamlMember;
            }
            xamlMember = CreateXamlMember(longMemberName);
            if (xamlMember != null)
            {
                _xamlMembers.Add(longMemberName, xamlMember);
            }
            return xamlMember;
        }

        global::System.Collections.Generic.Dictionary<string, global::Windows.UI.Xaml.Markup.IXamlType>
                _xamlTypeCacheByName = new global::System.Collections.Generic.Dictionary<string, global::Windows.UI.Xaml.Markup.IXamlType>();

        global::System.Collections.Generic.Dictionary<global::System.Type, global::Windows.UI.Xaml.Markup.IXamlType>
                _xamlTypeCacheByType = new global::System.Collections.Generic.Dictionary<global::System.Type, global::Windows.UI.Xaml.Markup.IXamlType>();

        global::System.Collections.Generic.Dictionary<string, global::Windows.UI.Xaml.Markup.IXamlMember>
                _xamlMembers = new global::System.Collections.Generic.Dictionary<string, global::Windows.UI.Xaml.Markup.IXamlMember>();

        string[] _typeNameTable = null;
        global::System.Type[] _typeTable = null;

        private void InitTypeTables()
        {
            _typeNameTable = new string[15];
            _typeNameTable[0] = "GetUpAndGo.AboutPage";
            _typeNameTable[1] = "Windows.UI.Xaml.Controls.Page";
            _typeNameTable[2] = "Windows.UI.Xaml.Controls.UserControl";
            _typeNameTable[3] = "GetUpAndGo.Common.NavigationHelper";
            _typeNameTable[4] = "Windows.UI.Xaml.DependencyObject";
            _typeNameTable[5] = "GetUpAndGo.Common.ObservableDictionary";
            _typeNameTable[6] = "Object";
            _typeNameTable[7] = "String";
            _typeNameTable[8] = "GetUpAndGo.SettingsPageBase";
            _typeNameTable[9] = "GetUpAndGo.ActiveTimeSelectorPage";
            _typeNameTable[10] = "GetUpAndGo.CalendarSettingsPage";
            _typeNameTable[11] = "GetUpAndGo.IntroPage";
            _typeNameTable[12] = "GetUpAndGo.MainPage";
            _typeNameTable[13] = "GetUpAndGo.ReminderFrequencyPage";
            _typeNameTable[14] = "GetUpAndGo.StepQuotaPage";

            _typeTable = new global::System.Type[15];
            _typeTable[0] = typeof(global::GetUpAndGo.AboutPage);
            _typeTable[1] = typeof(global::Windows.UI.Xaml.Controls.Page);
            _typeTable[2] = typeof(global::Windows.UI.Xaml.Controls.UserControl);
            _typeTable[3] = typeof(global::GetUpAndGo.Common.NavigationHelper);
            _typeTable[4] = typeof(global::Windows.UI.Xaml.DependencyObject);
            _typeTable[5] = typeof(global::GetUpAndGo.Common.ObservableDictionary);
            _typeTable[6] = typeof(global::System.Object);
            _typeTable[7] = typeof(global::System.String);
            _typeTable[8] = typeof(global::GetUpAndGo.SettingsPageBase);
            _typeTable[9] = typeof(global::GetUpAndGo.ActiveTimeSelectorPage);
            _typeTable[10] = typeof(global::GetUpAndGo.CalendarSettingsPage);
            _typeTable[11] = typeof(global::GetUpAndGo.IntroPage);
            _typeTable[12] = typeof(global::GetUpAndGo.MainPage);
            _typeTable[13] = typeof(global::GetUpAndGo.ReminderFrequencyPage);
            _typeTable[14] = typeof(global::GetUpAndGo.StepQuotaPage);
        }

        private int LookupTypeIndexByName(string typeName)
        {
            if (_typeNameTable == null)
            {
                InitTypeTables();
            }
            for (int i=0; i<_typeNameTable.Length; i++)
            {
                if(0 == string.CompareOrdinal(_typeNameTable[i], typeName))
                {
                    return i;
                }
            }
            return -1;
        }

        private int LookupTypeIndexByType(global::System.Type type)
        {
            if (_typeTable == null)
            {
                InitTypeTables();
            }
            for(int i=0; i<_typeTable.Length; i++)
            {
                if(type == _typeTable[i])
                {
                    return i;
                }
            }
            return -1;
        }

        private object Activate_0_AboutPage() { return new global::GetUpAndGo.AboutPage(); }
        private object Activate_5_ObservableDictionary() { return new global::GetUpAndGo.Common.ObservableDictionary(); }
        private object Activate_9_ActiveTimeSelectorPage() { return new global::GetUpAndGo.ActiveTimeSelectorPage(); }
        private object Activate_10_CalendarSettingsPage() { return new global::GetUpAndGo.CalendarSettingsPage(); }
        private object Activate_11_IntroPage() { return new global::GetUpAndGo.IntroPage(); }
        private object Activate_12_MainPage() { return new global::GetUpAndGo.MainPage(); }
        private object Activate_13_ReminderFrequencyPage() { return new global::GetUpAndGo.ReminderFrequencyPage(); }
        private object Activate_14_StepQuotaPage() { return new global::GetUpAndGo.StepQuotaPage(); }
        private void MapAdd_5_ObservableDictionary(object instance, object key, object item)
        {
            var collection = (global::System.Collections.Generic.IDictionary<global::System.String, global::System.Object>)instance;
            var newKey = (global::System.String)key;
            var newItem = (global::System.Object)item;
            collection.Add(newKey, newItem);
        }

        private global::Windows.UI.Xaml.Markup.IXamlType CreateXamlType(int typeIndex)
        {
            global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlSystemBaseType xamlType = null;
            global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlUserType userType;
            string typeName = _typeNameTable[typeIndex];
            global::System.Type type = _typeTable[typeIndex];

            switch (typeIndex)
            {

            case 0:   //  GetUpAndGo.AboutPage
                userType = new global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Windows.UI.Xaml.Controls.Page"));
                userType.Activator = Activate_0_AboutPage;
                userType.AddMemberName("NavigationHelper");
                userType.AddMemberName("DefaultViewModel");
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 1:   //  Windows.UI.Xaml.Controls.Page
                xamlType = new global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlSystemBaseType(typeName, type);
                break;

            case 2:   //  Windows.UI.Xaml.Controls.UserControl
                xamlType = new global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlSystemBaseType(typeName, type);
                break;

            case 3:   //  GetUpAndGo.Common.NavigationHelper
                userType = new global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Windows.UI.Xaml.DependencyObject"));
                userType.SetIsReturnTypeStub();
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 4:   //  Windows.UI.Xaml.DependencyObject
                xamlType = new global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlSystemBaseType(typeName, type);
                break;

            case 5:   //  GetUpAndGo.Common.ObservableDictionary
                userType = new global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Object"));
                userType.DictionaryAdd = MapAdd_5_ObservableDictionary;
                userType.SetIsReturnTypeStub();
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 6:   //  Object
                xamlType = new global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlSystemBaseType(typeName, type);
                break;

            case 7:   //  String
                xamlType = new global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlSystemBaseType(typeName, type);
                break;

            case 8:   //  GetUpAndGo.SettingsPageBase
                userType = new global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Windows.UI.Xaml.Controls.Page"));
                userType.AddMemberName("NavigationHelper");
                userType.AddMemberName("DefaultViewModel");
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 9:   //  GetUpAndGo.ActiveTimeSelectorPage
                userType = new global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("GetUpAndGo.SettingsPageBase"));
                userType.Activator = Activate_9_ActiveTimeSelectorPage;
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 10:   //  GetUpAndGo.CalendarSettingsPage
                userType = new global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("GetUpAndGo.SettingsPageBase"));
                userType.Activator = Activate_10_CalendarSettingsPage;
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 11:   //  GetUpAndGo.IntroPage
                userType = new global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Windows.UI.Xaml.Controls.Page"));
                userType.Activator = Activate_11_IntroPage;
                userType.AddMemberName("NavigationHelper");
                userType.AddMemberName("DefaultViewModel");
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 12:   //  GetUpAndGo.MainPage
                userType = new global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Windows.UI.Xaml.Controls.Page"));
                userType.Activator = Activate_12_MainPage;
                userType.AddMemberName("NavigationHelper");
                userType.AddMemberName("DefaultViewModel");
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 13:   //  GetUpAndGo.ReminderFrequencyPage
                userType = new global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("GetUpAndGo.SettingsPageBase"));
                userType.Activator = Activate_13_ReminderFrequencyPage;
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 14:   //  GetUpAndGo.StepQuotaPage
                userType = new global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("GetUpAndGo.SettingsPageBase"));
                userType.Activator = Activate_14_StepQuotaPage;
                userType.SetIsLocalType();
                xamlType = userType;
                break;
            }
            return xamlType;
        }

        private global::System.Collections.Generic.List<global::Windows.UI.Xaml.Markup.IXamlMetadataProvider> _otherProviders;
        private global::System.Collections.Generic.List<global::Windows.UI.Xaml.Markup.IXamlMetadataProvider> OtherProviders
        {
            get
            {
                if(_otherProviders == null)
                {
                    _otherProviders = new global::System.Collections.Generic.List<global::Windows.UI.Xaml.Markup.IXamlMetadataProvider>();
                    global::Windows.UI.Xaml.Markup.IXamlMetadataProvider provider;
                    provider = new global::Microsoft.Band.Microsoft_Band_Phone_XamlTypeInfo.XamlMetaDataProvider() as global::Windows.UI.Xaml.Markup.IXamlMetadataProvider;
                    _otherProviders.Add(provider); 
                    provider = new global::Microsoft.Band.Microsoft_Band_Store_XamlTypeInfo.XamlMetaDataProvider() as global::Windows.UI.Xaml.Markup.IXamlMetadataProvider;
                    _otherProviders.Add(provider); 
                }
                return _otherProviders;
            }
        }

        private global::Windows.UI.Xaml.Markup.IXamlType CheckOtherMetadataProvidersForName(string typeName)
        {
            global::Windows.UI.Xaml.Markup.IXamlType xamlType = null;
            global::Windows.UI.Xaml.Markup.IXamlType foundXamlType = null;
            foreach(global::Windows.UI.Xaml.Markup.IXamlMetadataProvider xmp in OtherProviders)
            {
                xamlType = xmp.GetXamlType(typeName);
                if(xamlType != null)
                {
                    if(xamlType.IsConstructible)    // not Constructible means it might be a Return Type Stub
                    {
                        return xamlType;
                    }
                    foundXamlType = xamlType;
                }
            }
            return foundXamlType;
        }

        private global::Windows.UI.Xaml.Markup.IXamlType CheckOtherMetadataProvidersForType(global::System.Type type)
        {
            global::Windows.UI.Xaml.Markup.IXamlType xamlType = null;
            global::Windows.UI.Xaml.Markup.IXamlType foundXamlType = null;
            foreach(global::Windows.UI.Xaml.Markup.IXamlMetadataProvider xmp in OtherProviders)
            {
                xamlType = xmp.GetXamlType(type);
                if(xamlType != null)
                {
                    if(xamlType.IsConstructible)    // not Constructible means it might be a Return Type Stub
                    {
                        return xamlType;
                    }
                    foundXamlType = xamlType;
                }
            }
            return foundXamlType;
        }

        private object get_0_AboutPage_NavigationHelper(object instance)
        {
            var that = (global::GetUpAndGo.AboutPage)instance;
            return that.NavigationHelper;
        }
        private object get_1_AboutPage_DefaultViewModel(object instance)
        {
            var that = (global::GetUpAndGo.AboutPage)instance;
            return that.DefaultViewModel;
        }
        private object get_2_SettingsPageBase_NavigationHelper(object instance)
        {
            var that = (global::GetUpAndGo.SettingsPageBase)instance;
            return that.NavigationHelper;
        }
        private object get_3_SettingsPageBase_DefaultViewModel(object instance)
        {
            var that = (global::GetUpAndGo.SettingsPageBase)instance;
            return that.DefaultViewModel;
        }
        private object get_4_IntroPage_NavigationHelper(object instance)
        {
            var that = (global::GetUpAndGo.IntroPage)instance;
            return that.NavigationHelper;
        }
        private object get_5_IntroPage_DefaultViewModel(object instance)
        {
            var that = (global::GetUpAndGo.IntroPage)instance;
            return that.DefaultViewModel;
        }
        private object get_6_MainPage_NavigationHelper(object instance)
        {
            var that = (global::GetUpAndGo.MainPage)instance;
            return that.NavigationHelper;
        }
        private object get_7_MainPage_DefaultViewModel(object instance)
        {
            var that = (global::GetUpAndGo.MainPage)instance;
            return that.DefaultViewModel;
        }

        private global::Windows.UI.Xaml.Markup.IXamlMember CreateXamlMember(string longMemberName)
        {
            global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlMember xamlMember = null;
            global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlUserType userType;

            switch (longMemberName)
            {
            case "GetUpAndGo.AboutPage.NavigationHelper":
                userType = (global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlUserType)GetXamlTypeByName("GetUpAndGo.AboutPage");
                xamlMember = new global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlMember(this, "NavigationHelper", "GetUpAndGo.Common.NavigationHelper");
                xamlMember.Getter = get_0_AboutPage_NavigationHelper;
                xamlMember.SetIsReadOnly();
                break;
            case "GetUpAndGo.AboutPage.DefaultViewModel":
                userType = (global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlUserType)GetXamlTypeByName("GetUpAndGo.AboutPage");
                xamlMember = new global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlMember(this, "DefaultViewModel", "GetUpAndGo.Common.ObservableDictionary");
                xamlMember.Getter = get_1_AboutPage_DefaultViewModel;
                xamlMember.SetIsReadOnly();
                break;
            case "GetUpAndGo.SettingsPageBase.NavigationHelper":
                userType = (global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlUserType)GetXamlTypeByName("GetUpAndGo.SettingsPageBase");
                xamlMember = new global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlMember(this, "NavigationHelper", "GetUpAndGo.Common.NavigationHelper");
                xamlMember.Getter = get_2_SettingsPageBase_NavigationHelper;
                xamlMember.SetIsReadOnly();
                break;
            case "GetUpAndGo.SettingsPageBase.DefaultViewModel":
                userType = (global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlUserType)GetXamlTypeByName("GetUpAndGo.SettingsPageBase");
                xamlMember = new global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlMember(this, "DefaultViewModel", "GetUpAndGo.Common.ObservableDictionary");
                xamlMember.Getter = get_3_SettingsPageBase_DefaultViewModel;
                xamlMember.SetIsReadOnly();
                break;
            case "GetUpAndGo.IntroPage.NavigationHelper":
                userType = (global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlUserType)GetXamlTypeByName("GetUpAndGo.IntroPage");
                xamlMember = new global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlMember(this, "NavigationHelper", "GetUpAndGo.Common.NavigationHelper");
                xamlMember.Getter = get_4_IntroPage_NavigationHelper;
                xamlMember.SetIsReadOnly();
                break;
            case "GetUpAndGo.IntroPage.DefaultViewModel":
                userType = (global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlUserType)GetXamlTypeByName("GetUpAndGo.IntroPage");
                xamlMember = new global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlMember(this, "DefaultViewModel", "GetUpAndGo.Common.ObservableDictionary");
                xamlMember.Getter = get_5_IntroPage_DefaultViewModel;
                xamlMember.SetIsReadOnly();
                break;
            case "GetUpAndGo.MainPage.NavigationHelper":
                userType = (global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlUserType)GetXamlTypeByName("GetUpAndGo.MainPage");
                xamlMember = new global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlMember(this, "NavigationHelper", "GetUpAndGo.Common.NavigationHelper");
                xamlMember.Getter = get_6_MainPage_NavigationHelper;
                xamlMember.SetIsReadOnly();
                break;
            case "GetUpAndGo.MainPage.DefaultViewModel":
                userType = (global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlUserType)GetXamlTypeByName("GetUpAndGo.MainPage");
                xamlMember = new global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlMember(this, "DefaultViewModel", "GetUpAndGo.Common.ObservableDictionary");
                xamlMember.Getter = get_7_MainPage_DefaultViewModel;
                xamlMember.SetIsReadOnly();
                break;
            }
            return xamlMember;
        }
    }

    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks", "4.0.0.0")]    
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    internal class XamlSystemBaseType : global::Windows.UI.Xaml.Markup.IXamlType
    {
        string _fullName;
        global::System.Type _underlyingType;

        public XamlSystemBaseType(string fullName, global::System.Type underlyingType)
        {
            _fullName = fullName;
            _underlyingType = underlyingType;
        }

        public string FullName { get { return _fullName; } }

        public global::System.Type UnderlyingType
        {
            get
            {
                return _underlyingType;
            }
        }

        virtual public global::Windows.UI.Xaml.Markup.IXamlType BaseType { get { throw new global::System.NotImplementedException(); } }
        virtual public global::Windows.UI.Xaml.Markup.IXamlMember ContentProperty { get { throw new global::System.NotImplementedException(); } }
        virtual public global::Windows.UI.Xaml.Markup.IXamlMember GetMember(string name) { throw new global::System.NotImplementedException(); }
        virtual public bool IsArray { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsCollection { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsConstructible { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsDictionary { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsMarkupExtension { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsBindable { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsReturnTypeStub { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsLocalType { get { throw new global::System.NotImplementedException(); } }
        virtual public global::Windows.UI.Xaml.Markup.IXamlType ItemType { get { throw new global::System.NotImplementedException(); } }
        virtual public global::Windows.UI.Xaml.Markup.IXamlType KeyType { get { throw new global::System.NotImplementedException(); } }
        virtual public object ActivateInstance() { throw new global::System.NotImplementedException(); }
        virtual public void AddToMap(object instance, object key, object item)  { throw new global::System.NotImplementedException(); }
        virtual public void AddToVector(object instance, object item)  { throw new global::System.NotImplementedException(); }
        virtual public void RunInitializer()   { throw new global::System.NotImplementedException(); }
        virtual public object CreateFromString(string input)   { throw new global::System.NotImplementedException(); }
    }
    
    internal delegate object Activator();
    internal delegate void AddToCollection(object instance, object item);
    internal delegate void AddToDictionary(object instance, object key, object item);

    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks", "4.0.0.0")]    
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    internal class XamlUserType : global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlSystemBaseType
    {
        global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlTypeInfoProvider _provider;
        global::Windows.UI.Xaml.Markup.IXamlType _baseType;
        bool _isArray;
        bool _isMarkupExtension;
        bool _isBindable;
        bool _isReturnTypeStub;
        bool _isLocalType;

        string _contentPropertyName;
        string _itemTypeName;
        string _keyTypeName;
        global::System.Collections.Generic.Dictionary<string, string> _memberNames;
        global::System.Collections.Generic.Dictionary<string, object> _enumValues;

        public XamlUserType(global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlTypeInfoProvider provider, string fullName, global::System.Type fullType, global::Windows.UI.Xaml.Markup.IXamlType baseType)
            :base(fullName, fullType)
        {
            _provider = provider;
            _baseType = baseType;
        }

        // --- Interface methods ----

        override public global::Windows.UI.Xaml.Markup.IXamlType BaseType { get { return _baseType; } }
        override public bool IsArray { get { return _isArray; } }
        override public bool IsCollection { get { return (CollectionAdd != null); } }
        override public bool IsConstructible { get { return (Activator != null); } }
        override public bool IsDictionary { get { return (DictionaryAdd != null); } }
        override public bool IsMarkupExtension { get { return _isMarkupExtension; } }
        override public bool IsBindable { get { return _isBindable; } }
        override public bool IsReturnTypeStub { get { return _isReturnTypeStub; } }
        override public bool IsLocalType { get { return _isLocalType; } }

        override public global::Windows.UI.Xaml.Markup.IXamlMember ContentProperty
        {
            get { return _provider.GetMemberByLongName(_contentPropertyName); }
        }

        override public global::Windows.UI.Xaml.Markup.IXamlType ItemType
        {
            get { return _provider.GetXamlTypeByName(_itemTypeName); }
        }

        override public global::Windows.UI.Xaml.Markup.IXamlType KeyType
        {
            get { return _provider.GetXamlTypeByName(_keyTypeName); }
        }

        override public global::Windows.UI.Xaml.Markup.IXamlMember GetMember(string name)
        {
            if (_memberNames == null)
            {
                return null;
            }
            string longName;
            if (_memberNames.TryGetValue(name, out longName))
            {
                return _provider.GetMemberByLongName(longName);
            }
            return null;
        }

        override public object ActivateInstance()
        {
            return Activator(); 
        }

        override public void AddToMap(object instance, object key, object item) 
        {
            DictionaryAdd(instance, key, item);
        }

        override public void AddToVector(object instance, object item)
        {
            CollectionAdd(instance, item);
        }

        override public void RunInitializer() 
        {
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(UnderlyingType.TypeHandle);
        }

        override public object CreateFromString(string input)
        {
            if (_enumValues != null)
            {
                int value = 0;

                string[] valueParts = input.Split(',');

                foreach (string valuePart in valueParts) 
                {
                    object partValue;
                    int enumFieldValue = 0;
                    try
                    {
                        if (_enumValues.TryGetValue(valuePart.Trim(), out partValue))
                        {
                            enumFieldValue = global::System.Convert.ToInt32(partValue);
                        }
                        else
                        {
                            try
                            {
                                enumFieldValue = global::System.Convert.ToInt32(valuePart.Trim());
                            }
                            catch( global::System.FormatException )
                            {
                                foreach( string key in _enumValues.Keys )
                                {
                                    if( string.Compare(valuePart.Trim(), key, global::System.StringComparison.OrdinalIgnoreCase) == 0 )
                                    {
                                        if( _enumValues.TryGetValue(key.Trim(), out partValue) )
                                        {
                                            enumFieldValue = global::System.Convert.ToInt32(partValue);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        value |= enumFieldValue; 
                    }
                    catch( global::System.FormatException )
                    {
                        throw new global::System.ArgumentException(input, FullName);
                    }
                }

                return value; 
            }
            throw new global::System.ArgumentException(input, FullName);
        }

        // --- End of Interface methods

        public Activator Activator { get; set; }
        public AddToCollection CollectionAdd { get; set; }
        public AddToDictionary DictionaryAdd { get; set; }

        public void SetContentPropertyName(string contentPropertyName)
        {
            _contentPropertyName = contentPropertyName;
        }

        public void SetIsArray()
        {
            _isArray = true; 
        }

        public void SetIsMarkupExtension()
        {
            _isMarkupExtension = true;
        }

        public void SetIsBindable()
        {
            _isBindable = true;
        }

        public void SetIsReturnTypeStub()
        {
            _isReturnTypeStub = true;
        }

        public void SetIsLocalType()
        {
            _isLocalType = true;
        }

        public void SetItemTypeName(string itemTypeName)
        {
            _itemTypeName = itemTypeName;
        }

        public void SetKeyTypeName(string keyTypeName)
        {
            _keyTypeName = keyTypeName;
        }

        public void AddMemberName(string shortName)
        {
            if(_memberNames == null)
            {
                _memberNames =  new global::System.Collections.Generic.Dictionary<string,string>();
            }
            _memberNames.Add(shortName, FullName + "." + shortName);
        }

        public void AddEnumValue(string name, object value)
        {
            if (_enumValues == null)
            {
                _enumValues = new global::System.Collections.Generic.Dictionary<string, object>();
            }
            _enumValues.Add(name, value);
        }
    }

    internal delegate object Getter(object instance);
    internal delegate void Setter(object instance, object value);

    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks", "4.0.0.0")]    
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    internal class XamlMember : global::Windows.UI.Xaml.Markup.IXamlMember
    {
        global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlTypeInfoProvider _provider;
        string _name;
        bool _isAttachable;
        bool _isDependencyProperty;
        bool _isReadOnly;

        string _typeName;
        string _targetTypeName;

        public XamlMember(global::GetUpAndGo.GetUpAndGo_XamlTypeInfo.XamlTypeInfoProvider provider, string name, string typeName)
        {
            _name = name;
            _typeName = typeName;
            _provider = provider;
        }

        public string Name { get { return _name; } }

        public global::Windows.UI.Xaml.Markup.IXamlType Type
        {
            get { return _provider.GetXamlTypeByName(_typeName); }
        }

        public void SetTargetTypeName(string targetTypeName)
        {
            _targetTypeName = targetTypeName;
        }
        public global::Windows.UI.Xaml.Markup.IXamlType TargetType
        {
            get { return _provider.GetXamlTypeByName(_targetTypeName); }
        }

        public void SetIsAttachable() { _isAttachable = true; }
        public bool IsAttachable { get { return _isAttachable; } }

        public void SetIsDependencyProperty() { _isDependencyProperty = true; }
        public bool IsDependencyProperty { get { return _isDependencyProperty; } }

        public void SetIsReadOnly() { _isReadOnly = true; }
        public bool IsReadOnly { get { return _isReadOnly; } }

        public Getter Getter { get; set; }
        public object GetValue(object instance)
        {
            if (Getter != null)
                return Getter(instance);
            else
                throw new global::System.InvalidOperationException("GetValue");
        }

        public Setter Setter { get; set; }
        public void SetValue(object instance, object value)
        {
            if (Setter != null)
                Setter(instance, value);
            else
                throw new global::System.InvalidOperationException("SetValue");
        }
    }
}






