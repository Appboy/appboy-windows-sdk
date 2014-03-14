Please e-mail support@appboy.com with any questions.

## Run the Test App
1.  Clone the root repository to a local drive with `git clone --recursive git@github.com:Appboy/appboy-windows-samples.git`.
2.  Make sure nuget package restore is enabled for the solution.  If it isn't, run `nuget restore TestApp.Phone.csproj`.
3.  Build the solution and test on a Windows 8 Phone emulator or device.

## Integration Instructions
#### The following integration instructions demonstrate how this sample is integrated with the Appboy SDK and UI Library.
1. Set up Appboy as an authenticated push sender to your app (see http://msdn.microsoft.com/en-us/library/windowsphone/develop/ff941099(v=vs.105).aspx for MSFT details).  To do this, get a certificate from a Windows Root Certificate Authority (http://social.technet.microsoft.com/wiki/contents/articles/14215.windows-and-windows-phone-8-ssl-root-certificate-program-member-cas.aspx).  Ensure that that the key-usage value is set to `client-authentication`.   Then upload this certificate to Appboy in your App Settings on our dashboard.
2. Using the Visual Studio Nuget Package Manager, install the latest Appboy Windows Phone SDK.
3. Download and add a reference to the Appboy Windows Phone UI Library.  In this TestApp we download the source code by adding a git submodole: `git submodule add "git@github.com:Appboy/windows-phone-ui.git" Externals/AppboyUI.Phone/.` Keeping a git submodule up-to-date requires running `git submodule update --init` when new versions are available.  Optionally, you may choose to download the code directly from `git@github.com:Appboy/windows-phone-ui.git`.
4. Add `assets\windows_store\AppboyConfiguration.xml` to your project in the root directory.
5. Modify the file properties for AppboyConfiguration.xml:
  - Set the `Build Action` to `Content`.
  - Set `Copy to Output Directory` to `Copy always`.
6. Edit the `AppboyConfiguration.xml` file, replacing `YOUR_API_KEY_HERE` with your api key from the Appboy dashboard.
7. In your `WMAppManifest.xml`, ensure that `ID_CAP_LOCATION`, `ID_CAP_PUSH_NOTIFICATION`, and `ID_CAP_IDENTITY_DEVICE` are checked.

## Analytics
1. Call `Appboy.SharedInstance.OpenSession()` in the application's Launching and Activated event handlers. These are found in the App.xaml.cs file and are typically have the following signatures: 
 - `Application_Launching(object, LaunchingEventArgs)`
 - `Application_Activated(object, ActivatedEventArgs)`
2. Call `Appboy.SharedInstance.CloseSession()` in the application's Deactivated and Closing event handlers. These are found in the App.xaml.cs file and are typically have the following signatures:
 - `Application_Deactivated(object, DeactivatedEventArgs)`
 - `Application_Closing(object, ClosingEventArgs)`

## Custom Events
Custom events can be logged by using the `EventLogger`, which is a property exposed in IAppboy. To obtain a reference to the `EventLogger`, call `Appboy.SharedInstance.EventLogger`. You can use the following methods to track important user actions:
 - `bool LogCustomEvent(string eventName)`
 - `bool LogPurchase(string productId, string currencyCode, decimal price)`
 - `bool LogFeedbackDisplayed()`
 - `bool LogFeedDisplayed()`
 - `bool LogFeedCardImpression(string cardId)`
 - `bool LogFeedCardClick(string cardId)`

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

## Push Notifications
1. Add PushChannelName and PushServiceName values to your AppboyConfiguration.xml
<pre><code>&lt;PushChannelName&gt;YOUR_PUSH_CHANNEL_NAME&lt;/PushChannelName&gt;
&lt;PushServiceName&gt;THE_CN_NAME_OF_YOUR_AUTHENTICATED_SENDER_CERT&lt;/PushServiceName&gt;
</code></pre>

2. To listen to events that are fired when the push is received and activated (clicked by user), create event handlers and add them to the PushManager events:
 - `Appboy.SharedInstance.PushManager.RawPushReceivedEvent += YourRawPushReceivedEventHandler;`
 - `Appboy.SharedInstance.PushManager.ToastPushReceivedEvent += YourToastPushReceivedEventHandler;`

<i><b>Note:</b> Only add these event handlers once per app execution.</i>

Your event handlers should have the signatures: 
 - `void YourOnRawPushReceived(object sender, RawPushReceivedEventArgs args);`
 - `void OnToastPushReceived(object sender, ToastPushReceivedEventArgs args);`

Appboy tracks push open events by inspecting navigation event arguments. You must add the following line of code to the `InitializePhoneApplication` method:
 - `RootFrame.Navigated += Appboy.SharedInstance.PushManager.NavigationEvent;`

<i><b>Note:</b> We've added `{tracking_id}_ab_pn_cid` to the launch string from push activations.</i>

## Feedback
1. Call the method `Task<IResult> Appboy.SharedInstance.PostFeedback(string replyToEmail, string message, bool isReportingABug)` to post feedback. The result can be determined by examining the IResult.

<i><b>Note:</b> This must be called after the application has been initialized and is able to make network requests. Feedback will appear under the "Feedback" section of the Appboy dashboard.</i>

## Slideup
1. Slideup messages are handled by the Appboy `SlideupManager`, which is exposed as a property of the Appboy
 class. To use the default Appboy slideup UI, assign the `AppboyUI.Phone.Factories.SlideupControlFactory`. 

## Theming
You can customize the user interface to fit your app by overriding the default styles. The default styles for all of the UI controls are defined in the `AppboyUI.Phone` project in the `Assets/Styles/Default.xaml` file. To override a default style, copy over the style element and one or more of the properties.

<b>Example:</b> Changing the slideup border color to RED 
<pre><code>&lt;Style x:Key="Appboy.Slideup.Border" TargetType="Border"&gt;
  &lt;Setter Property="BorderBrush" Value="#ff0000"/&gt;
  &lt;Setter Property="BorderThickness" Value="1"/&gt;
&lt;/Style&gt;
</code></pre>
