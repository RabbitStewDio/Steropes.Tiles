using Avalonia;
using System;

namespace Steropes.Tiles.TemplateGen.ColorPicker.ViewModels
{
    public class AlphaProperties : ColorPickerProperties
    {
        public static readonly StyledProperty<double> AlphaProperty =
            AvaloniaProperty.Register<AlphaProperties, double>(nameof(Alpha), 100.0, validate: ValidateAlpha);

        static bool ValidateAlpha(double alpha)
        {
            if (alpha < 0.0 || alpha > 100.0)
            {
                throw new ArgumentException("Invalid Alpha value.");
            }
            return true;
        }

        bool updating;

        public AlphaProperties()
        {
            this.GetObservable(AlphaProperty).Subscribe(_ => UpdateColorPickerValues());
        }

        public double Alpha
        {
            get { return GetValue(AlphaProperty); }
            set { SetValue(AlphaProperty, value); }
        }

        public override void UpdateColorPickerValues()
        {
            if (updating == false && ColorPicker != null)
            {
                updating = true;
                ColorPicker.Value4 = Alpha;
                updating = false;
            }
        }

        public override void UpdatePropertyValues()
        {
            if (updating == false && ColorPicker != null)
            {
                updating = true;
                Alpha = ColorPicker.Value4;
                updating = false;
            }
        }
    }
}
