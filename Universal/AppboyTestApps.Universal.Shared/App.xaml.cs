using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Globalization;
using Windows.Networking.PushNotifications;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227
using Windows.UI.Xaml.Resources;
using AppboyPlatform.PCL.Managers;
using AppboyPlatform.PCL.Models.Incoming;
using AppboyPlatform.Universal;
using AppboyPlatform.Universal.Managers.PushArgs;
using AppboyUI.Universal.Assets.Localization;
using AppboyUI.Universal.Factories;
using AppboyUI.Universal.Popups;
using AppboyTestApps.Universal.Pages;
#if WINDOWS_PHONE_APP
using Windows.Phone.UI.Input;

#endif

namespace AppboyTestApps.Universal {
  /// <summary>
  /// Provides application-specific behavior to supplement the default Application class.
  /// </summary>
  public sealed partial class App : Application {
#if WINDOWS_PHONE_APP
    private TransitionCollection transitions;
#endif

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App() {
      this.InitializeComponent();
      this.Suspending += this.OnSuspending;
      CustomXamlResourceLoader.Current = new TranslationResourceProvider();

      // Assigns the event handler to be called when a push notification is received.
      Appboy.SharedInstance.PushManager.PushReceivedEvent += OnPushReceived;
      // Assigns the event handler to be called when a push notification is opened.
      Appboy.SharedInstance.PushManager.ToastActivatedEvent += OnToastActivated;

      // BASIC SLIDEUP INTEGRATION:
      // Assigns the default Appboy slideup factory. The slideup will be displayed with the default Appboy slideup UI. The 
      // default Appboy UI can be themed by editing the AppboyUI.Universal stylesheet.
      Appboy.SharedInstance.SlideupManager.SlideupControlFactory = new SlideupControlFactory();
      // Sets the feed modal that should be displayed when a slideup with ClickAction set to NEWS_FEED is clicked.
      Appboy.SharedInstance.SlideupManager.FeedModal = new FeedPopup();
      // Note: A NavigationEventHandler must be set in the OnLaunched method below in order to correctly reposition 
      // slideups after page navigations. See below.

      // ADVANCED SLIDEUP INTEGRATION OPTIONS:
      // Sets up the Appboy slideup. The default slideup is provided by the AppboyUI.Universal.Factories.SlideupControlFactory.
      bool customSlideupEnabled = (bool?)ApplicationData.Current.LocalSettings.Values[SlideupPage.CustomSlideupFactorySetKey] ?? false;
      if (customSlideupEnabled) {
        // Assigns a custom Appboy slideup factory. A custom Appboy slideup factory can be used to change the default 
        // Appboy slideup user interface and behavior.
        Appboy.SharedInstance.SlideupManager.SlideupControlFactory = new AppboyClasses.SlideupControlFactory();
      }

#if WINDOWS_PHONE_APP
      HardwareButtons.BackPressed += HardwareButtons_BackPressed;
#endif
    }

#if WINDOWS_PHONE_APP
    private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e) {
      var rootFrame = Window.Current.Content as Frame;
      if (rootFrame != null && rootFrame.CanGoBack) {
        rootFrame.GoBack();
        e.Handled = true;
      }
    }
#endif

    /// <summary>
    /// Invoked when the application is launched normally by the end user.  Other entry points
    /// will be used when the application is launched to open a specific file, to display
    /// search results, and so forth.
    /// </summary>
    /// <param name="e">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs e) {
      Appboy.SharedInstance.OpenSession();
#if DEBUG
      if (System.Diagnostics.Debugger.IsAttached) {
        this.DebugSettings.EnableFrameRateCounter = true;
      }
#endif

      Frame rootFrame = Window.Current.Content as Frame;

      // Do not repeat app initialization when the Window already has content,
      // just ensure that the window is active
      if (rootFrame == null) {
        rootFrame = new Frame();
        rootFrame.Language = ApplicationLanguages.Languages[0];
        rootFrame.NavigationFailed += OnNavigationFailed;
        rootFrame.Navigated += Appboy.SharedInstance.SlideupManager.NavigationEvent;
        if (e.PreviousExecutionState == ApplicationExecutionState.Terminated) {
          //TODO: Load state from previously suspended application
        }
        Window.Current.Content = rootFrame;
      }


      if (rootFrame.Content == null) {
#if WINDOWS_PHONE_APP
        // Removes the turnstile navigation for startup.
        if (rootFrame.ContentTransitions != null) {
          this.transitions = new TransitionCollection();
          foreach (var c in rootFrame.ContentTransitions) {
            this.transitions.Add(c);
          }
        }

        rootFrame.ContentTransitions = null;
        rootFrame.Navigated += this.RootFrame_FirstNavigated;
#endif

        // When the navigation stack isn't restored navigate to the first page,
        // configuring the new page by passing required information as a navigation
        // parameter
        if (!rootFrame.Navigate(typeof(MainPage), e.Arguments)) {
          throw new Exception("Failed to create initial page");
        }
      }

      // Ensure the current window is active
      Window.Current.Activate();
    }

#if WINDOWS_PHONE_APP
    /// <summary>
    /// Restores the content transitions after the app has launched.
    /// </summary>
    /// <param name="sender">The object where the handler is attached.</param>
    /// <param name="e">Details about the navigation event.</param>
    private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e) {
      var rootFrame = sender as Frame;
      rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
      rootFrame.Navigated -= this.RootFrame_FirstNavigated;
      Appboy.SharedInstance.OpenSession();
    }
#endif


    private static void OnPushReceived(PushNotificationChannel sender, AppboyPushNotificationReceivedEventArgs args) {
      Debug.WriteLine("Push notification received of type {0} and we can action on it in the app.", args.pushNotificationReceivedEventArgs.NotificationType);
    }

    private static void OnToastActivated(ToastNotification sender, AppboyToastActivatedEventArgs args) {
      Debug.WriteLine("Push notification opened and we can action on it in the app.");
    }

    private bool SlideupReceivedDelegate(Slideup slideup) {
      return !slideup.Message.Contains("NoShow");
    }

    private static void OnSlideupClicked(object sender, SlideupClickedEventArgs args) {
      Debug.WriteLine("Slideup clicked and we can action on it in the app.");
    }

    private void OnNavigationFailed(object sender, NavigationFailedEventArgs e) {
      throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
    }


    /// <summary>
    /// Invoked when application execution is being suspended.  Application state is saved
    /// without knowing whether the application will be terminated or resumed with the contents
    /// of memory still intact.
    /// </summary>
    /// <param name="sender">The source of the suspend request.</param>
    /// <param name="e">Details about the suspend request.</param>
    private void OnSuspending(object sender, SuspendingEventArgs e) {
      var deferral = e.SuspendingOperation.GetDeferral();

      // TODO: Save application state and stop any background activity
      deferral.Complete();
    }
  }
}
