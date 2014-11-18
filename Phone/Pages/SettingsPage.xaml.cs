using System;
using System.Windows;
using System.Windows.Navigation;
using AppboyPlatform.Phone;
using Microsoft.Phone.Controls;

namespace TestApp.Phone.Pages {
  public partial class SettingsPage : PhoneApplicationPage {
    public SettingsPage() {
      InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
      base.OnNavigatedTo(e);
      ToastEnabledCheckBox.IsChecked = Appboy.SharedInstance.PushManager.ToastOptInStatus ?? false;
    }

    private void Back_Click(object sender, EventArgs e) {
      NavigationService.GoBack();
    }

    private void ToastEnabledCheckBox_Click(object sender, RoutedEventArgs e) {
      if (ToastEnabledCheckBox.IsChecked == true) {
        Appboy.SharedInstance.PushManager.ToastOptInStatus = true;
      } else {
        Appboy.SharedInstance.PushManager.ToastOptInStatus = false;
      }
    }
  }
}
