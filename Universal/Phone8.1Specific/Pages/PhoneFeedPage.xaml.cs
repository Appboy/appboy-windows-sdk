using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TestApp.Store.Common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using AppboyPlatform.PCL.Models.Incoming.Cards;
using System.Collections.Generic;
using System;
using AppboyUI.Universal.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TestApp.Phone._8._1.Phone8._1Specific.Pages {
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class PhoneFeedPage : Page {
    private NavigationHelper _navigationHelper;
    private HashSet<CardCategory> _categories;

    public NavigationHelper NavigationHelper {
      get { return _navigationHelper; }
    }

    public PhoneFeedPage() {
      InitializeComponent();
      _navigationHelper = new NavigationHelper(this);
      _navigationHelper.LoadState += navigationHelper_LoadState;
      _navigationHelper.SaveState += navigationHelper_SaveState;
      _categories = new HashSet<CardCategory>();
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
