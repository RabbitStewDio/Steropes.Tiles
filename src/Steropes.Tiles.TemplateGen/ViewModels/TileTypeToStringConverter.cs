using Avalonia;
using Avalonia.Data.Converters;
using Steropes.Tiles.TemplateGen.Models;
using System;
using System.Globalization;
using System.Text;

namespace Steropes.Tiles.TemplateGen.ViewModels
{
    public class TileTypeToStringConverter: IValueConverter
    {
        static string ToHumanForm(string m)
        {
            StringBuilder b = new StringBuilder();
            foreach (var c in m)
            {
                if (b.Length > 0 && char.IsUpper(c))
                {
                    b.Append(' ');
                }
                b.Append(c);
            }

            return b.ToString();
        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TileType matcher)
            {
                if (targetType == typeof(string))
                {
                    return ToHumanForm(matcher.ToString());
                }
            }
            
            return AvaloniaProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return AvaloniaProperty.UnsetValue;
        }
    }
}
