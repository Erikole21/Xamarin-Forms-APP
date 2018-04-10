using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace ClientePeatonXamarin.iOS
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
            global::Xamarin.Forms.Forms.Init();

            //-->Initialize scanner
            global::ZXing.Net.Mobile.Forms.iOS.Platform.Init();

            LoadApplication(new App());

            //Added to prevent iOS linker to strip behaviors assembly out of deployed package.
            ClientePeatonXamarin.Code.Infrastructure.Init();
            Xamarin.FormsMaps.Init();
            MessagingCenter.Subscribe<ImageSource>(this, "Share", Share, null);

            return base.FinishedLaunching(app, options);
        }

        async void Share(ImageSource imageSource)
        {
            var handler = new StreamImagesourceHandler();
            var uiImage = await handler.LoadImageAsync(imageSource);
            var item = NSObject.FromObject(uiImage);
            var activityItems = new[] { item };

            var activityController = new UIActivityViewController(activityItems, null);
            var topController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            while (topController.PresentedViewController != null)
            {
                topController = topController.PresentedViewController;
            }
            topController.PresentViewController(activityController, true, () => { });
        }
    }
}
