using ClientePeatonXamarin.Modelos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace ClientePeatonXamarin.Code
{
    public class MapaBehavior : BindableBehavior<MapInterRapidisimoView>
    {
        MapInterRapidisimoView mapActual;

        protected override void OnAttachedTo(MapInterRapidisimoView map)
        {
            mapActual = map;
            base.OnAttachedTo(map);
        }

        protected override void OnDetachingFrom(MapInterRapidisimoView map)
        {
            base.OnDetachingFrom(map);
            mapActual = null;
        }

        public static readonly BindableProperty PuntosProperty = BindableProperty.Create("Puntos", typeof(ObservableCollection<Pin>), typeof(MapaBehavior), null, BindingMode.TwoWay, null, AgregarPuntosChanged);

        public ObservableCollection<Pin> Puntos
        {
            get { return (ObservableCollection<Pin>)GetValue(PuntosProperty); }
            set { SetValue(PuntosProperty, value); }
        }


        public static readonly BindableProperty UbicacioInicialProperty = BindableProperty.Create("UbicacioInicial", typeof(Posicion), typeof(MapaBehavior), null);

        public Posicion UbicacioInicial
        {
            get { return (Posicion)GetValue(UbicacioInicialProperty); }
            set { SetValue(UbicacioInicialProperty, value); }
        }


        private static void AgregarPuntosChanged(BindableObject view, object oldValue, object newValue)
        {
            var mapBehavior = view as MapaBehavior;
            if (mapBehavior != null)
            {
                mapBehavior.CargarPuntos();
            }
        }

        private void CargarPuntos()
        {
            if (mapActual != null)
            {
                mapActual.Pins.Clear();

                foreach (var punto in Puntos)
                    mapActual.Pins.Add(punto);

                if (UbicacioInicial != null)
                {
                    double distanciaCarga = 0.5;
                    if (Device.RuntimePlatform == Device.UWP)
                        distanciaCarga = 1.5;

                    mapActual.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(UbicacioInicial.Latitud, UbicacioInicial.Longitud), Distance.FromMiles(distanciaCarga)));
                    Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
                    {
                        mapActual.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(UbicacioInicial.Latitud, UbicacioInicial.Longitud), Distance.FromMiles(distanciaCarga)));
                        return false;
                    });
                }
            }
        }

    }

    public class BindableBehavior<T> : Behavior<T> where T : BindableObject
    {

        public T AssociatedObject { get; private set; }

        protected override void OnAttachedTo(T visualElement)
        {
            base.OnAttachedTo(visualElement);
            AssociatedObject = visualElement;
            if (visualElement.BindingContext != null)
                BindingContext = visualElement.BindingContext;

            visualElement.BindingContextChanged += OnBindingContextChanged;
        }


        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }

        protected override void OnDetachingFrom(T view)
        {
            view.BindingContextChanged -= OnBindingContextChanged;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }
    }
}
