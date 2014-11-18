using System;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AppboyPlatform.PCL.Models.Incoming;
using AppboyPlatform.Universal.Managers;

namespace AppboyTestApps.Universal.AppboyClasses {
  internal class SlideupControlFactory : ISlideupControlFactory {
    public UserControl GetSlideupControl(Slideup slideup) {
      var control = new CustomSlideupControl();
      if (slideup.ClickAction == ClickAction.NONE) {
        control.Chevron.Visibility = Visibility.Collapsed;
      } else {
        control.Chevron.Visibility = Visibility.Visible;
      }

      var message = new StringBuilder();
      message.Append(slideup.Message);
      if (slideup.Extras != null && slideup.Extras.Count > 0) {
        message.AppendLine(Environment.NewLine);
        foreach (var extra in slideup.Extras) {
          message.AppendFormat("Extra key {0} has value {1}." + Environment.NewLine, extra.Key, extra.Value);
        }
      }
      control.Message.Text = message.ToString();
      return control;
    }
  }
}
