using AppboyPlatform.PCL.Managers;
using AppboyPlatform.Phone;
using AppboyPlatform.Phone.Managers.PushArgs;
using BugSense;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;
using TestApp.Phone.AppboyClasses;
using TestApp.Phone.Resources;

namespace TestApp.Phone {
  // Appboy sessions: The Appboy class exposes methods to open and close sessions named OpenSession and CloseSession. These
  //                  must be called at the proper times during the Windows Phone application lifecycle in order to 
  //                  properly collect session data. For more information about the Windows Phone application lifecycle, 
  //                  see http://msdn.microsoft.com/en-us/library/windowsphone/develop/ff817008(v=vs.105).aspx. The Appboy 
  //                  OpenSession method must be called in Launching and Activated event handlers
  //                  (Application_Launching(object, LaunchingEventArgs) and Application_Activated(object, ActivatedEventArgs)). 
  //                  The CloseSession method must be called in the Deactivated and Closing event handlers 
  //                  (Application_Deactivated(object, DeactivatedEventArgs) and Application_Closing(object, ClosingEventArgs)).
  //                  
  // Appboy slideup: Slideup messages are handled by the Appboy SlideupManager, which is exposed as a property of the Appboy
  //                 class. You can configure the SlideupManager to your applications needs. The slideup user interface is
  //                 provided to the SlideupManager via a SlideupFactory. To use the default Appboy slideup UI, assign
  //                 the AppboyUI.Phone.Factories.SlideupFactory. The default Appboy slideup UI can the themed by overriding 
  //                 the style elements in the Styles/OverrideStyles.xaml located in the AppboyUI.Phone project. If you want
  //                 to provide your own slideup experience, you can implement your own SlideupFactory. The slideup returned 
  //                 from the Slideup factory is animated in from the bottom of the screen by the SlideupManager. When a slideup 
  //                 is clicked, the SlideupClickedEvent is fired. The SlideupManager can also be given a SlideupReceivedDelegate 
  //                 that can control which messages are displayed. To suppress all slideup messages from being displayed, assign 
  //                 a SlideupReceivedDelegate which always returns false.
  //
  // Appboy push notifications: Appboy exposes two push message events. One that fire when a raw push message is received 
  //                            (RawPushReceivedEvent) and another that fires when a toast push message is received 
  //                            (ToastPushReceivedEvent). You can attach event handlers to these events if you want to perform 
  //                            custom logic when these events happen. 
  //                            Appboy tracks push open events by inspecting navigation event arguments. You must add the 
  //                            following line of code to the InitializePhoneApplication method:
  //                            RootFrame.Navigated += Appboy.SharedInstance.PushManager.NavigationEvent;
  public partial class App : Application {
    public static PhoneApplicationFrame RootFrame { get; private set; }

    public App() {
      UnhandledException += Application_UnhandledException;
      InitializeComponent();
      InitializePhoneApplication();
      InitializeLanguage();

      if (Debugger.IsAttached) {
        Application.Current.Host.Settings.EnableFrameRateCounter = true;
        PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
      }

      // Assigns the event handler to be called when a raw push notification is received.
      Appboy.SharedInstance.PushManager.RawPushReceivedEvent += OnRawPushReceived;
      // Assigns the event handler to be called when a toast push notification is received.
      Appboy.SharedInstance.PushManager.ToastPushReceivedEvent += OnToastPushReceived;

      // Sets up the Appboy slideup. The default slideup is provided by the AppboyUI.Phone.Factories.SlideupFactory.
      bool? customSlideup;
      if (IsolatedStorageSettings.ApplicationSettings.TryGetValue(Pages.SettingsPage.CustomSlideupKey, out customSlideup) && (bool) customSlideup) {
        // Assigns a custom Appboy slideup factory. A custom Appboy slideup factory can be used to change the default 
        // Appboy slideup user interface and behavior.
        Appboy.SharedInstance.SlideupManager.SlideupFactory = new SlideupFactory();
      } else {
        // Assigns the default Appboy slideup factory. The slideup will be displayed with the default Appboy slideup UI. The 
        // default Appboy UI can be themed by editing the AppboyUI.Phone stylesheet.
        Appboy.SharedInstance.SlideupManager.SlideupFactory = new AppboyUI.Phone.Factories.SlideupFactory();
      }
      // Assigns a delegate that controls which slideups should be displayed.
      Appboy.SharedInstance.SlideupManager.SetSlideupReceivedDelegate(SlideupReceivedDelegate);
      // Assigns the event handler to be called when a slideup is clicked.
      Appboy.SharedInstance.SlideupManager.SlideupClickedEvent += OnSlideupClicked;
    }

    private static void OnRawPushReceived(object sender, RawPushReceivedEventArgs args) {
      using (System.IO.StreamReader reader = new System.IO.StreamReader(args.Notification.Body)) {
        var message = reader.ReadToEnd();
        Debug.WriteLine("Received a raw push notification with message {0} and we can action on it in the app.", message);
      }
    }

    private static void OnToastPushReceived(object sender, ToastPushReceivedEventArgs args) {
      Debug.WriteLine("Received a toast push notification with the following args.");
      foreach (string key in args.ToastCollection.Keys) {
        string val;
        args.ToastCollection.TryGetValue(key, out val);
        Debug.WriteLine("Toast push notification arguement key = {0} with value = {1}.", key, val);
      }
    }

