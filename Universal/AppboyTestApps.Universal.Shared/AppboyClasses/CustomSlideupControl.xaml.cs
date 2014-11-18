using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using AppboyPlatform.Universal;

namespace AppboyTestApps.Universal.AppboyClasses {
  public sealed partial class CustomSlideupControl : UserControl {
    public CustomSlideupControl() {
      InitializeComponent();
    }

    private void Dismiss_Tapped(object sender, TappedRoutedEventArgs e) {
      Appboy.SharedInstance.SlideupManager.HideCurrentSlideup(true);
    }
  }
}
