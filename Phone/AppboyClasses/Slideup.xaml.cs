using System.Windows.Controls;

namespace TestApp.Phone.AppboyClasses {
  public partial class Slideup : UserControl {
    public Slideup() {
      InitializeComponent();
    }

    public void SetText(string s) {
      SlideupTextblock.Text = s;
    }
  }
}
