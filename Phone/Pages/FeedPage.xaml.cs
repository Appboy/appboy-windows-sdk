using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using AppboyPlatform.PCL.Models.Incoming.Cards;
using System.Collections.Generic;
using System.Windows.Controls;

namespace TestApp.Phone.Pages {
  public partial class FeedPage : PhoneApplicationPage {

    private HashSet<CardCategory> _categories;

    public FeedPage() {
      InitializeComponent();
      _categories = new HashSet<CardCategory>();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
      base.OnNavigatedTo(e);
    }

    private void Checkbox_Checked(object sender, RoutedEventArgs e) {
      CheckBox cb = sender as CheckBox;
      _categories.Add((CardCategory)Enum.Parse(typeof(CardCategory), cb.Name.ToUpper()));
      updateCards();
    }

    private void updateCards() {
      this.AppboyFeed.SetCategories(_categories);
    }

    private void Checkbox_Unchecked(object sender, RoutedEventArgs e) {
      CheckBox cb = sender as CheckBox;
      _categories.Remove((CardCategory)Enum.Parse(typeof(CardCategory), cb.Name.ToUpper()));
      updateCards();
    }
  }
}