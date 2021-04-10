using Avalonia;
using Avalonia.Media;
using System;

namespace Steropes.Tiles.TemplateGen.ColorPicker.ViewModels
{
    public class HexProperties : ColorPickerProperties
    {
        public static readonly StyledProperty<string> HexProperty =
            AvaloniaProperty.Register<HexProperties, string>(nameof(Hex), "#FFFF0000", validate: ValidateHex);

        static bool ValidateHex(string hex)
        {
            if (!ColorHelpers.IsValidHexColor(hex))
            {
                throw new ArgumentException("Invalid Hex value.");
            }
            return true;
        }

        bool updating;

        public HexProperties()
        {
            this.GetObservable(HexProperty).Subscribe(_ => UpdateColorPickerValues());
        }

        public string Hex
        {
            get { return GetValue(HexProperty); }
            set { SetValue(HexProperty, value); }
        }

        public override void UpdateColorPickerValues()
        {
            if (updating == false && ColorPicker != null)
            {
                updating = true;
                Color color = Color.Parse(Hex);
                ColorHelpers.FromColor(color, out double h, out double s, out double v, out double a);
                ColorPicker.Value1 = h;
                ColorPicker.Value2 = s;
                ColorPicker.Value3 = v;
                ColorPicker.Value4 = a;
                updating = false;
            }
        }

        public override void UpdatePropertyValues()
        {
            if (updating == false && ColorPicker != null)
            {
                updating = true;
                var color = ColorHelpers.FromHSVA(ColorPicker.Value1, ColorPicker.Value2, ColorPicker.Value3, ColorPicker.Value4);
                Hex = ColorHelpers.ToHexColor(color);
                updating = false;
            }
        }
    }
}
