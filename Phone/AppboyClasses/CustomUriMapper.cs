using System;
using System.Windows.Navigation;

namespace TestApp.Phone.AppboyClasses {
  public class CustomUriMapper : UriMapperBase {
    public override Uri MapUri(Uri uri) {
      var decodedUri = System.Net.HttpUtility.UrlDecode(uri.ToString()).ToLower();
      // Appboy may, at times, request to show the news feed. This can happen if a slideup with ClickAction 
      // set to NEWS_FEED is clicked. It will use the mapper and pass in an absolute uri with string 
      // 'appboy://newsfeed'. The app register a mapper that will redirect to the integrated news feed. To 
      // do so, return a Uri to the page where the news feed has been integrated.
      if (decodedUri.Contains("appboy://newsfeed")) {
        return new Uri("/Pages/FeedPage.xaml", UriKind.Relative);
      }
      return uri;
    }
  }
}
