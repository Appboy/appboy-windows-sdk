using AppboyPlatform.Phone.Managers;
using System.Windows.Controls;

namespace TestApp.Phone.AppboyClasses {
  public class SlideupFactory : ISlideupFactory {
    public UserControl GetSlideup(string message) {
      var slideup = new Slideup();
      slideup.SetText(message);
      return slideup;
    }
  }
}
