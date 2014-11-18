using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.ApplicationModel.Core;

namespace AppboyTestApps.Universal.Pages {
  public sealed partial class SocialFeedPage : Page {
    private NavigationHelper _navigationHelper;

    public NavigationHelper NavigationHelper {
      get { return _navigationHelper; }
    }

    public SocialFeedPage() {
      InitializeComponent();
      _navigationHelper = new NavigationHelper(this);
      _navigationHelper.LoadState += navigationHelper_LoadState;
      _navigationHelper.SaveState += navigationHelper_SaveState;
    }

    private void navigationHelper_LoadState(object sender, LoadStateEventArgs e) {}

    private void navigationHelper_SaveState(object sender, SaveStateEventArgs e) {}

    #region NavigationHelper registration

    protected override void OnNavigatedTo(NavigationEventArgs e) {
      _navigationHelper.OnNavigatedTo(e);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e) {
      _navigationHelper.OnNavigatedFrom(e);
    }

    #endregion
  }
}
