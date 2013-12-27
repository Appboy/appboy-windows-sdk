using AppboyPlatform.Store.Managers;

namespace TestApp.Store.AppboyClasses {
  internal class SlideupFactory : ISlideupFactory {
    public Windows.UI.Xaml.Controls.UserControl GetSlideup(string message) {
      var slideup = new Slideup();
      slideup.SetText(message);
      return slideup;
    }
  }
}
