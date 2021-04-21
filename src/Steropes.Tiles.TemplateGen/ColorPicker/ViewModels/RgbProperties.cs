using Avalonia;
using Steropes.Tiles.TemplateGen.ColorPicker.Colors;
using System;

namespace Steropes.Tiles.TemplateGen.ColorPicker.ViewModels
{
    public class RgbProperties : ColorPickerProperties
    {
        public static readonly StyledProperty<byte> RedProperty =
            AvaloniaProperty.Register<RgbProperties, byte>(nameof(Red), 0xFF);

        public static readonly StyledProperty<byte> GreenProperty =
            AvaloniaProperty.Register<RgbProperties, byte>(nameof(Green));

        public static readonly StyledProperty<byte> BlueProperty =
            AvaloniaProperty.Register<RgbProperties, byte>(nameof(Blue));


        bool updating;

        public RgbProperties()
        {
            this.GetObservable(RedProperty).Subscribe(_ => UpdateColorPickerValues());
            this.GetObservable(GreenProperty).Subscribe(_ => UpdateColorPickerValues());
            this.GetObservable(BlueProperty).Subscribe(_ => UpdateColorPickerValues());
        }

        public byte Red
        {
            get { return GetValue(RedProperty); }
            set { SetValue(RedProperty, value); }
        }

        public byte Green
        {
            get { return GetValue(GreenProperty); }
            set { SetValue(GreenProperty, value); }
        }

        public byte Blue
        {
            get { return GetValue(BlueProperty); }
            set { SetValue(BlueProperty, value); }
        }

        public override void UpdateColorPickerValues()
        {
            if (updating == false && ColorPicker != null)
            {
                updating = true;
                RGB rgb = new RGB(Red, Green, Blue);
                HSV hsv = rgb.ToHSV();
                ColorPicker.Value1 = hsv.H;
                ColorPicker.Value2 = hsv.S;
                ColorPicker.Value3 = hsv.V;
                updating = false;
            }
        }

        public override void UpdatePropertyValues()
        {
            if (updating == false && ColorPicker != null)
            {
                updating = true;
                HSV hsv = new HSV(ColorPicker.Value1, ColorPicker.Value2, ColorPicker.Value3);
                RGB rgb = hsv.ToRGB();
                Red = (byte)rgb.R;
                Green = (byte)rgb.G;
                Blue = (byte)rgb.B;
                updating = false;
            }
        }
    }
}