    private bool SlideupReceivedDelegate(AppboyPlatform.PCL.Models.Incoming.Slideup slideup) {
      return !slideup.Message.Contains("NoShow");
    }

    private static void OnSlideupClicked(object sender, SlideupClickedEventArgs args) {
      Debug.WriteLine("Slideup clicked and we can action on it in the app.");
    }

    // Code to execute when the application is launching (eg, from Start)
    // This code will not execute when the application is reactivated
    private void Application_Launching(object sender, LaunchingEventArgs e) {
      Appboy.SharedInstance.OpenSession();
    }

    // Code to execute when the application is activated (brought to foreground)
    // This code will not execute when the application is first launched
    private void Application_Activated(object sender, ActivatedEventArgs e) {
      Appboy.SharedInstance.OpenSession();
    }

    // Code to execute when the application is deactivated (sent to background)
    // This code will not execute when the application is closing
    private void Application_Deactivated(object sender, DeactivatedEventArgs e) {
      Appboy.SharedInstance.CloseSession();
    }

    // Code to execute when the application is closing (eg, user hit Back)
    // This code will not execute when the application is deactivated
    private void Application_Closing(object sender, ClosingEventArgs e) {
      Appboy.SharedInstance.CloseSession();
    }

    // Code to execute if a navigation fails
    private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e) {
      if (Debugger.IsAttached) {
        // A navigation has failed; break into the debugger
        Debugger.Break();
      }
    }

    // Code to execute on Unhandled Exceptions
    private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e) {
      if (Debugger.IsAttached) {
        // An unhandled exception has occurred; break into the debugger
        Debugger.Break();
      }
    }

    #region Phone application initialization

    // Avoid double-initialization
    private bool phoneApplicationInitialized = false;

    // Do not add any additional code to this method
    private void InitializePhoneApplication() {
      if (phoneApplicationInitialized) {
        return;
      }

      // Create the frame but don't set it as RootVisual yet; this allows the splash
      // screen to remain active until the application is ready to render.
      RootFrame = new PhoneApplicationFrame();
      RootFrame.Navigated += CompleteInitializePhoneApplication;

      // Handle navigation failures
      RootFrame.NavigationFailed += RootFrame_NavigationFailed;

      // Handle reset requests for clearing the backstack
      RootFrame.Navigated += CheckForResetNavigation;

      // Detects and logs to Appboy any push open events.
      RootFrame.Navigated += Appboy.SharedInstance.PushManager.NavigationEvent;

      // Ensure we don't initialize again
      phoneApplicationInitialized = true;
    }

    // Do not add any additional code to this method
    private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e) {
      // Set the root visual to allow the application to render
      if (RootVisual != RootFrame) {
        RootVisual = RootFrame;
      }

      // Remove this handler since it is no longer needed
      RootFrame.Navigated -= CompleteInitializePhoneApplication;
    }

    private void CheckForResetNavigation(object sender, NavigationEventArgs e) {
      // If the app has received a 'reset' navigation, then we need to check
      // on the next navigation to see if the page stack should be reset
      if (e.NavigationMode == NavigationMode.Reset) {
        RootFrame.Navigated += ClearBackStackAfterReset;
      }
    }

    private void ClearBackStackAfterReset(object sender, NavigationEventArgs e) {
      // Unregister the event so it doesn't get called again
      RootFrame.Navigated -= ClearBackStackAfterReset;

      // Only clear the stack for 'new' (forward) and 'refresh' navigations
      if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh) {
        return;
      }

      // For UI consistency, clear the entire page stack
      while (RootFrame.RemoveBackEntry() != null) {
        ; // do nothing
      }
    }

    #endregion

    // Initialize the app's font and flow direction as defined in its localized resource strings.
    //
    // To ensure that the font of your application is aligned with its supported languages and that the
    // FlowDirection for each of those languages follows its traditional direction, ResourceLanguage
    // and ResourceFlowDirection should be initialized in each resx file to match these values with that
    // file's culture. For example:
    //
    // AppResources.es-ES.resx
    //    ResourceLanguage's value should be "es-ES"
    //    ResourceFlowDirection's value should be "LeftToRight"
    //
    // AppResources.ar-SA.resx
    //     ResourceLanguage's value should be "ar-SA"
    //     ResourceFlowDirection's value should be "RightToLeft"
    //
    // For more info on localizing Windows Phone apps see http://go.microsoft.com/fwlink/?LinkId=262072.
    //
    private void InitializeLanguage() {
      try {
        // Set the font to match the display language defined by the
        // ResourceLanguage resource string for each supported language.
        //
        // Fall back to the font of the neutral language if the Display
        // language of the phone is not supported.
        //
        // If a compiler error is hit then ResourceLanguage is missing from
        // the resource file.
        RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);

        // Set the FlowDirection of all elements under the root frame based
        // on the ResourceFlowDirection resource string for each
        // supported language.
        //
        // If a compiler error is hit then ResourceFlowDirection is missing from
        // the resource file.
        FlowDirection flow =
          (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
        RootFrame.FlowDirection = flow;
      } catch {
        // If an exception is caught here it is most likely due to either
        // ResourceLangauge not being correctly set to a supported language
        // code or ResourceFlowDirection is set to a value other than LeftToRight
        // or RightToLeft.

        if (Debugger.IsAttached) {
          Debugger.Break();
        }

        throw;
      }
    }
  }
}