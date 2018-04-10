using ClientePeatonXamarin.UWP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls.Maps;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.UWP;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(ClientePeatonXamarin.Code.MapInterRapidisimoView), typeof(MapInterRapidisimoRender))]
namespace ClientePeatonXamarin.UWP
{
    public class MapInterRapidisimoRender : MapRenderer
    {
        MapControl nativeMap;

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                nativeMap.Children.Clear();
                nativeMap = null;
            }

            if (e.NewElement != null)
            {
                var formsMap = (ClientePeatonXamarin.Code.MapInterRapidisimoView)e.NewElement;
                nativeMap = Control as MapControl;
                nativeMap.Children.Clear();
                nativeMap.MapElements.Clear();

                List<MapElement> Landmarks = new List<MapElement>();

                foreach (var pin in formsMap.Pins)
                {
                    var snPosition = new BasicGeoposition { Latitude = pin.Position.Latitude, Longitude = pin.Position.Longitude };
                    var snPoint = new Geopoint(snPosition);

                    var mapIcon = new MapIcon();
                    if (pin.Address == "Tu Ubicación")
                        mapIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/MiUbicacion.png"));
                    else
                        mapIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/markerMapSmall.png"));

                    mapIcon.CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible;
                    mapIcon.Location = snPoint;
                    mapIcon.NormalizedAnchorPoint = new Windows.Foundation.Point(0.5, 1.0);

                    nativeMap.MapElements.Add(mapIcon);
                }                
            }
        }
    }
}
