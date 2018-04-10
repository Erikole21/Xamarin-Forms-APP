using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace ClientePeatonXamarin.ViewModels
{
    /// <summary>
    /// ViewModelBase    
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        private INavigation navigation;

        public INavigation Navegacion
        {
            get { return navigation; }
            set { navigation = value; }
        }

        public ViewModelBase()
        {

        }

        public ViewModelBase(INavigation _navegacion)
        {
            this.Navegacion = _navegacion;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
