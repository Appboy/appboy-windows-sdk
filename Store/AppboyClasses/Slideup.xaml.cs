using Windows.UI.Xaml.Controls;

namespace TestApp.Store.AppboyClasses {
  public sealed partial class Slideup : UserControl {
    public Slideup() {
      this.InitializeComponent();      
    }

    public void SetText(string text) {
      SlideupTextbox.Text = text;
    }
  }
}
