using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Steropes.Tiles.TemplateGen.Models
{
    public static class TextureParserExtensions
    {
        public static ObservableCollection<T> AddRange<T>(this ObservableCollection<T> list, IEnumerable<T> range)
        {
            foreach (var r in range)
            {
                list.Add(r);
            }

            return list;
        }

        public static XElement? ElementLocal(this XElement e, string localName)
        {
            return e.Elements().FirstOrDefault(ex => ex.Name.LocalName == localName);
        }

        public static XAttribute? AttributeLocal(this XElement e, string localName)
        {
            return e.Attributes().FirstOrDefault(ex => ex.Name.LocalName == localName);
        }

        public static Color? AsColor(this XAttribute? attr)
        {
            return AsColor((string?)attr);
        }

        public static Color? AsColor(this XElement? element)
        {
            return AsColor(element?.Value);
        }

        public static Color? AsColor(this string? attr)
        {
            if (string.IsNullOrEmpty(attr))
            {
                return null;
            }

            var name = attr;
            if (KnownColors.TryGetValue(name, out var c))
            {
                return c;
            }

            try
            {
                return ParseFromString(name);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static TileType ParseTextureType(IXmlLineInfo lineInfo, string? t, TileType? defaultValue = null)
        {
            return t.ParseEnumStrict(lineInfo, defaultValue) ?? throw new XmlParseException("Attribute value missing", lineInfo);
        }

        public static T? ParseEnumStrict<T>(this string? t, IXmlLineInfo lineInfo, T? defaultValue = null)
            where T : struct
        {
            if (string.IsNullOrEmpty(t))
            {
                return defaultValue;
            }

            if (!Enum.TryParse(t, out T result))
            {
                throw new XmlParseException("Attribute value is invalid.", lineInfo);
            }

            return result;
        }

        public static T? ParseEnumLenient<T>(this string t, T? defaultValue = null)
            where T : struct
        {
            if (string.IsNullOrEmpty(t))
            {
                return defaultValue;
            }

            return Enum.TryParse(t, out T result) ? result : defaultValue;
        }

        public static string? AsText(this Color? cn)
        {
            if (cn == null || cn == default(Color))
            {
                return null;
            }

            var c = cn.Value;
            return AsText(c);
        }

        public static string AsText(this Color c)
        {
            bool ColorEquals(KeyValuePair<string, Color> kv)
            {
                var c2 = kv.Value;
                return c2.A == c.A && c2.R == c.R && c2.G == c.G && c2.B == c.B;
            }

            var l = KnownColors.Where(ColorEquals);
            using (var en = l.GetEnumerator())
            {
                if (en.MoveNext())
                {
                    return en.Current.Key;
                }
            }

            StringBuilder result = new StringBuilder();
            result.Append("#");
            if (c.A != 255)
            {
                result.Append($"{c.A:x2}");
            }

            result.Append($"{c.R:x2}");
            result.Append($"{c.G:x2}");
            result.Append($"{c.B:x2}");
            return result.ToString();
        }

        static readonly Dictionary<string, Color> KnownColors;
        public static IReadOnlyList<Color> Colors { get; }

        static TextureParserExtensions()
        {
            // Harvested from MonoGame's Color class ..
            KnownColors = new Dictionary<string, Color>();
            KnownColors["aliceblue"] = Color.FromArgb(255, 240, 248, 255);
            KnownColors["antiquewhite"] = Color.FromArgb(255, 250, 235, 215);
            KnownColors["aqua"] = Color.FromArgb(255, 0, 255, 255);
            KnownColors["aquamarine"] = Color.FromArgb(255, 127, 255, 212);
            KnownColors["azure"] = Color.FromArgb(255, 240, 255, 255);
            KnownColors["beige"] = Color.FromArgb(255, 245, 245, 220);
            KnownColors["bisque"] = Color.FromArgb(255, 255, 228, 196);
            KnownColors["black"] = Color.FromArgb(255, 0, 0, 0);
            KnownColors["blanchedalmond"] = Color.FromArgb(255, 255, 235, 205);
            KnownColors["blue"] = Color.FromArgb(255, 0, 0, 255);
            KnownColors["blueviolet"] = Color.FromArgb(255, 138, 43, 226);
            KnownColors["brown"] = Color.FromArgb(255, 165, 42, 42);
            KnownColors["burlywood"] = Color.FromArgb(255, 222, 184, 135);
            KnownColors["cadetblue"] = Color.FromArgb(255, 95, 158, 160);
            KnownColors["chartreuse"] = Color.FromArgb(255, 127, 255, 0);
            KnownColors["chocolate"] = Color.FromArgb(255, 210, 105, 30);
            KnownColors["coral"] = Color.FromArgb(255, 255, 127, 80);
            KnownColors["cornflowerblue"] = Color.FromArgb(255, 100, 149, 237);
            KnownColors["cornsilk"] = Color.FromArgb(255, 255, 248, 220);
            KnownColors["crimson"] = Color.FromArgb(255, 220, 20, 60);
            KnownColors["cyan"] = Color.FromArgb(255, 0, 255, 255);
            KnownColors["darkblue"] = Color.FromArgb(255, 0, 0, 139);
            KnownColors["darkcyan"] = Color.FromArgb(255, 0, 139, 139);
            KnownColors["darkgoldenrod"] = Color.FromArgb(255, 184, 134, 11);
            KnownColors["darkgray"] = Color.FromArgb(255, 169, 169, 169);
            KnownColors["darkgreen"] = Color.FromArgb(255, 0, 100, 0);
            KnownColors["darkkhaki"] = Color.FromArgb(255, 189, 183, 107);
            KnownColors["darkmagenta"] = Color.FromArgb(255, 139, 0, 139);
            KnownColors["darkolivegreen"] = Color.FromArgb(255, 85, 107, 47);
            KnownColors["darkorange"] = Color.FromArgb(255, 255, 140, 0);
            KnownColors["darkorchid"] = Color.FromArgb(255, 153, 50, 204);
            KnownColors["darkred"] = Color.FromArgb(255, 139, 0, 0);
            KnownColors["darksalmon"] = Color.FromArgb(255, 233, 150, 122);
            KnownColors["darkseagreen"] = Color.FromArgb(255, 143, 188, 139);
            KnownColors["darkslateblue"] = Color.FromArgb(255, 72, 61, 139);
            KnownColors["darkslategray"] = Color.FromArgb(255, 47, 79, 79);
            KnownColors["darkturquoise"] = Color.FromArgb(255, 0, 206, 209);
            KnownColors["darkviolet"] = Color.FromArgb(255, 148, 0, 211);
            KnownColors["deeppink"] = Color.FromArgb(255, 255, 20, 147);
            KnownColors["deepskyblue"] = Color.FromArgb(255, 0, 191, 255);
            KnownColors["dimgray"] = Color.FromArgb(255, 105, 105, 105);
            KnownColors["dodgerblue"] = Color.FromArgb(255, 30, 144, 255);
            KnownColors["firebrick"] = Color.FromArgb(255, 178, 34, 34);
            KnownColors["floralwhite"] = Color.FromArgb(255, 255, 250, 240);
            KnownColors["forestgreen"] = Color.FromArgb(255, 34, 139, 34);
            KnownColors["fuchsia"] = Color.FromArgb(255, 255, 0, 255);
            KnownColors["gainsboro"] = Color.FromArgb(255, 220, 220, 220);
            KnownColors["ghostwhite"] = Color.FromArgb(255, 248, 248, 255);
            KnownColors["gold"] = Color.FromArgb(255, 255, 215, 0);
            KnownColors["goldenrod"] = Color.FromArgb(255, 218, 165, 32);
            KnownColors["gray"] = Color.FromArgb(255, 128, 128, 128);
            KnownColors["green"] = Color.FromArgb(255, 0, 128, 0);
            KnownColors["greenyellow"] = Color.FromArgb(255, 173, 255, 47);
            KnownColors["honeydew"] = Color.FromArgb(255, 240, 255, 240);
            KnownColors["hotpink"] = Color.FromArgb(255, 255, 105, 180);
            KnownColors["indianred"] = Color.FromArgb(255, 205, 92, 92);
            KnownColors["indigo"] = Color.FromArgb(255, 75, 0, 130);
            KnownColors["ivory"] = Color.FromArgb(255, 255, 255, 240);
            KnownColors["khaki"] = Color.FromArgb(255, 240, 230, 140);
            KnownColors["lavender"] = Color.FromArgb(255, 230, 230, 250);
            KnownColors["lavenderblush"] = Color.FromArgb(255, 255, 240, 245);
            KnownColors["lawngreen"] = Color.FromArgb(255, 124, 252, 0);
            KnownColors["lemonchiffon"] = Color.FromArgb(255, 255, 250, 205);
            KnownColors["lightblue"] = Color.FromArgb(255, 173, 216, 230);
            KnownColors["lightcoral"] = Color.FromArgb(255, 240, 128, 128);
            KnownColors["lightcyan"] = Color.FromArgb(255, 224, 255, 255);
            KnownColors["lightgoldenrodyellow"] = Color.FromArgb(255, 250, 250, 210);
            KnownColors["lightgray"] = Color.FromArgb(255, 211, 211, 211);
            KnownColors["lightgreen"] = Color.FromArgb(255, 144, 238, 144);
            KnownColors["lightpink"] = Color.FromArgb(255, 255, 182, 193);
            KnownColors["lightsalmon"] = Color.FromArgb(255, 255, 160, 122);
            KnownColors["lightseagreen"] = Color.FromArgb(255, 32, 178, 170);
            KnownColors["lightskyblue"] = Color.FromArgb(255, 135, 206, 250);
            KnownColors["lightslategray"] = Color.FromArgb(255, 119, 136, 153);
            KnownColors["lightsteelblue"] = Color.FromArgb(255, 176, 196, 222);
            KnownColors["lightyellow"] = Color.FromArgb(255, 255, 255, 224);
            KnownColors["lime"] = Color.FromArgb(255, 0, 255, 0);
            KnownColors["limegreen"] = Color.FromArgb(255, 50, 205, 50);
            KnownColors["linen"] = Color.FromArgb(255, 250, 240, 230);
            KnownColors["magenta"] = Color.FromArgb(255, 255, 0, 255);
            KnownColors["maroon"] = Color.FromArgb(255, 128, 0, 0);
            KnownColors["mediumaquamarine"] = Color.FromArgb(255, 102, 205, 170);
            KnownColors["mediumblue"] = Color.FromArgb(255, 0, 0, 205);
            KnownColors["mediumorchid"] = Color.FromArgb(255, 186, 85, 211);
            KnownColors["mediumpurple"] = Color.FromArgb(255, 147, 112, 219);
            KnownColors["mediumseagreen"] = Color.FromArgb(255, 60, 179, 113);
            KnownColors["mediumslateblue"] = Color.FromArgb(255, 123, 104, 238);
            KnownColors["mediumspringgreen"] = Color.FromArgb(255, 0, 250, 154);
            KnownColors["mediumturquoise"] = Color.FromArgb(255, 72, 209, 204);
            KnownColors["mediumvioletred"] = Color.FromArgb(255, 199, 21, 133);
            KnownColors["midnightblue"] = Color.FromArgb(255, 25, 25, 112);
            KnownColors["mintcream"] = Color.FromArgb(255, 245, 255, 250);
            KnownColors["mistyrose"] = Color.FromArgb(255, 255, 228, 225);
            KnownColors["moccasin"] = Color.FromArgb(255, 255, 228, 181);
            KnownColors["monogameorange"] = Color.FromArgb(255, 231, 60, 0);
            KnownColors["navajowhite"] = Color.FromArgb(255, 255, 222, 173);
            KnownColors["navy"] = Color.FromArgb(255, 0, 0, 128);
            KnownColors["oldlace"] = Color.FromArgb(255, 253, 245, 230);
            KnownColors["olive"] = Color.FromArgb(255, 128, 128, 0);
            KnownColors["olivedrab"] = Color.FromArgb(255, 107, 142, 35);
            KnownColors["orange"] = Color.FromArgb(255, 255, 165, 0);
            KnownColors["orangered"] = Color.FromArgb(255, 255, 69, 0);
            KnownColors["orchid"] = Color.FromArgb(255, 218, 112, 214);
            KnownColors["palegoldenrod"] = Color.FromArgb(255, 238, 232, 170);
            KnownColors["palegreen"] = Color.FromArgb(255, 152, 251, 152);
            KnownColors["paleturquoise"] = Color.FromArgb(255, 175, 238, 238);
            KnownColors["palevioletred"] = Color.FromArgb(255, 219, 112, 147);
            KnownColors["papayawhip"] = Color.FromArgb(255, 255, 239, 213);
            KnownColors["peachpuff"] = Color.FromArgb(255, 255, 218, 185);
            KnownColors["peru"] = Color.FromArgb(255, 205, 133, 63);
            KnownColors["pink"] = Color.FromArgb(255, 255, 192, 203);
            KnownColors["plum"] = Color.FromArgb(255, 221, 160, 221);
            KnownColors["powderblue"] = Color.FromArgb(255, 176, 224, 230);
            KnownColors["purple"] = Color.FromArgb(255, 128, 0, 128);
            KnownColors["red"] = Color.FromArgb(255, 255, 0, 0);
            KnownColors["rosybrown"] = Color.FromArgb(255, 188, 143, 143);
            KnownColors["royalblue"] = Color.FromArgb(255, 65, 105, 225);
            KnownColors["saddlebrown"] = Color.FromArgb(255, 139, 69, 19);
            KnownColors["salmon"] = Color.FromArgb(255, 250, 128, 114);
            KnownColors["sandybrown"] = Color.FromArgb(255, 244, 164, 96);
            KnownColors["seagreen"] = Color.FromArgb(255, 46, 139, 87);
            KnownColors["seashell"] = Color.FromArgb(255, 255, 245, 238);
            KnownColors["sienna"] = Color.FromArgb(255, 160, 82, 45);
            KnownColors["silver"] = Color.FromArgb(255, 192, 192, 192);
            KnownColors["skyblue"] = Color.FromArgb(255, 135, 206, 235);
            KnownColors["slateblue"] = Color.FromArgb(255, 106, 90, 205);
            KnownColors["slategray"] = Color.FromArgb(255, 112, 128, 144);
            KnownColors["snow"] = Color.FromArgb(255, 255, 250, 250);
            KnownColors["springgreen"] = Color.FromArgb(255, 0, 255, 127);
            KnownColors["steelblue"] = Color.FromArgb(255, 70, 130, 180);
            KnownColors["tan"] = Color.FromArgb(255, 210, 180, 140);
            KnownColors["teal"] = Color.FromArgb(255, 0, 128, 128);
            KnownColors["thistle"] = Color.FromArgb(255, 216, 191, 216);
            KnownColors["tomato"] = Color.FromArgb(255, 255, 99, 71);
            KnownColors["transparent"] = Color.FromArgb(0, 0, 0, 0);
            KnownColors["transparentblack"] = Color.FromArgb(0, 0, 0, 0);
            KnownColors["turquoise"] = Color.FromArgb(255, 64, 224, 208);
            KnownColors["violet"] = Color.FromArgb(255, 238, 130, 238);
            KnownColors["wheat"] = Color.FromArgb(255, 245, 222, 179);
            KnownColors["white"] = Color.FromArgb(255, 255, 255, 255);
            KnownColors["whitesmoke"] = Color.FromArgb(255, 245, 245, 245);
            KnownColors["yellow"] = Color.FromArgb(255, 255, 255, 0);
            KnownColors["yellowgreen"] = Color.FromArgb(255, 154, 205, 50);

            Colors = KnownColors.Values.Where(c => c.A == 255).ToArray();
        }

        public static Color ParseFromString(string colorAsText)
        {
            var parsed = int.Parse(colorAsText.Substring(1), NumberStyles.HexNumber);
            var red = (0xFF0000 & parsed) >> 16;
            var green = (0xFF00 & parsed) >> 8;
            var blue = 0xFF & parsed;
            var alpha = (int)(colorAsText.Length == 9 ? (0xFF000000 & parsed) >> 24 : 255);
            return Color.FromArgb((byte) alpha, (byte) red, (byte) green, (byte) blue);
        }
    }
}
