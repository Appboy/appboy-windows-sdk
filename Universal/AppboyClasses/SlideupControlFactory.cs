using AppboyPlatform.PCL.Models.Incoming;
using AppboyPlatform.Universal.Managers;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TestApp.Store.AppboyClasses {
  internal class SlideupControlFactory : ISlideupControlFactory {
    public UserControl GetSlideupControl(Slideup slideup) {
      var control = new CustomSlideupControl();
      if (slideup.ClickAction == ClickAction.NONE) {
        control.Chevron.Visibility = Visibility.Collapsed;
      } else {
        control.Chevron.Visibility = Visibility.Visible;
      }

      StringBuilder message = new StringBuilder();
      message.Append(slideup.Message);
      if (slideup.Extras != null && slideup.Extras.Count > 0) {
        message.AppendLine(System.Environment.NewLine);
        foreach (var extra in slideup.Extras) {
          message.AppendFormat("Extra key {0} has value {1}." + System.Environment.NewLine, extra.Key, extra.Value);
        }
      }
      control.Message.Text = message.ToString();
      return control;
    }
  }
}
