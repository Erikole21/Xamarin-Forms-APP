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
    public partial class RecuperarClavePage : ContentPage
    {
        public RecuperarClavePage()
        {
            InitializeComponent();
        }

        public RecuperarClavePage(ViewModels.LoginViewModel Vm)
        {
            InitializeComponent();
            this.BindingContext = Vm;
        }
    }
}