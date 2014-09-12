using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using TestApp.Phone._8._1.Phone8._1Specific.Pages;
using TestApp.Store.Common;

namespace TestApp.Store.Pages {
  public sealed partial class MainPage : Page {
    private NavigationHelper _navigationHelper;

    public NavigationHelper NavigationHelper {
      get { return _navigationHelper; }
    }

    public MainPage() {
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

    private void Feed_Click(object sender, RoutedEventArgs e) {
      Frame.Navigate(typeof(PhoneFeedPage));
    }
    
    private void Feedback_Click(object sender, RoutedEventArgs e) {
      Frame.Navigate(typeof(AppboyFeedbackPage));
    }

    private void Profile_Click(object sender, RoutedEventArgs e) {
      Frame.Navigate(typeof(ProfilePage));
    }

    private void Misc_Click(object sender, RoutedEventArgs e) {
      Frame.Navigate(typeof(MiscPage));
    }

    private void Social_Feed_Click(object sender, RoutedEventArgs e) {
      Frame.Navigate(typeof(AppboySocialFeedPage));
    }

    private void Slideups_Click(object sender, RoutedEventArgs e) {
      Frame.Navigate(typeof(SlideupPage));
    }
  }
}
