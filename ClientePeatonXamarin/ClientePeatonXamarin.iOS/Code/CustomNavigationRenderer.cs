using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ClientePeatonXamarin.iOS.Code;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(CustomNavigationRenderer))]
namespace ClientePeatonXamarin.iOS.Code
{

    public class CustomNavigationRenderer : NavigationRenderer
    {

        public override void PushViewController(UIKit.UIViewController viewController, bool animated)
        {
            base.PushViewController(viewController, animated);

            var list = new List<UIBarButtonItem>();
            foreach (var item in TopViewController.NavigationItem.RightBarButtonItems)
            {
                if (item.Target == null)
                {
                    continue;
                }                
                UIButton menuButton = new UIButton(UIButtonType.Custom);
                menuButton.SetImage(UIImage.FromFile("MisRecogidas.png"), UIControlState.Normal);
                menuButton.Frame = new RectangleF(0, 0, 32, 32);
                menuButton.AddTarget(item.Target, item.Action, UIControlEvent.TouchUpInside);
                UIBarButtonItem menuItem = new UIBarButtonItem(menuButton);
                list.Add(menuItem);
                TopViewController.NavigationItem.RightBarButtonItems = list.ToArray();
            }
        }
    }

}