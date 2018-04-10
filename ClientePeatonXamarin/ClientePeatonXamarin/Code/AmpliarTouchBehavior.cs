using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ClientePeatonXamarin.Code
{
    public class AmpliarTouchBehavior : Behavior<View>
    {
        #region Fields        
        private TapGestureRecognizer tapGestureRecognizer;
        private ContentView _parent;
        private View _associatedObject;
        #endregion


        /// <summary>
        /// Occurs when BindingContext is changed: used to initialise the Gesture Recognizers.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event parameters.</param>
        private void AssociatedObjectBindingContextChanged(object sender, EventArgs e)
        {
            _parent = _associatedObject.Parent as ContentView;
            _parent?.GestureRecognizers.Remove(tapGestureRecognizer);
            _parent?.GestureRecognizers.Add(tapGestureRecognizer);
        }

        /// <summary>
        /// Cleanup the events.
        /// </summary>
        private void CleanupEvents()
        {
            tapGestureRecognizer.Tapped -= TapGestureRecognizer_Tapped;
            _associatedObject.BindingContextChanged -= AssociatedObjectBindingContextChanged;
        }


        /// <summary>
        /// Initialise the events.
        /// </summary>
        private void InitializeEvents()
        {
            CleanupEvents();
            tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
            _associatedObject.BindingContextChanged += AssociatedObjectBindingContextChanged;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Image image = (sender as ContentView)?.Content as Image;
            image?.Navigation?.PushAsync(new Views.VerImagenPage(image?.BindingContext as Modelos.Imagen));
        }


        /// <summary>
        /// Initialise the Gesture Recognizers.
        /// </summary>
        private void InitialiseRecognizers()
        {
            tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.NumberOfTapsRequired = 2;
        }


        /// <summary>
        /// Occurs when Behavior is attached to the View: initialises fields, properties and events.
        /// </summary>
        protected override void OnAttachedTo(View associatedObject)
        {
            InitialiseRecognizers();
            _associatedObject = associatedObject;
            InitializeEvents();
            base.OnAttachedTo(associatedObject);
        }



        /// <summary>
        /// Occurs when Behavior is detached from the View: cleanup fields, properties and events.
        /// </summary>
        protected override void OnDetachingFrom(View associatedObject)
        {
            CleanupEvents();
            _parent = null;
            tapGestureRecognizer = null;
            _associatedObject = null;
            base.OnDetachingFrom(associatedObject);
        }
    }
}
