using System.Windows;
using AppboyPlatform.PCL.Models;
using AppboyPlatform.Phone;
using Microsoft.Phone.Controls;

namespace TestApp.Phone.Pages {
  public partial class ProfilePage : PhoneApplicationPage {
    public ProfilePage() {
      InitializeComponent();
    }

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
      NavigationService.GoBack();
    }
  }
}
