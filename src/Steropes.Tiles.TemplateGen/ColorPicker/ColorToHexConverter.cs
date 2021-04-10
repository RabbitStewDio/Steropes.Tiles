using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace Steropes.Tiles.TemplateGen.ColorPicker
{
    /// <summary>
    ///   Converts a color value into a text value and back.
    /// </summary>
    public class ColorToHexConverter : IValueConverter
    {
        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
        {
            if (value is string hex)
            {
                if (targetType == typeof(Color))
                {
                    try
                    {
                        if (ColorHelpers.IsValidHexColor(hex))
                        {
                            return Color.Parse(hex);
                        }
                    }
                    catch (Exception)
                    {
                        return AvaloniaProperty.UnsetValue;
                    }
                }
            }

            return AvaloniaProperty.UnsetValue;
        }

        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {
            if (value is Color color)
            {
                if (targetType == typeof(string))
                {
                    try
                    {
                        return ColorHelpers.ToHexColor(color);
                    }
                    catch (Exception)
                    {
                        return AvaloniaProperty.UnsetValue;
                    }
                }
            }

            return AvaloniaProperty.UnsetValue;
        }
    }
}
