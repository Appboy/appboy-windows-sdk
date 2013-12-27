using AppboyPlatform.PCL.Models.Incoming.Cards;
using AppboyPlatform.PCL.Results;
using AppboyPlatform.Store;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestApp.Store.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace TestApp.Store.Pages {
  public sealed partial class MiscPage : Page {
    private NavigationHelper _navigationHelper;
    private string _userId1;
    private string _userId2;
    private bool? _isUserId1;

    public NavigationHelper NavigationHelper {
      get { return _navigationHelper; }
    }

    public MiscPage() {
      InitializeComponent();
      _navigationHelper = new NavigationHelper(this);
      _navigationHelper.LoadState += navigationHelper_LoadState;
      _navigationHelper.SaveState += navigationHelper_SaveState;
      _userId1 = "testUser1";
      _userId2 = "testUser2";
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

    private void LogCustomEvent_Click(object sender, RoutedEventArgs e) {
      Appboy.SharedInstance.EventLogger.LogCustomEvent("Clicked Button");
    }

    private void LogPurchase_Click(object sender, RoutedEventArgs e) {
      Appboy.SharedInstance.EventLogger.LogPurchase("Test_Purchase", "USD", 0.99m);
    }

    private void SubmitFeedback_Click(object sender, RoutedEventArgs e) {
      Appboy.SharedInstance.SubmitFeedback("martin@appboy.com", "test", false);
    }

    private void RequestFeed_Click(object sender, RoutedEventArgs e) {
      Action<Task<IResult>> logCards = (continuation) => {
        Debug.WriteLine("Received the following news feed cards.");
        foreach (BaseCard card in continuation.Result.Cards ?? Enumerable.Empty<BaseCard>()) {
          Debug.WriteLine("News feed card {0}.", card);
        }
      };
      Appboy.SharedInstance.RequestFeed().ContinueWith(logCards);
    }

    private void RequestDataFlush_Click(object sender, RoutedEventArgs e) {
      Appboy.SharedInstance.RequestDataFlush();
    }

    private void RequestSlideup_Click(object sender, RoutedEventArgs e) {
      Appboy.SharedInstance.RequestSlideup();
    }    

    private void ChangeUser_Click(object sender, RoutedEventArgs e) {
      if (!(_isUserId1 ?? false)) {
        Appboy.SharedInstance.ChangeUser(_userId1);
        _isUserId1 = true;
      } else {
        Appboy.SharedInstance.ChangeUser(_userId2);
        _isUserId1 = false;
      }
    }
  }
}
