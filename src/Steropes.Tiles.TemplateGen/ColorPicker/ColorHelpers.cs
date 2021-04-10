using Avalonia.Media;
using Steropes.Tiles.TemplateGen.ColorPicker.Colors;
using System.Text.RegularExpressions;

namespace Steropes.Tiles.TemplateGen.ColorPicker
{
    public static class ColorHelpers
    {
        static readonly Regex s_hexRegex = new Regex("^#[a-fA-F0-9]{8}$");

        public static bool IsValidHexColor(string hex)
        {
            return !string.IsNullOrWhiteSpace(hex) && s_hexRegex.Match(hex).Success;
        }

        public static string ToHexColor(Color color)
        {
            return $"#{color.ToUint32():X8}";
        }

        public static Color FromHexColor(string hex)
        {
            return Color.Parse(hex);
        }

        public static void FromColor(Color color, out double h, out double s, out double v, out double a)
        {
            HSV hsv = new RGB(color.R, color.G, color.B).ToHSV();
            h = hsv.H;
            s = hsv.S;
            v = hsv.V;
            a = color.A * 100.0 / 255.0;
        }

        public static Color FromHSVA(double h, double s, double v, double a)
        {
            RGB rgb = new HSV(h, s, v).ToRGB();
            byte A = (byte)(a * 255.0 / 100.0);
            return new Color(A, (byte)rgb.R, (byte)rgb.G, (byte)rgb.B);
        }

        public static Color FromRGBA(byte r, byte g, byte b, double a)
        {
            byte A = (byte)(a * 255.0 / 100.0);
            return new Color(A, r, g, b);
        }
    }
}
