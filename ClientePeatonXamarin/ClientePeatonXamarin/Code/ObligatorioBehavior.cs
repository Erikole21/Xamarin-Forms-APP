using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ClientePeatonXamarin.Code
{
    public class ObligatorioBehavior : Behavior<Entry>
    {
        Entry EntryControl;

        protected override void OnAttachedTo(Entry entry)
        {
            EntryControl = entry;
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
            entry = null;
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            var textValue = args.NewTextValue;
            bool isValid = !string.IsNullOrEmpty(textValue);
            if (isValid)
            {
                if (!IsNumerico)
                {
                    //Si no es numerico valida la longitud
                    if (LongitudMaxima.HasValue && textValue.Length > LongitudMaxima)
                        ((Entry)sender).Text = textValue.Substring(0, LongitudMaxima.Value);
                }
                else
                {
                    if (ValorMaximo.HasValue)
                    {
                        decimal valor;
                        if (!ValidarNumeric(textValue, out valor))
                            ((Entry)sender).Text = valor.ToString();

                        if (valor > ValorMaximo.Value)
                            ((Entry)sender).Text = ValorMaximo.Value.ToString();
                    }

                }

            }
            else
                ((Entry)sender).PlaceholderColor = isValid ? Color.Default : Color.Red;
        }

        private bool ValidarNumeric(string valor, out decimal convertido)
        {
            try
            {
                convertido = Convert.ToDecimal(valor);
                return true;
            }
            catch
            {
                convertido = 0;
                return false;
            }
        }

        /// <summary>
        /// Identifies the <see cref="IsScaleEnabledProperty" /> property.
        /// </summary>
        public static readonly BindableProperty ButtonValidaProperty =
            BindableProperty.Create("ButtonValida", typeof(Button), typeof(ObligatorioBehavior), null);


        /// <summary>
        /// Identifies the <see cref="IsScaleEnabled" /> dependency / bindable property.
        /// </summary>
        public Button ButtonValida
        {
            get { return (Button)GetValue(ButtonValidaProperty); }
            set
            {

                SetValue(ButtonValidaProperty, value);
                if (value != null)
                    value.Clicked += Control_Clicked;
            }
        }


        public static readonly BindableProperty ValorMaximoProperty = BindableProperty.Create("ValorMaximo", typeof(int?), typeof(ObligatorioBehavior), null);

        public int? ValorMaximo { get { return (int?)GetValue(ValorMaximoProperty); } set { SetValue(ValorMaximoProperty, value); } }


        public static readonly BindableProperty LongitudMaximaProperty = BindableProperty.Create("LongitudMaxima", typeof(short?), typeof(ObligatorioBehavior), null);

        public short? LongitudMaxima { get { return (short?)GetValue(LongitudMaximaProperty); } set { SetValue(LongitudMaximaProperty, value); } }


        public static readonly BindableProperty NumericoProperty = BindableProperty.Create("IsNumerico", typeof(bool), typeof(ObligatorioBehavior), false);

        public bool IsNumerico { get { return (bool)GetValue(NumericoProperty); } set { SetValue(NumericoProperty, value); } }


        private void Control_Clicked(object sender, EventArgs e)
        {
            EntryControl.PlaceholderColor = !string.IsNullOrEmpty(EntryControl.Text) ? Color.Default : Color.Red;
        }

    }
}
