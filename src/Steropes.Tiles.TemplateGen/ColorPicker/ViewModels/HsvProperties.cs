using Avalonia;
using System;

namespace Steropes.Tiles.TemplateGen.ColorPicker.ViewModels
{
    public class HsvProperties : ColorPickerProperties
    {
        public static readonly StyledProperty<double> HueProperty =
            AvaloniaProperty.Register<HsvProperties, double>(nameof(Hue), 0.0, validate: ValidateHue);

        public static readonly StyledProperty<double> SaturationProperty =
            AvaloniaProperty.Register<HsvProperties, double>(nameof(Saturation), 100.0, validate: ValidateSaturation);

        public static readonly StyledProperty<double> ValueProperty =
            AvaloniaProperty.Register<HsvProperties, double>(nameof(Value), 100.0, validate: ValidateValue);

        static bool ValidateHue(double hue)
        {
            if (hue < 0.0 || hue > 360.0)
            {
                throw new ArgumentException("Invalid Hue value.");
            }
            return true;
        }

        static bool ValidateSaturation(double saturation)
        {
            if (saturation < 0.0 || saturation > 100.0)
            {
                throw new ArgumentException("Invalid Saturation value.");
            }
            return true;
        }

        static bool ValidateValue(double value)
        {
            if (value < 0.0 || value > 100.0)
            {
                throw new ArgumentException("Invalid Value value.");
            }
            return true;
        }

        bool updating;

        public HsvProperties()
        {
            this.GetObservable(HueProperty).Subscribe(_ => UpdateColorPickerValues());
            this.GetObservable(SaturationProperty).Subscribe(_ => UpdateColorPickerValues());
            this.GetObservable(ValueProperty).Subscribe(_ => UpdateColorPickerValues());
        }

        public double Hue
        {
            get { return GetValue(HueProperty); }
            set { SetValue(HueProperty, value); }
        }

        public double Saturation
        {
            get { return GetValue(SaturationProperty); }
            set { SetValue(SaturationProperty, value); }
        }

        public double Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public override void UpdateColorPickerValues()
        {
            if (updating == false && ColorPicker != null)
            {
                updating = true;
                ColorPicker.Value1 = Hue;
                ColorPicker.Value2 = Saturation;
                ColorPicker.Value3 = Value;
                updating = false;
            }
        }

        public override void UpdatePropertyValues()
        {
            if (updating == false && ColorPicker != null)
            {
                updating = true;
                Hue = ColorPicker.Value1;
                Saturation = ColorPicker.Value2;
                Value = ColorPicker.Value3;
                updating = false;
            }
        }
    }
}
