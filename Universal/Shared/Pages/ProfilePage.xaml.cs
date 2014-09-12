using AppboyPlatform.PCL.Models;
using AppboyPlatform.Universal;
using TestApp.Store.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace TestApp.Store.Pages {
  public sealed partial class ProfilePage : Page {
    private NavigationHelper navigationHelper;

    public NavigationHelper NavigationHelper {
      get { return navigationHelper; }
    }

    public ProfilePage() {
      InitializeComponent();
      navigationHelper = new NavigationHelper(this);
      navigationHelper.LoadState += navigationHelper_LoadState;
      navigationHelper.SaveState += navigationHelper_SaveState;
    }

    private void navigationHelper_LoadState(object sender, LoadStateEventArgs e) { }
    private void navigationHelper_SaveState(object sender, SaveStateEventArgs e) { }

    #region NavigationHelper registration
    protected override void OnNavigatedTo(NavigationEventArgs e) {
      navigationHelper.OnNavigatedTo(e);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e) {
      navigationHelper.OnNavigatedFrom(e);
    }
    #endregion

    private void Save_Click(object sender, RoutedEventArgs e) {
      Appboy.SharedInstance.AppboyUser.FirstName = FirstNameTextBox.Text;
      Appboy.SharedInstance.AppboyUser.LastName = LastNameTextBox.Text;
      Appboy.SharedInstance.AppboyUser.Email = EmailTextBox.Text;
      Appboy.SharedInstance.AppboyUser.Bio = BioTextBox.Text;
      Appboy.SharedInstance.AppboyUser.PhoneNumber = PhoneNumberTextBox.Text;
      if (GenderMale.IsChecked ?? false) {
        Appboy.SharedInstance.AppboyUser.Gender = Gender.Male;
      }
      if (GenderFemale.IsChecked ?? false) {
        Appboy.SharedInstance.AppboyUser.Gender = Gender.Female;
      }
      Appboy.SharedInstance.AppboyUser.SetCustomAttribute("FavoriteColor", FavoriteColorTextBox.Text);
      navigationHelper.GoBack();
    }
  }
}
