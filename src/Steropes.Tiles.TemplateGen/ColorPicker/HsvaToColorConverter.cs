using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Steropes.Tiles.TemplateGen.ColorPicker
{
    public class HsvaToColorConverter : IMultiValueConverter
    {
        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            double[] v = values.OfType<double>().ToArray();
            if (v.Length == values.Count)
            {
                return ColorHelpers.FromHSVA(v[0], v[1], v[2], v[3]);
            }
            return AvaloniaProperty.UnsetValue;
        }
    }
}
