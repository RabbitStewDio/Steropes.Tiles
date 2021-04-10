using Avalonia;
using System;

namespace Steropes.Tiles.TemplateGen.ColorPicker.ViewModels
{
    public abstract class ColorPickerProperties : AvaloniaObject
    {
        public static readonly StyledProperty<Views.ColorPicker?> ColorPickerProperty =
            AvaloniaProperty.Register<ColorPickerProperties, Views.ColorPicker?>(nameof(ColorPicker));

        public ColorPickerProperties()
        {
            this.GetObservable(ColorPickerProperty).Subscribe(_ => OnColorPickerChange());
        }

        public Views.ColorPicker? ColorPicker
        {
            get { return GetValue(ColorPickerProperty); }
            set { SetValue(ColorPickerProperty, value); }
        }

        public abstract void UpdateColorPickerValues();

        public abstract void UpdatePropertyValues();

        public virtual void OnColorPickerChange()
        {
            if (ColorPicker != null)
            {
                ColorPicker.GetObservable(Views.ColorPicker.Value1Property).Subscribe(_ => UpdatePropertyValues());
                ColorPicker.GetObservable(Views.ColorPicker.Value2Property).Subscribe(_ => UpdatePropertyValues());
                ColorPicker.GetObservable(Views.ColorPicker.Value3Property).Subscribe(_ => UpdatePropertyValues());
                ColorPicker.GetObservable(Views.ColorPicker.Value4Property).Subscribe(_ => UpdatePropertyValues());
            }
        }
    }
}
