using AppboyPlatform.PCL.Managers;
using AppboyPlatform.PCL.Models.Incoming;
using AppboyPlatform.Phone;
using Microsoft.Phone.Controls;
using System;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using TestApp.Phone.AppboyClasses;

namespace TestApp.Phone.Pages {
  public partial class SlideupPage : PhoneApplicationPage {
    public const string CustomSlideupFactorySetKey = "_custom_slideup_";
    public const string UrlMapperSetKey = "_url_mapper_";

    public ObservableCollection<Tuple<string, string>> Extras { get; set; }

    public SlideupPage() {
      InitializeComponent();
      Extras = new ObservableCollection<Tuple<string, string>>();
      ExtrasListBox.ItemsSource = Extras;
    }

    protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e) {
      base.OnNavigatedTo(e);
      bool usingCustomSlideupFactory;
      IsolatedStorageSettings.ApplicationSettings.TryGetValue(CustomSlideupFactorySetKey, out usingCustomSlideupFactory);
      CustomSlideupFactoryCheckBox.IsChecked = usingCustomSlideupFactory;
      bool usingUrlMapper;
      IsolatedStorageSettings.ApplicationSettings.TryGetValue(UrlMapperSetKey, out usingUrlMapper);
      UrlMapperCheckBox.IsChecked = usingUrlMapper;
      SlideFromListBox.SelectedIndex = 0;
      ClickActionListBox.SelectedIndex = 0;
      DismissTypeListBox.SelectedIndex = 0;
    }

    private void AddExtraButton_Click(object sender, RoutedEventArgs e) {
      if (String.IsNullOrWhiteSpace(ExtraKeyTextBox.Text)) {
        MessageBox.Show("The key cannot be whitespace or empty.");
        return;
      } else if (String.IsNullOrWhiteSpace(ExtraValueTextBox.Text)) {
        MessageBox.Show("The value cannot be whitespace or empty.");
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

    private void CreateAndAddSlideupButton_Click(object sender, RoutedEventArgs e) {
      if (SlideFromListBox.SelectedIndex == -1) {
        MessageBox.Show("Please select from which direction the slideup will animate in.");
        return;
      } else if (ClickActionListBox.SelectedIndex == -1) {
        MessageBox.Show("Please select a ClickAction.");
        return;
      } else if (DismissTypeListBox.SelectedIndex == -1) {
        MessageBox.Show("Please select a DismissType.");
        return;
      } else if (ClickActionListBox.SelectedIndex == 1 && (String.IsNullOrWhiteSpace(UriTextBox.Text) || !Uri.IsWellFormedUriString(UriTextBox.Text, UriKind.Absolute))) {
        MessageBox.Show("A well formed URI is required when the ClickAction is set to URI.");
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
          MessageBox.Show("Cannot map {0} to a SlideFrom value.");
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
          MessageBox.Show("Cannot map {0} to a DismissType value.");
          return;
      }

      var slideup = new Slideup("This is a test slideup.", slideFrom, dismissType, 5000);

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
          MessageBox.Show("Cannot map {0} to a ClickAction value.");
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
        Appboy.SharedInstance.SlideupManager.SlideupControlFactory = new SlideupControlFactory();
      } else {
        Appboy.SharedInstance.SlideupManager.SlideupControlFactory = new AppboyUI.Phone.Factories.SlideupControlFactory();
      }
      IsolatedStorageSettings.ApplicationSettings[CustomSlideupFactorySetKey] = CustomSlideupFactoryCheckBox.IsChecked;
      IsolatedStorageSettings.ApplicationSettings.Save();
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

    private void UrlMapperCheckBox_Click(object sender, RoutedEventArgs e) {
      if (UrlMapperCheckBox.IsChecked == true) {
        Appboy.SharedInstance.UriMapper = new CustomUriMapper();
      } else {
        Appboy.SharedInstance.UriMapper = null;
      }
      IsolatedStorageSettings.ApplicationSettings[UrlMapperSetKey] = UrlMapperCheckBox.IsChecked;
      IsolatedStorageSettings.ApplicationSettings.Save();
    }

    private void RequestSlideupFromServerButton_Click(object sender, RoutedEventArgs e) {
      Appboy.SharedInstance.RequestSlideup();
    }

    private void HideCurrentSlideupButton_Click(object sender, RoutedEventArgs e) {
      Appboy.SharedInstance.SlideupManager.HideCurrentSlideup(true);
    }

    private bool CustomSlideupClicked(Slideup slideup, SlideupManagerBase.CloseSlideupDelegate closeSlideup) {
      closeSlideup(true);
      MessageBox.Show("The ClickAction was " + slideup.ClickAction + " but we're overriding the Click event to cancel it.");
      return true;
    }

    private void CustomSlideupDismissed(Slideup slideup) {
      MessageBox.Show("Dismissed the slideup.");
    }

    private bool CustomSlideupReceived(Slideup slideup) {
      MessageBox.Show(slideup.Message);
      return true;
    }
  }
}