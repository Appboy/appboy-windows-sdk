## 2.0.0.0
  - Removes UI elements from the SDK - support for analytics and push remains
  - Removes error log accessing Windows.UI.Core.CoreWindow.get_Bounds() 
  - Locks Lex.db dependency to 1.2.5
## 1.3.2.0
  - Adds persistence of deviceId to prevent hardware changes from changing the deviceId.
  - Adds methods SetCustomAttributeArray, RemoveFromCustomAttributeArray, and AddToCustomAttributeArray.  These methods can be used to set or modify custom attributes arrays that can be used for filtering and segmentation in the Appboy dashboard.
    For example, watched_genres = [comedy, horror, drama].

## 1.2.0.0
- Version 1.2.0.0 adds a universal library to support Windows Phone 8.1 and Windows Store apps.  It also adds CHANGELOGs to the UI libraries for the Phone, Store, and Universal libraries, for more granularity and divergent updates.  The store library is now being sunsetted; the universal library is taking it's place.

## 1.1.0.0
Appboy version 1.1.0.0 provides a substantial upgrade to the slideup code and reorganization for better flexibility moving forward, but at the expense of a number of breaking changes. We've detailed the changes in this changelog and hope that you'll love the added power, increased flexibility, and improved UI that the new Appboy slideup provides. If you have any trouble with these changes, feel free to reach out to success@appboy.com for help, but most migrations to the new code structure should be relatively painless.

New SlideupManager
- Delegate methods have been added to control which slideups are displayed, when they are displayed, as well as what action(s) to perform when a slideup is clicked or dismissed.
- Default handling of actions assigned to the slideup from the Appboy dashboard.
- Slideups can be dismissed by swiping away the view to either the left or the right. 
- Repositions the slideup above the AppBar or below the System tray on navigation events.
- Custom slideups created locally in the app can be provided to the SlideupManager which will be displayed using the slideup UI.

Updated SlideupControlFactory
- The GetSlideupControl method replaces the previous GetSlideup method and now provides access to the entire Slideup object. This allows for better customization of the slideup. For example, if clicking on the slideup will navigate the user to the Appboy news feed, you can choose to add a visual indicator that will let the user know that clicking on the slideup will perform an action.

Slideup model
- A key value ```extras``` Dictionary has been added to provide additional data to the slideup. ```Extras``` can be on defined on a per slideup basis via the dashboard.
- The ```SlideFrom``` property defines whether the slideup originates from the top or the bottom of the screen.
- The ```DismissType``` property controls whether the slideup will dismiss automatically after a period of time has lapsed, or if it will wait for interaction with the user before disappearing. 
  - The slideup will be dismissed automatically after the number of milliseconds defined by the duration field have elapsed if the slideup's DismissType is set to AUTO_DISMISS.
- The ClickAction property defines the behavior after the slideup is clicked: display a news feed, redirect to a uri, or nothing but dismissing the slideup. This can be changed by calling any of the following methods: ```SetClickActionToNewsFeed()```, ```SetClickActionToUri(Uri uri)```, or ```SetClickActionToNone()```.
- The uri field defines the uri string that the slide up will open when the ClickAction is set to URI. To change this value, use the ```setClickActionToUri(Uri uri)``` method.
- Convenience methods to track slideup impression and click events have been added to the ```Slideup``` class.
  - Impression and click tracking methods have been removed from the ```EventLogger```.
- A public constructor has been exposed to allow you to create custom slideups.

Windows Phone only
- A UriMapper has been added to be able to point the SDK to your integrated Appboy news feed.
  - If the UriMapper is set, slideups with ClickAction set to NEWS_FEED will open the integrated news feed.

Windows Store only
- A modal version of the Appboy news feed was added to the UI library (ModalFeed).
  - If the ModalFeed is set on the SlideupManager, slideups with ClickAction set to NEWS_FEED will open the modal news feed.

## 1.0
* Initial release
