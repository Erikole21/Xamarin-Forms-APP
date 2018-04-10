using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ClientePeatonXamarin.Droid.Code;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ClientePeatonXamarin.Code.MapInterRapidisimoView), typeof(MapInterRapidisimoRender))]
namespace ClientePeatonXamarin.Droid.Code
{
    public class MapInterRapidisimoRender : MapRenderer
    {
        public MapInterRapidisimoRender(Context context) : base(context)
        {

        }

        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            marker.SetTitle(pin.Label);
            marker.SetSnippet(pin.Address);
            if (pin.Address == "Tu Ubicación")            
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.MiUbicacion));            
            else
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.markerMapSmall));

            return marker;

        }
    }
}