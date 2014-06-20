using AppboyPlatform.PCL.Managers;
using AppboyPlatform.Store;
using AppboyPlatform.Store.Managers.PushArgs;
using AppboyUI.Store.Factories;
using System;
using System.Diagnostics;
using TestApp.Store.AppboyClasses;
using TestApp.Store.Pages;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Networking.PushNotifications;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using AppboyUI.Store.Assets.Localization;
using Windows.UI.Xaml.Resources;

namespace TestApp.Store {
  // Appboy sessions: The Appboy class exposes methods to open and close sessions named OpenSession and CloseSession. These
  //                  must be called at the proper times during the Windows Store application lifecycle in order to 
  //                  properly collect session data. For more information about the Windows Store application lifecycle, 
  //                  see http://msdn.microsoft.com/en-us/library/windows/apps/hh464925.aspx. The Appboy OpenSession method 
  //                  must be called in the OnLaunched(LaunchActivatedEventArgs) method, OnActivated(IActivatedEventArgs), 
  //                  and in a Resuming event handler. The CloseSession method must be called in a Suspending event handler.
  //                  
  // Appboy slideup: Slideup messages are handled by the Appboy SlideupManager, which is exposed as a property of the Appboy
  //                 class. You can configure the SlideupManager to your applications needs. The default UI used to display 
  //                 slideup messages is provided by the AppboyUI.Phone.Factories.SlideupControlFactory class. The default UI 
  //                 can be changed by either overriding the default slideup UI styles (see AppboyStyles.xaml) or by setting 
  //                 a custom ISlideupControlFactory using the SlideupManager.SlideupControlFactory property. When a new slideup 
  //                 arrives, the default behavior is to display it immediately (or put it onto to the top of a slideup stack 
  //                 if another slideup is currently being displayed. The SlideupManager provides delegate methods that can be 
  //                 used to override the default slideup handling.
  //
  // Appboy push notifications: Appboy exposes two push message events. One that fire when a push message is received 
  //                            (PushReceivedEvent) and another that fires when a push message is opened (ToastActivatedEvent). 
  //                            You can attach event handlers to these events if you want to perform custom logic when these 
  //                            events happen.
  sealed partial class App : Application {
    public App() {
      this.InitializeComponent();

      // Setting event handlers for the application lifecycle events. 
      this.Resuming += OnResuming;
      this.Suspending += OnSuspending;

      CustomXamlResourceLoader.Current = new TranslationResourceProvider();

      // Assigns the event handler to be called when a push notification is received.
      Appboy.SharedInstance.PushManager.PushReceivedEvent += OnPushReceived;
      // Assigns the event handler to be called when a push notification is opened.
      Appboy.SharedInstance.PushManager.ToastActivatedEvent += OnToastActivated;

      // BASIC SLIDEUP INTEGRATION:
      // Assigns the default Appboy slideup factory. The slideup will be displayed with the default Appboy slideup UI. The 
      // default Appboy UI can be themed by editing the AppboyUI.Store stylesheet.
      Appboy.SharedInstance.SlideupManager.SlideupControlFactory = new AppboyUI.Store.Factories.SlideupControlFactory();
      // Sets the feed modal that should be displayed when a slideup with ClickAction set to NEWS_FEED is clicked.
      Appboy.SharedInstance.SlideupManager.FeedModal = new AppboyUI.Store.Popups.FeedPopup();
      // Note: A NavigationEventHandler must be set in the OnLaunched method below in order to correctly reposition 
      // slideups after page navigations. See below.

      // ADVANCED SLIDEUP INTEGRATION OPTIONS:
      // Sets up the Appboy slideup. The default slideup is provided by the AppboyUI.Store.Factories.SlideupControlFactory.
      var customSlideupEnabled = (bool?)Windows.Storage.ApplicationData.Current.LocalSettings.Values[SlideupPage.CustomSlideupFactorySetKey] ?? false;
      if (customSlideupEnabled) {
        // Assigns a custom Appboy slideup factory. A custom Appboy slideup factory can be used to change the default 
        // Appboy slideup user interface and behavior.
        Appboy.SharedInstance.SlideupManager.SlideupControlFactory = new AppboyClasses.SlideupControlFactory();
      }
    }

    /// <summary>
    /// Invoked when the application is launched normally by the end user.  Other entry points
    /// will be used such as when the application is launched to open a specific file.
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
      if (rootFrame == null) {
        rootFrame = new Frame();
        rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];
        rootFrame.NavigationFailed += OnNavigationFailed;
        rootFrame.Navigated += Appboy.SharedInstance.SlideupManager.NavigationEvent;
        if (e.PreviousExecutionState == ApplicationExecutionState.Terminated) {
          //TODO: Load state from previously suspended application
        }
        Window.Current.Content = rootFrame;
      }

      if (rootFrame.Content == null) {
        rootFrame.Navigate(typeof(MainPage), e.Arguments);
      }
      Window.Current.Activate();
    }

    private static void OnPushReceived(PushNotificationChannel sender, AppboyPushNotificationReceivedEventArgs args) {
      Debug.WriteLine("Push notification received of type {0} and we can action on it in the app.", args.pushNotificationReceivedEventArgs.NotificationType);
    }

    private static void OnToastActivated(ToastNotification sender, AppboyToastActivatedEventArgs args) {
      Debug.WriteLine("Push notification opened and we can action on it in the app.");
    }

    private bool SlideupReceivedDelegate(AppboyPlatform.PCL.Models.Incoming.Slideup slideup) {
      return !slideup.Message.Contains("NoShow");
    }

    private static void OnSlideupClicked(object sender, SlideupClickedEventArgs args) {
      Debug.WriteLine("Slideup clicked and we can action on it in the app.");
    }

    void OnNavigationFailed(object sender, NavigationFailedEventArgs e) {
      throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
    }

    protected override void OnActivated(IActivatedEventArgs args) {
      base.OnActivated(args);
      Appboy.SharedInstance.OpenSession();
    }

    /// <summary>
    /// Invoked when the application transitions from Suspended state to Running state.
    /// </summary>
    void OnResuming(object sender, object e) {
      Appboy.SharedInstance.OpenSession();
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
      Appboy.SharedInstance.CloseSession();
      deferral.Complete();
    }
  }
}
