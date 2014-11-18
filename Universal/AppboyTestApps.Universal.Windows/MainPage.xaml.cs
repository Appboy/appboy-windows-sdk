using AppboyTestApps.Universal.Pages;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AppboyTestApps.Universal {
  public sealed partial class MainPage : Page {
    private readonly NavigationHelper _navigationHelper;

    public MainPage() {
      this.InitializeComponent();
      _navigationHelper = new NavigationHelper(this);
      _navigationHelper.LoadState += navigationHelper_LoadState;
      _navigationHelper.SaveState += navigationHelper_SaveState;
    }

    public NavigationHelper NavigationHelper {
      get { return _navigationHelper; }
    }

    private void navigationHelper_LoadState(object sender, LoadStateEventArgs e) {}

    private void navigationHelper_SaveState(object sender, SaveStateEventArgs e) {}

    private void Feed_Click(object sender, RoutedEventArgs e) {
      Frame.Navigate(typeof(FeedPage));
    }

    private void SocialFeed_Click(object sender, RoutedEventArgs e) {
      Frame.Navigate(typeof(SocialFeedPage));
    }

    private void Feedback_Click(object sender, RoutedEventArgs e) {
      Frame.Navigate(typeof(FeedbackPage));
    }

    private void Profile_Click(object sender, RoutedEventArgs e) {
      Frame.Navigate(typeof(ProfilePage));
    }

    private void Misc_Click(object sender, RoutedEventArgs e) {
      Frame.Navigate(typeof(MiscPage));
    }

    private void Slideups_Click(object sender, RoutedEventArgs e) {
      Frame.Navigate(typeof(SlideupPage));
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
      _navigationHelper.OnNavigatedTo(e);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e) {
      _navigationHelper.OnNavigatedFrom(e);
    }
  }
}
