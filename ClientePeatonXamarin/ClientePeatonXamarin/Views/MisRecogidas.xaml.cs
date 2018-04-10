using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClientePeatonXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MisRecogidas : ContentPage
    {
        public MisRecogidas()
        {
            InitializeComponent();
        }

        public MisRecogidas(ViewModels.MisRecogidasViewModel Vm)
        {
            InitializeComponent();
            this.BindingContext = Vm;
        }
    }
}