Please e-mail support@appboy.com with any questions.

## Run the Test App
1.  Clone the root repository to a local drive with `git clone --recursive git@github.com:Appboy/appboy-windows-samples.git`.
2.  Make sure nuget package restore is enabled for the solution.  If it isn't, run `nuget restore TestApp.Store.csproj`.
3.  Build the solution and test on your local Windows 8 machines.

## Integration Instructions
#### The following integration instructions demonstrate how this sample is integrated with the Appboy SDK and UI Library.
1. Set up your application in the Windows Store and include your SID and Client Secret in your app (Step by step instructions: http://msdn.microsoft.com/en-us/library/windows/apps/hh465407.aspx).
2. Under App Settings in the Appboy dashboard add the SID and Client Secret in your app settings.
3. Using the Visual Studio Nuget Package Manager, install the latest Appboy Windows Store SDK.
4. Download and add a reference to the Appboy Windows Store UI Library.  In this TestApp we download the source code by adding a git submodole: `git submodule add "git@github.com:Appboy/windows-store-ui.git" Externals/AppboyUI.Store/.` Keeping a git submodule up-to-date requires running `git submodule update --init` when new versions are available.  Optionally, you may choose to download the code directly from `git@github.com:Appboy/windows-store-ui.git`.
5. Add `assets\windows_store\AppboyConfiguration.xml` to your project in the root directory.
6. Modify the file properties for `AppboyConfiguration.xml`:
  - Set the `Build Action` to `Content`.
  - Set `Copy to Output Directory` to `Copy always`.
7. Edit `AppboyConfiguration.xml`, replacing `YOUR_API_KEY_HERE` with your api key from the Appboy dashboard.
8. In your `Package.appxmanifest` make sure under Notifications you have Toast capable set to Yes.  Also, ensure that under capabilities you have Locatmion checked.

## Analytics
1. Call `Appboy.SharedInstance.OpenSession()` in the application's OnLaunching and OnActivated lifecycle methods, as well as in a Resuming event handler. 
2. Add `Appboy.SharedInstance.CloseSession()` to a Suspending event handler.

## Custom Events
Custom events can be logged by using the `EventLogger`, which is a property exposed in IAppboy. To obtain a reference to the `EventLogger`, call `Appboy.SharedInstance.EventLogger`. You can use the following methods to track important user actions:
 - `bool LogCustomEvent(string eventName)`
 - `bool LogPurchase(string productId, string currencyCode, decimal price)`
 - `bool LogFeedbackDisplayed()`
 - `bool LogFeedDisplayed()`
 - `bool LogFeedCardImpression(string cardId)`
 - `bool LogFeedCardClick(string cardId)`
 - `bool LogSlideupShown()`
 - `bool LogSlideupClicked()`

## User Attributes
Appboy provides methods for assigning attributes to users. You'll be able to filter and segment your users according to these attributes on the dashboard. User attributes can be assigned to the current `IAppboyUser`. To obtain a reference to the current `IAppboyUser`, call `Appboy.SharedInstance.AppboyUser`. The following attributes are exposed as properties of the `IAppboyUser`:
 - FirstName
 - LastName
 - Email
 - Bio
 - Gender
 - DateOfBirth
 - Country
 - HomeCity
 - IsSubscribedToEmails
 - PhoneNumber
  
Custom attributes can be assigned to a user by using the `SetCustomAttribute` method.
Integer custom attributes can be incremented by using the `IncrementCustomAttribute` method.

## Push
1. To listen to events that are fired when the push is received and activated (clicked by user), create event handlers and add them to the PushManager events:
 - `Appboy.SharedInstance.PushManager.PushReceivedEvent += YourPushReceivedEventHandler;`
 - `Appboy.SharedInstance.PushManager.ToastActivatedEvent += YourToastActivatedEventHandler;`
 
<i><b>Note:</b> Only add these eventhandlers once per app execution.</i>

2. Your event handlers should have the signatures: 
 - `void YourPushReceivedEventHandler(PushNotificationChannel sender, AppboyPushNotificationReceivedEventArgs args);`
 - `void YourToastActivatedEventHandler(ToastNotification sender, AppboyToastActivatedEventArgs args);`

## Feedback
1. Call the method `Task<IResult> Appboy.SharedInstance.PostFeedback(string replyToEmail, string message, bool isReportingABug)` to post feedback. The result can be determined by examining the IResult.

<i><b>Note:</b> This must be called after the application has been initialized and is able to make network requests. Feedback will appear under the "Feedback" section of the Appboy dashboard.</i>

## Slideup
1. Slideup messages are handled by the Appboy `SlideupManager`, which is exposed as a property of the Appboy
 class. To use the default Appboy slideup UI, assign the `AppboyUI.Store.Factories.SlideupFactory`. 
2.  If you want to provide your own slideup experience, you can implement your own `SlideupFactory`. The slideup returned  from the `SlideupFactory` is animated in from the bottom of the screen by the `SlideupManager`. When a slideup is clicked, the `SlideupClickedEvent` is fired. 
3.  The `SlideupManager` can also be given a `SlideupReceivedDelegate` that can control which messages are displayed. To suppress all slideup messages from being displayed, assign a `SlideupReceivedDelegate` which always returns false.

## Theming
You can customize the user interface to fit your app by overriding the default styles. The default styles for all of the UI controls are defined in the `AppboyUI.Store` project in the `Assets/Styles/Default.xaml` file. To override a default style, copy over the style element into the `AppboyStyles.xaml` locaated at the root of your project and change one or more of its properties.

Example: Changing the slideup border color to RED 
<pre><code>&lt;Style x:Key="Appboy.Slideup.Border" TargetType="Border"&gt;
  &lt;Setter Property="BorderBrush" Value="#ff0000"/&gt;
  &lt;Setter Property="BorderThickness" Value="1"/&gt;
&lt;/Style&gt;
</code></pre>
