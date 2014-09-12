using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TestApp.Store.Common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.ApplicationModel.Core;

namespace TestApp.Store.Pages {
  public sealed partial class AppboyFeedbackPage : Page {
    private NavigationHelper _navigationHelper;

    public NavigationHelper NavigationHelper {
      get { return _navigationHelper; }
    }

    public AppboyFeedbackPage() {
      InitializeComponent();
      _navigationHelper = new NavigationHelper(this);
      _navigationHelper.LoadState += navigationHelper_LoadState;
      _navigationHelper.SaveState += navigationHelper_SaveState;
    }

    private void navigationHelper_LoadState(object sender, LoadStateEventArgs e) { }

    private void navigationHelper_SaveState(object sender, SaveStateEventArgs e) { }

    #region NavigationHelper registration
    protected override void OnNavigatedTo(NavigationEventArgs e) {
      _navigationHelper.OnNavigatedTo(e);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e) {
      _navigationHelper.OnNavigatedFrom(e);
    }
    #endregion

    private void AppboyFeedback_OnCancel(object sender, EventArgs e) {
      _navigationHelper.GoBack();
    }

    private void AppboyFeedback_AfterSubmit(object sender, EventArgs e) {
      _navigationHelper.GoBack();
    }
  }
}
