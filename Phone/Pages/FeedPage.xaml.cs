using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using AppboyPlatform.PCL.Models.Incoming.Cards;
using Microsoft.Phone.Controls;

namespace TestApp.Phone.Pages {
  public partial class FeedPage : PhoneApplicationPage {
    private readonly HashSet<CardCategory> _categories;

    public FeedPage() {
      InitializeComponent();
      _categories = new HashSet<CardCategory>();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
      base.OnNavigatedTo(e);
    }

    private void Checkbox_Checked(object sender, RoutedEventArgs e) {
      var cb = sender as CheckBox;
      _categories.Add((CardCategory)Enum.Parse(typeof(CardCategory), cb.Name.ToUpper()));
      updateCards();
    }

    private void updateCards() {
      AppboyFeed.SetCategories(_categories);
    }

    private void PhoneApplicationPage_OrientationChanged(object sender, OrientationChangedEventArgs e) {
      AppboyFeed.OrientationChanged();
    }

    private void Checkbox_Unchecked(object sender, RoutedEventArgs e) {
      var cb = sender as CheckBox;
      _categories.Remove((CardCategory)Enum.Parse(typeof(CardCategory), cb.Name.ToUpper()));
      updateCards();
    }
  }
}
