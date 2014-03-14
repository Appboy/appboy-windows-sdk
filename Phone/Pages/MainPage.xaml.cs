using AppboyPlatform.Phone;
using Microsoft.Phone.Controls;
using System;
using System.Windows;

namespace TestApp.Phone.Pages {
  public partial class MainPage : PhoneApplicationPage {
    public MainPage() {
      InitializeComponent();      
    }

    protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e) {
      base.OnNavigatedTo(e);

      if (Appboy.SharedInstance.PushManager.ToastOptInStatus == null) {
        var messageBoxResult = MessageBox.Show("We would like to send toast notifications to enhance your experience of the application.","Toast Notifications.", MessageBoxButton.OKCancel);        
        Appboy.SharedInstance.PushManager.ToastOptInStatus = (messageBoxResult == MessageBoxResult.OK);
      }
    }

    private void Feed_Click(object sender, EventArgs e) {
      NavigationService.Navigate(new Uri("/Pages/FeedPage.xaml", UriKind.Relative));
    }

    private void Feedback_Click(object sender, EventArgs e) {
      NavigationService.Navigate(new Uri("/Pages/FeedbackPage.xaml", UriKind.Relative));
    }

    private void Profile_Click(object sender, EventArgs e) {
      NavigationService.Navigate(new Uri("/Pages/ProfilePage.xaml", UriKind.Relative));
    }

    private void Misc_Click(object sender, EventArgs e) {
      NavigationService.Navigate(new Uri("/Pages/MiscPage.xaml", UriKind.Relative));
    }

    private void Settings_Click(object sender, EventArgs e) {
      NavigationService.Navigate(new Uri("/Pages/SettingsPage.xaml", UriKind.Relative));
    }

    private void Slideups_Click(object sender, EventArgs e) {
      NavigationService.Navigate(new Uri("/Pages/SlideupPage.xaml", UriKind.Relative));
    }
  }
}