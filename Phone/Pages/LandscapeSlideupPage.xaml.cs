using System.Windows;
using AppboyPlatform.PCL.Models.Incoming;
using AppboyPlatform.Phone;
using Microsoft.Phone.Controls;

namespace TestApp.Phone.Pages {
  public partial class LandscapeSlideupPage : PhoneApplicationPage {
    public LandscapeSlideupPage() {
      InitializeComponent();
      Appboy.SharedInstance.SlideupManager.CurrentPanelForSinglePageLandscapeApps = LayoutRoot;
    }

    private void CreateAndAddSlideupTopButton_Click(object sender, RoutedEventArgs e) {
      var slideup = new Slideup("This is a test slideup.", SlideFrom.TOP, DismissType.AUTO_DISMISS, 5000);
      Appboy.SharedInstance.SlideupManager.AddSlideup(slideup);
    }

    private void CreateAndAddSlideupBottomButton_Click(object sender, RoutedEventArgs e) {
      var slideup = new Slideup("This is a test slideup.", SlideFrom.BOTTOM, DismissType.AUTO_DISMISS, 5000);
      Appboy.SharedInstance.SlideupManager.AddSlideup(slideup);
    }
  }
}
