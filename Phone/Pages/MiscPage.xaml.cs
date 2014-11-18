using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AppboyPlatform.PCL.Models.Incoming.Cards;
using AppboyPlatform.PCL.Results;
using AppboyPlatform.Phone;
using Microsoft.Phone.Controls;

namespace TestApp.Phone.Pages {
  public partial class MiscPage : PhoneApplicationPage {
    private readonly string _userId1;
    private readonly string _userId2;
    private bool? _isUserId1;

    public MiscPage() {
      InitializeComponent();
      _userId1 = "testUser1";
      _userId2 = "testUser2";
    }

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
      Action<Task<IResult>> logCards = continuation => {
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
