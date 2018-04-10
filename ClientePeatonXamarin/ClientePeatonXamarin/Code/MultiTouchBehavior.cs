using System;
using Xamarin.Forms;

namespace ClientePeatonXamarin.Code
{

    public class MultiTouchBehavior : Behavior<View>
    {

        #region Fields
        private double _currentScale = 1, _startScale = 1, _xOffset, _yOffset;
        private PinchGestureRecognizer _pinchGestureRecognizer;
        private PanGestureRecognizer _panGestureRecognizer;
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
            _parent?.GestureRecognizers.Remove(_panGestureRecognizer);
            _parent?.GestureRecognizers.Add(_panGestureRecognizer);
            _parent?.GestureRecognizers.Remove(_pinchGestureRecognizer);
            _parent?.GestureRecognizers.Add(_pinchGestureRecognizer);
        }

        /// <summary>
        /// Cleanup the events.
        /// </summary>
        private void CleanupEvents()
        {
            _pinchGestureRecognizer.PinchUpdated -= OnPinchUpdated;
            _panGestureRecognizer.PanUpdated -= OnPanUpdated;
            _associatedObject.BindingContextChanged -= AssociatedObjectBindingContextChanged;
        }


        /// <summary>
        /// Initialise the events.
        /// </summary>
        private void InitializeEvents()
        {
            CleanupEvents();
            _pinchGestureRecognizer.PinchUpdated += OnPinchUpdated;
            _panGestureRecognizer.PanUpdated += OnPanUpdated;
            _associatedObject.BindingContextChanged += AssociatedObjectBindingContextChanged;
        }


        /// <summary>
        /// Initialise the Gesture Recognizers.
        /// </summary>
        private void InitialiseRecognizers()
        {
            _pinchGestureRecognizer = new PinchGestureRecognizer();
            _panGestureRecognizer = new PanGestureRecognizer();
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
            _pinchGestureRecognizer = null;
            _panGestureRecognizer = null;
            _associatedObject = null;
            base.OnDetachingFrom(associatedObject);
        }


        /// <summary>
        /// Implements Pan/Translate.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event parameters.</param>
        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (_parent == null)
            {
                return;
            }

            if (!IsTranslateEnabled)
            {
                return;
            }

            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    _parent.Content.TranslationX = _xOffset + e.TotalX;
                    _parent.Content.TranslationY = _yOffset + e.TotalY;
                    break;
                case GestureStatus.Completed:
                    _xOffset = _parent.Content.TranslationX;
                    _yOffset = _parent.Content.TranslationY;
                    break;
            }
        }

        /// <summary>
        /// Implements Pinch/Zoom.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event parameters.</param>
        private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            if (_parent == null)
            {
                return;
            }


            if (!IsScaleEnabled)
            {
                return;
            }

            switch (e.Status)
            {
                case GestureStatus.Started:
                    _startScale = _parent.Content.Scale;
                    _parent.Content.AnchorX = 0;
                    _parent.Content.AnchorY = 0;
                    break;
                case GestureStatus.Running:
                    _currentScale += (e.Scale - 1) * _startScale;
                    _currentScale = Math.Max(1, _currentScale);
                    var renderedX = _parent.Content.X + _xOffset;
                    var deltaX = renderedX / _parent.Width;
                    var deltaWidth = _parent.Width / (_parent.Content.Width * _startScale);
                    var originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;
                    var renderedY = _parent.Content.Y + _yOffset;
                    var deltaY = renderedY / _parent.Height;
                    var deltaHeight = _parent.Height / (_parent.Content.Height * _startScale);
                    var originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;
                    var targetX = _xOffset - (originX * _parent.Content.Width) * (_currentScale - _startScale);
                    var targetY = _yOffset - (originY * _parent.Content.Height) * (_currentScale - _startScale);
                    _parent.Content.TranslationX = targetX.Clamp(-_parent.Content.Width * (_currentScale - 1), 0);
                    _parent.Content.TranslationY = targetY.Clamp(-_parent.Content.Height * (_currentScale - 1), 0);
                    _parent.Content.Scale = _currentScale;
                    break;
                case GestureStatus.Completed:
                    _xOffset = _parent.Content.TranslationX;
                    _yOffset = _parent.Content.TranslationY;
                    break;
            }

        }



        #region IsScaleEnabled property
        /// <summary>
        /// Identifies the <see cref="IsScaleEnabledProperty" /> property.
        /// </summary>
        public static readonly BindableProperty IsScaleEnabledProperty =
            BindableProperty.Create("IsScaleEnabled", typeof(bool), typeof(MultiTouchBehavior), default(bool));


        /// <summary>
        /// Identifies the <see cref="IsScaleEnabled" /> dependency / bindable property.
        /// </summary>
        public bool IsScaleEnabled
        {
            get { return (bool)GetValue(IsScaleEnabledProperty); }
            set { SetValue(IsScaleEnabledProperty, value); }
        }

        #endregion



        #region IsTranslateEnabled property
        /// <summary>
        /// Identifies the <see cref="IsTranslateEnabledProperty" /> property.
        /// </summary>
        public static readonly BindableProperty IsTranslateEnabledProperty =
            BindableProperty.Create("IsTranslateEnabled", typeof(bool), typeof(MultiTouchBehavior), default(bool));

        /// <summary>
        /// Identifies the <see cref="IsTranslateEnabled" /> dependency / bindable property.
        /// </summary>
        public bool IsTranslateEnabled
        {
            get { return (bool)GetValue(IsTranslateEnabledProperty); }
            set { SetValue(IsTranslateEnabledProperty, value); }
        }
        #endregion

    }

}