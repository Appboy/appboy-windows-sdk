using AppboyPlatform.Universal;
using AppboyPlatform.Universal.Managers;
using Windows.UI.Xaml.Controls;

namespace TestApp.Store.AppboyClasses {
  public sealed partial class CustomSlideupControl : UserControl {
    public CustomSlideupControl() {
      this.InitializeComponent();
    }

    private void Dismiss_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) {
      Appboy.SharedInstance.SlideupManager.HideCurrentSlideup(true);
    }
  }
}
