using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClientePeatonXamarin.iOS.Code;
using CoreGraphics;
using Foundation;
using MapKit;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ClientePeatonXamarin.Code.MapInterRapidisimoView), typeof(MapInterRapidisimoRender))]
namespace ClientePeatonXamarin.iOS.Code
{
    public class MapInterRapidisimoRender : MapRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                var nativeMap = Control as MKMapView;
                if (nativeMap != null)
                {
                    nativeMap.RemoveAnnotations(nativeMap.Annotations);
                    nativeMap.GetViewForAnnotation = null;
                }
            }

            if (e.NewElement != null)
            {
                var formsMap = (ClientePeatonXamarin.Code.MapInterRapidisimoView)e.NewElement;
                var nativeMap = Control as MKMapView;
                nativeMap.GetViewForAnnotation = GetViewForAnnotation;
            }
        }

        MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            MKAnnotationView annotationView = null;
            if (annotation is MKUserLocation)
                return null;

            annotationView = mapView.DequeueReusableAnnotation("Interpin") as MKPinAnnotationView;
            if (annotationView == null)
            {
                annotationView = new MKAnnotationView(annotation, "Interpin")
                {
                    CanShowCallout = true
                };
            }

            annotationView.CalloutOffset = new CGPoint(0, 0);
            annotationView.Annotation = annotation;
            if (annotation.GetTitle() == "Tu Ubicación")
                annotationView.Image = UIImage.FromFile("MiUbicacion.png");
            else
                annotationView.Image = UIImage.FromFile("markerMapSmall.png");


            return annotationView;
        }
    }
}