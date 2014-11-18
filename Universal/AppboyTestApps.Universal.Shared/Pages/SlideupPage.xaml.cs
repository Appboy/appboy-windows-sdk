using AppboyPlatform.PCL.Managers;
using AppboyPlatform.PCL.Models.Incoming;
using AppboyPlatform.Universal;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AppboyTestApps.Universal.Pages {
  public sealed partial class SlideupPage : Page {
    public const string CustomSlideupFactorySetKey = "_custom_slideup_";

    private ObservableDictionary defaultViewModel = new ObservableDictionary();

    public ObservableDictionary DefaultViewModel {
      get { return defaultViewModel; }
    }

    private NavigationHelper _navigationHelper;

    public NavigationHelper NavigationHelper {
      get { return _navigationHelper; }
    }

    public ObservableCollection<Tuple<string, string>> Extras { get; set; }

    public SlideupPage() {
      InitializeComponent();
      _navigationHelper = new NavigationHelper(this);
      _navigationHelper.LoadState += navigationHelper_LoadState;
      _navigationHelper.SaveState += navigationHelper_SaveState;
      Extras = new ObservableCollection<Tuple<string, string>>();
      ExtrasListBox.ItemsSource = Extras;
    }

    private async void AddExtraButton_Click(object sender, RoutedEventArgs e) {
      if (String.IsNullOrWhiteSpace(ExtraKeyTextBox.Text)) {
        await new MessageDialog("The key cannot be whitespace or empty.").ShowAsync();
        return;
      }
      if (String.IsNullOrWhiteSpace(ExtraValueTextBox.Text)) {
        await new MessageDialog("The value cannot be whitespace or empty.").ShowAsync();
        return;
      }
      Extras.Add(new Tuple<String, String>(ExtraKeyTextBox.Text, ExtraValueTextBox.Text));
      ExtraKeyTextBox.Text = String.Empty;
      ExtraValueTextBox.Text = String.Empty;
    }

    private void RemoveExtraButton_Click(object sender, RoutedEventArgs e) {
      if (ExtrasListBox.SelectedIndex != -1) {
        Extras.RemoveAt(ExtrasListBox.SelectedIndex);
      }
    }

    private async void CreateAndAddSlideupButton_Click(object sender, RoutedEventArgs e) {
      if (SlideFromListBox.SelectedIndex == -1) {
        await new MessageDialog("Please select from which direction the slideup will animate in.").ShowAsync();
        return;
      } else if (ClickActionListBox.SelectedIndex == -1) {
        await new MessageDialog("Please select a ClickAction.").ShowAsync();
        return;
      } else if (DismissTypeListBox.SelectedIndex == -1) {
        await new MessageDialog("Please select a DismissType.").ShowAsync();
        return;
      } else if (ClickActionListBox.SelectedIndex == 1 && (String.IsNullOrWhiteSpace(UriTextBox.Text) || !Uri.IsWellFormedUriString(UriTextBox.Text, UriKind.Absolute))) {
        await new MessageDialog("A well formed URI is required when the ClickAction is set to URI.").ShowAsync();
        return;
      }

      var slideFromText = ((ListBoxItem)SlideFromListBox.SelectedItem).Content.ToString().ToLower();
      SlideFrom slideFrom;
      switch (slideFromText) {
        case "top":
          slideFrom = SlideFrom.TOP;
          break;
        case "bottom":
          slideFrom = SlideFrom.BOTTOM;
          break;
        default:
          await new MessageDialog("Cannot map " + slideFromText + " to a DismissType value.").ShowAsync();
          return;
      }

      var dismissTypeText = ((ListBoxItem)DismissTypeListBox.SelectedItem).Content.ToString().ToLower();
      DismissType dismissType;
      switch (dismissTypeText) {
        case "auto":
          dismissType = DismissType.AUTO_DISMISS;
          break;
        case "swipe":
          dismissType = DismissType.SWIPE;
          break;
        default:
          await new MessageDialog("Cannot map " + dismissTypeText + " to a DismissType value.").ShowAsync();
          return;
      }

      Slideup slideup = new Slideup("This is a test slideup.", slideFrom, dismissType, 5000);

      var clickActionText = ((ListBoxItem)ClickActionListBox.SelectedItem).Content.ToString().ToLower();
      switch (clickActionText) {
        case "news feed":
          slideup.SetClickActionToNewsFeed();
          break;
        case "uri":
          Uri uri = new Uri(UriTextBox.Text);
          slideup.SetClickActionToUri(uri);
          break;
        case "none":
          break;
        default:
          await new MessageDialog("Cannot map " + clickActionText + " to a ClickAction value.").ShowAsync();
          return;
      }

      foreach (Tuple<string, string> extra in Extras) {
        slideup.Extras.Add(extra.Item1, extra.Item2);
      }
      Appboy.SharedInstance.SlideupManager.AddSlideup(slideup);
    }

    private void DisplayNextSlideupButton_Click(object sender, RoutedEventArgs e) {
      Appboy.SharedInstance.SlideupManager.RequestDisplaySlideup();
    }

    private void CustomSlideupFactoryCheckBox_Click(object sender, RoutedEventArgs e) {
      if (CustomSlideupFactoryCheckBox.IsChecked == true) {
        Appboy.SharedInstance.SlideupManager.SlideupControlFactory = new AppboyClasses.SlideupControlFactory();
      } else {
        Appboy.SharedInstance.SlideupManager.SlideupControlFactory = new AppboyUI.Universal.Factories.SlideupControlFactory();
      }
      Windows.Storage.ApplicationData.Current.LocalSettings.Values[CustomSlideupFactorySetKey] = CustomSlideupFactoryCheckBox.IsChecked;
    }

    private void SlideupManagerDelegatesCheckBox_Click(object sender, RoutedEventArgs e) {
      if (SlideupManagerDelegatesCheckBox.IsChecked == true) {
        Appboy.SharedInstance.SlideupManager.SlideupClicked = CustomSlideupClicked;
        Appboy.SharedInstance.SlideupManager.SlideupDismissed = CustomSlideupDismissed;
      } else {
        Appboy.SharedInstance.SlideupManager.SlideupClicked = null;
        Appboy.SharedInstance.SlideupManager.SlideupDismissed = null;
      }
    }

    private void HandleSlideupsInternallyCheckBox_Click(object sender, RoutedEventArgs e) {
      if (HandleSlideupsInternallyCheckBox.IsChecked == true) {
        Appboy.SharedInstance.SlideupManager.SlideupReceived = CustomSlideupReceived;
      } else {
        Appboy.SharedInstance.SlideupManager.SlideupReceived = null;
      }
    }

    private void RequestSlideupFromServerButton_Click(object sender, RoutedEventArgs e) {
      Appboy.SharedInstance.RequestSlideup();
    }

    private bool CustomSlideupClicked(Slideup slideup, SlideupManagerBase.CloseSlideupDelegate closeSlideup) {
      closeSlideup(true);
      new MessageDialog("The ClickAction was " + slideup.ClickAction + " but we're overriding the Click event to cancel it.").ShowAsync();
      return true;
    }

    private async void CustomSlideupDismissed(Slideup slideup) {
      await new MessageDialog("Dismissed the slideup.").ShowAsync();
    }

    private bool CustomSlideupReceived(Slideup slideup) {
      new MessageDialog(slideup.Message).ShowAsync();
      return true;
    }

    private void navigationHelper_LoadState(object sender, LoadStateEventArgs e) {}

    private void navigationHelper_SaveState(object sender, SaveStateEventArgs e) {}

    #region NavigationHelper registration

    protected override void OnNavigatedTo(NavigationEventArgs e) {
      _navigationHelper.OnNavigatedTo(e);
      bool usingCustomSlideupFactory = (bool?)Windows.Storage.ApplicationData.Current.LocalSettings.Values[CustomSlideupFactorySetKey] ?? false;
      CustomSlideupFactoryCheckBox.IsChecked = usingCustomSlideupFactory;
      SlideFromListBox.SelectedIndex = 0;
      ClickActionListBox.SelectedIndex = 0;
      DismissTypeListBox.SelectedIndex = 0;
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e) {
      _navigationHelper.OnNavigatedFrom(e);
    }

    #endregion
  }
}
