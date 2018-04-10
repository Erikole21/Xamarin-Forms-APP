using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Xamarin.Forms;

namespace ClientePeatonXamarin.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            Xamarin.FormsMaps.Init("6AHfObxrgR8CgNprVq1R~A4LDbiFUaLoZeEfK9e_dSw~AgKuFnWNx0K9hO46UUhfLD6BEBwG_AmhwOPhEWobIVAC28NOCPpub_BGUkAOWs7G");
            LoadApplication(new ClientePeatonXamarin.App());
        }

    }
}
