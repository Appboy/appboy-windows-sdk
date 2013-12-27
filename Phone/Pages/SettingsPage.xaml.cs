using AppboyPlatform.Phone;
using Microsoft.Phone.Controls;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Windows;
using TestApp.Phone.AppboyClasses;
using AppboyUI.Phone;

namespace TestApp.Phone.Pages {
  public partial class SettingsPage : PhoneApplicationPage {
    public SettingsPage() {
      InitializeComponent();
      Loaded += SettingsPage_Loaded;
    }

    private SettingsData _settingsData;

    private void SettingsPage_Loaded(object sender, RoutedEventArgs e) {
      bool? useCustomSettings;
      IsolatedStorageSettings.ApplicationSettings.TryGetValue(CustomSlideupKey, out useCustomSettings);
      _settingsData = new SettingsData() {
        ToastEnabledCheckBoxValue = Appboy.SharedInstance.PushManager.ToastOptInStatus ?? false,
        UseCustomSlideupCheckBoxValue = useCustomSettings ?? false
      };
      ToastEnabledCheckBox.DataContext = _settingsData;
      UseCustomSlideupCheckBox.DataContext = _settingsData;
    }

    public class SettingsData : INotifyPropertyChanged {

      private bool _toastEnabledCheckBoxValue = true;
      private bool _useCustomSlideupCheckBoxValue = true;
      public event PropertyChangedEventHandler PropertyChanged;

      public bool ToastEnabledCheckBoxValue {
        get { return _toastEnabledCheckBoxValue; }
        set {
          _toastEnabledCheckBoxValue = value;
          // Call NotifyPropertyChanged when the source property 
          // is updated.
          NotifyPropertyChanged("ToastEnabledCheckBoxValue");
        }
      }

      public bool UseCustomSlideupCheckBoxValue {
        get { return _useCustomSlideupCheckBoxValue; }
        set {
          _useCustomSlideupCheckBoxValue = value;
          NotifyPropertyChanged("UseCustomSlideupCheckBoxValue");
        }
      }

      public void NotifyPropertyChanged(string propertyName) {
        if (PropertyChanged != null) {
          PropertyChanged(this,
            new PropertyChangedEventArgs(propertyName));
        }
      }
    }

    private void ToastCheckBox_Checked(object sender, RoutedEventArgs e) {
      Appboy.SharedInstance.PushManager.ToastOptInStatus = true;
    }

    private void ToastCheckBox_Unchecked(object sender, RoutedEventArgs e) {
      Appboy.SharedInstance.PushManager.ToastOptInStatus = false;
    }

    public static string CustomSlideupKey = "use-custom-slideup-key";

    private void SlideupCheckBox_Checked(object sender, RoutedEventArgs e) {
      IsolatedStorageSettings.ApplicationSettings[CustomSlideupKey] = true;
      IsolatedStorageSettings.ApplicationSettings.Save();
      Appboy.SharedInstance.SlideupManager.SlideupFactory = new SlideupFactory();
    }

    private void SlideupCheckBox_Unchecked(object sender, RoutedEventArgs e) {
      IsolatedStorageSettings.ApplicationSettings[CustomSlideupKey] = false;
      IsolatedStorageSettings.ApplicationSettings.Save();
      Appboy.SharedInstance.SlideupManager.SlideupFactory = new AppboyUI.Phone.Factories.SlideupFactory();
    }
  }
}