﻿using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using Lottie.Forms.iOS.Renderers;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Auth;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;
using PanCardView.iOS;
using SSW.Rewards.Services;
using UIKit;
using Xamarin.Forms;

namespace SSW.Rewards.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.SetFlags("Shell_Experimental", "Visual_Experimental", "CollectionView_Experimental", "FastRenderers_Experimental");

            Rg.Plugins.Popup.Popup.Init();

            global::Xamarin.Forms.Forms.Init();
            AnimationViewRenderer.Init();
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            CardsViewRenderer.Preserve();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            Bootstrapper.Init();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
