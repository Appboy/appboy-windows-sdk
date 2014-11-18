using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using AppboyUI.Universal.Controls;
using AppboyPlatform.PCL.Models.Incoming.Cards;
using System.Collections.Generic;
using System;
using Windows.UI.Xaml.Controls;

namespace AppboyTestApps.Universal.Pages {
  public sealed partial class FeedPage : Page {
    private NavigationHelper _navigationHelper;
    private HashSet<CardCategory> _categories;

    public NavigationHelper NavigationHelper {
      get { return _navigationHelper; }
    }

    public FeedPage() {
      InitializeComponent();
      _navigationHelper = new NavigationHelper(this);
      _navigationHelper.LoadState += navigationHelper_LoadState;
      _navigationHelper.SaveState += navigationHelper_SaveState;
      _categories = new HashSet<CardCategory>();
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

    private void Checkbox_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e) {
      CheckBox cb = sender as CheckBox;
      _categories.Add((CardCategory)Enum.Parse(typeof(CardCategory), cb.Name.ToUpper()));
      updateCards();
    }

    private void updateCards() {
      ((Feed)this.FindName("AppboyFeed")).SetCategories(_categories);
    }

    private void Checkbox_Unchecked(object sender, Windows.UI.Xaml.RoutedEventArgs e) {
      CheckBox cb = sender as CheckBox;
      _categories.Remove((CardCategory)Enum.Parse(typeof(CardCategory), cb.Name.ToUpper()));
      updateCards();
    }
  }
}
