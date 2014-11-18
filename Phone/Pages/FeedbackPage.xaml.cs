using System;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace TestApp.Phone.Pages {
  public partial class FeedbackPage : PhoneApplicationPage {
    public FeedbackPage() {
      InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
      base.OnNavigatedTo(e);
    }

    private void Feedback_OnCancel(object sender, EventArgs e) {
      NavigationService.GoBack();
    }

    private void Feedback_AfterSubmit(object sender, EventArgs e) {
      NavigationService.GoBack();
    }
  }
}
