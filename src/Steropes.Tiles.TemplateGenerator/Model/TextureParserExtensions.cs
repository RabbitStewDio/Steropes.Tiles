using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Steropes.Tiles.TemplateGenerator.Model
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

    public static XElement ElementLocal(this XElement e, string localName)
    {
      return e.Elements().FirstOrDefault(ex => ex.Name.LocalName == localName);
    }

    public static XAttribute AttributeLocal(this XElement e, string localName)
    {
      return e.Attributes().FirstOrDefault(ex => ex.Name.LocalName == localName);
    }

    public static Color? AsColor(this XAttribute attr)
    {
      return AsColor((string) attr);
    }

    public static Color? AsColor(this XElement element)
    {
      return AsColor(element.Value);
    }

    public static Color? AsColor(this string attr)
    {
      if (attr == null)
      {
        return null;
      }

      var name = attr;
      if (knownColors.TryGetValue(name, out var c))
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

    public static TileType ParseTextureType(IXmlLineInfo lineInfo, string t, TileType? defaultValue = null)
    {
      return t.ParseEnumStrict(lineInfo, defaultValue) ?? throw new XmlParseException("Attribute value missing", lineInfo);;
    }

    public static T? ParseEnumStrict<T>(this string t, IXmlLineInfo lineInfo, T? defaultValue = null) where T : struct
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

    public static T? ParseEnumLenient<T>(this string t, T? defaultValue = null) where T : struct
    {
      if (string.IsNullOrEmpty(t))
      {
        return defaultValue;
      }

      return Enum.TryParse(t, out T result) ? result : defaultValue;
    }

    public static string AsText(this Color? cn)
    {
      if (cn == null || cn == Color.Empty)
      {
        return null;
      }
      var c = cn.Value;
      return AsText(c);
    }

    public static string AsText(this Color c)
    {
      bool ColorEquals(KeyValuePair<string,Color> kv)
      {
        var c2 = kv.Value;
        return c2.A == c.A && c2.R == c.R && c2.G == c.G && c2.B == c.B;
      }

      var l = knownColors.Where(ColorEquals);
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

    static readonly Dictionary<string, Color> knownColors;

    static TextureParserExtensions()
    {
      // Harvested from MonoGame's Color class ..
      knownColors = new Dictionary<string, Color>();
      knownColors["aliceblue"] = Color.FromArgb(255, 240, 248, 255);
      knownColors["antiquewhite"] = Color.FromArgb(255, 250, 235, 215);
      knownColors["aqua"] = Color.FromArgb(255, 0, 255, 255);
      knownColors["aquamarine"] = Color.FromArgb(255, 127, 255, 212);
      knownColors["azure"] = Color.FromArgb(255, 240, 255, 255);
      knownColors["beige"] = Color.FromArgb(255, 245, 245, 220);
      knownColors["bisque"] = Color.FromArgb(255, 255, 228, 196);
      knownColors["black"] = Color.FromArgb(255, 0, 0, 0);
      knownColors["blanchedalmond"] = Color.FromArgb(255, 255, 235, 205);
      knownColors["blue"] = Color.FromArgb(255, 0, 0, 255);
      knownColors["blueviolet"] = Color.FromArgb(255, 138, 43, 226);
      knownColors["brown"] = Color.FromArgb(255, 165, 42, 42);
      knownColors["burlywood"] = Color.FromArgb(255, 222, 184, 135);
      knownColors["cadetblue"] = Color.FromArgb(255, 95, 158, 160);
      knownColors["chartreuse"] = Color.FromArgb(255, 127, 255, 0);
      knownColors["chocolate"] = Color.FromArgb(255, 210, 105, 30);
      knownColors["coral"] = Color.FromArgb(255, 255, 127, 80);
      knownColors["cornflowerblue"] = Color.FromArgb(255, 100, 149, 237);
      knownColors["cornsilk"] = Color.FromArgb(255, 255, 248, 220);
      knownColors["crimson"] = Color.FromArgb(255, 220, 20, 60);
      knownColors["cyan"] = Color.FromArgb(255, 0, 255, 255);
      knownColors["darkblue"] = Color.FromArgb(255, 0, 0, 139);
      knownColors["darkcyan"] = Color.FromArgb(255, 0, 139, 139);
      knownColors["darkgoldenrod"] = Color.FromArgb(255, 184, 134, 11);
      knownColors["darkgray"] = Color.FromArgb(255, 169, 169, 169);
      knownColors["darkgreen"] = Color.FromArgb(255, 0, 100, 0);
      knownColors["darkkhaki"] = Color.FromArgb(255, 189, 183, 107);
      knownColors["darkmagenta"] = Color.FromArgb(255, 139, 0, 139);
      knownColors["darkolivegreen"] = Color.FromArgb(255, 85, 107, 47);
      knownColors["darkorange"] = Color.FromArgb(255, 255, 140, 0);
      knownColors["darkorchid"] = Color.FromArgb(255, 153, 50, 204);
      knownColors["darkred"] = Color.FromArgb(255, 139, 0, 0);
      knownColors["darksalmon"] = Color.FromArgb(255, 233, 150, 122);
      knownColors["darkseagreen"] = Color.FromArgb(255, 143, 188, 139);
      knownColors["darkslateblue"] = Color.FromArgb(255, 72, 61, 139);
      knownColors["darkslategray"] = Color.FromArgb(255, 47, 79, 79);
      knownColors["darkturquoise"] = Color.FromArgb(255, 0, 206, 209);
      knownColors["darkviolet"] = Color.FromArgb(255, 148, 0, 211);
      knownColors["deeppink"] = Color.FromArgb(255, 255, 20, 147);
      knownColors["deepskyblue"] = Color.FromArgb(255, 0, 191, 255);
      knownColors["dimgray"] = Color.FromArgb(255, 105, 105, 105);
      knownColors["dodgerblue"] = Color.FromArgb(255, 30, 144, 255);
      knownColors["firebrick"] = Color.FromArgb(255, 178, 34, 34);
      knownColors["floralwhite"] = Color.FromArgb(255, 255, 250, 240);
      knownColors["forestgreen"] = Color.FromArgb(255, 34, 139, 34);
      knownColors["fuchsia"] = Color.FromArgb(255, 255, 0, 255);
      knownColors["gainsboro"] = Color.FromArgb(255, 220, 220, 220);
      knownColors["ghostwhite"] = Color.FromArgb(255, 248, 248, 255);
      knownColors["gold"] = Color.FromArgb(255, 255, 215, 0);
      knownColors["goldenrod"] = Color.FromArgb(255, 218, 165, 32);
      knownColors["gray"] = Color.FromArgb(255, 128, 128, 128);
      knownColors["green"] = Color.FromArgb(255, 0, 128, 0);
      knownColors["greenyellow"] = Color.FromArgb(255, 173, 255, 47);
      knownColors["honeydew"] = Color.FromArgb(255, 240, 255, 240);
      knownColors["hotpink"] = Color.FromArgb(255, 255, 105, 180);
      knownColors["indianred"] = Color.FromArgb(255, 205, 92, 92);
      knownColors["indigo"] = Color.FromArgb(255, 75, 0, 130);
      knownColors["ivory"] = Color.FromArgb(255, 255, 255, 240);
      knownColors["khaki"] = Color.FromArgb(255, 240, 230, 140);
      knownColors["lavender"] = Color.FromArgb(255, 230, 230, 250);
      knownColors["lavenderblush"] = Color.FromArgb(255, 255, 240, 245);
      knownColors["lawngreen"] = Color.FromArgb(255, 124, 252, 0);
      knownColors["lemonchiffon"] = Color.FromArgb(255, 255, 250, 205);
      knownColors["lightblue"] = Color.FromArgb(255, 173, 216, 230);
      knownColors["lightcoral"] = Color.FromArgb(255, 240, 128, 128);
      knownColors["lightcyan"] = Color.FromArgb(255, 224, 255, 255);
      knownColors["lightgoldenrodyellow"] = Color.FromArgb(255, 250, 250, 210);
      knownColors["lightgray"] = Color.FromArgb(255, 211, 211, 211);
      knownColors["lightgreen"] = Color.FromArgb(255, 144, 238, 144);
      knownColors["lightpink"] = Color.FromArgb(255, 255, 182, 193);
      knownColors["lightsalmon"] = Color.FromArgb(255, 255, 160, 122);
      knownColors["lightseagreen"] = Color.FromArgb(255, 32, 178, 170);
      knownColors["lightskyblue"] = Color.FromArgb(255, 135, 206, 250);
      knownColors["lightslategray"] = Color.FromArgb(255, 119, 136, 153);
      knownColors["lightsteelblue"] = Color.FromArgb(255, 176, 196, 222);
      knownColors["lightyellow"] = Color.FromArgb(255, 255, 255, 224);
      knownColors["lime"] = Color.FromArgb(255, 0, 255, 0);
      knownColors["limegreen"] = Color.FromArgb(255, 50, 205, 50);
      knownColors["linen"] = Color.FromArgb(255, 250, 240, 230);
      knownColors["magenta"] = Color.FromArgb(255, 255, 0, 255);
      knownColors["maroon"] = Color.FromArgb(255, 128, 0, 0);
      knownColors["mediumaquamarine"] = Color.FromArgb(255, 102, 205, 170);
      knownColors["mediumblue"] = Color.FromArgb(255, 0, 0, 205);
      knownColors["mediumorchid"] = Color.FromArgb(255, 186, 85, 211);
      knownColors["mediumpurple"] = Color.FromArgb(255, 147, 112, 219);
      knownColors["mediumseagreen"] = Color.FromArgb(255, 60, 179, 113);
      knownColors["mediumslateblue"] = Color.FromArgb(255, 123, 104, 238);
      knownColors["mediumspringgreen"] = Color.FromArgb(255, 0, 250, 154);
      knownColors["mediumturquoise"] = Color.FromArgb(255, 72, 209, 204);
      knownColors["mediumvioletred"] = Color.FromArgb(255, 199, 21, 133);
      knownColors["midnightblue"] = Color.FromArgb(255, 25, 25, 112);
      knownColors["mintcream"] = Color.FromArgb(255, 245, 255, 250);
      knownColors["mistyrose"] = Color.FromArgb(255, 255, 228, 225);
      knownColors["moccasin"] = Color.FromArgb(255, 255, 228, 181);
      knownColors["monogameorange"] = Color.FromArgb(255, 231, 60, 0);
      knownColors["navajowhite"] = Color.FromArgb(255, 255, 222, 173);
      knownColors["navy"] = Color.FromArgb(255, 0, 0, 128);
      knownColors["oldlace"] = Color.FromArgb(255, 253, 245, 230);
      knownColors["olive"] = Color.FromArgb(255, 128, 128, 0);
      knownColors["olivedrab"] = Color.FromArgb(255, 107, 142, 35);
      knownColors["orange"] = Color.FromArgb(255, 255, 165, 0);
      knownColors["orangered"] = Color.FromArgb(255, 255, 69, 0);
      knownColors["orchid"] = Color.FromArgb(255, 218, 112, 214);
      knownColors["palegoldenrod"] = Color.FromArgb(255, 238, 232, 170);
      knownColors["palegreen"] = Color.FromArgb(255, 152, 251, 152);
      knownColors["paleturquoise"] = Color.FromArgb(255, 175, 238, 238);
      knownColors["palevioletred"] = Color.FromArgb(255, 219, 112, 147);
      knownColors["papayawhip"] = Color.FromArgb(255, 255, 239, 213);
      knownColors["peachpuff"] = Color.FromArgb(255, 255, 218, 185);
      knownColors["peru"] = Color.FromArgb(255, 205, 133, 63);
      knownColors["pink"] = Color.FromArgb(255, 255, 192, 203);
      knownColors["plum"] = Color.FromArgb(255, 221, 160, 221);
      knownColors["powderblue"] = Color.FromArgb(255, 176, 224, 230);
      knownColors["purple"] = Color.FromArgb(255, 128, 0, 128);
      knownColors["red"] = Color.FromArgb(255, 255, 0, 0);
      knownColors["rosybrown"] = Color.FromArgb(255, 188, 143, 143);
      knownColors["royalblue"] = Color.FromArgb(255, 65, 105, 225);
      knownColors["saddlebrown"] = Color.FromArgb(255, 139, 69, 19);
      knownColors["salmon"] = Color.FromArgb(255, 250, 128, 114);
      knownColors["sandybrown"] = Color.FromArgb(255, 244, 164, 96);
      knownColors["seagreen"] = Color.FromArgb(255, 46, 139, 87);
      knownColors["seashell"] = Color.FromArgb(255, 255, 245, 238);
      knownColors["sienna"] = Color.FromArgb(255, 160, 82, 45);
      knownColors["silver"] = Color.FromArgb(255, 192, 192, 192);
      knownColors["skyblue"] = Color.FromArgb(255, 135, 206, 235);
      knownColors["slateblue"] = Color.FromArgb(255, 106, 90, 205);
      knownColors["slategray"] = Color.FromArgb(255, 112, 128, 144);
      knownColors["snow"] = Color.FromArgb(255, 255, 250, 250);
      knownColors["springgreen"] = Color.FromArgb(255, 0, 255, 127);
      knownColors["steelblue"] = Color.FromArgb(255, 70, 130, 180);
      knownColors["tan"] = Color.FromArgb(255, 210, 180, 140);
      knownColors["teal"] = Color.FromArgb(255, 0, 128, 128);
      knownColors["thistle"] = Color.FromArgb(255, 216, 191, 216);
      knownColors["tomato"] = Color.FromArgb(255, 255, 99, 71);
      knownColors["transparent"] = Color.FromArgb(255, 0, 0, 0);
      knownColors["transparentblack"] = Color.FromArgb(255, 0, 0, 0);
      knownColors["turquoise"] = Color.FromArgb(255, 64, 224, 208);
      knownColors["violet"] = Color.FromArgb(255, 238, 130, 238);
      knownColors["wheat"] = Color.FromArgb(255, 245, 222, 179);
      knownColors["white"] = Color.FromArgb(255, 255, 255, 255);
      knownColors["whitesmoke"] = Color.FromArgb(255, 245, 245, 245);
      knownColors["yellow"] = Color.FromArgb(255, 255, 255, 0);
      knownColors["yellowgreen"] = Color.FromArgb(255, 154, 205, 50);
    }

    public static Color ParseFromString(string colorAsText)
    {
      var parsed = int.Parse(colorAsText.Substring(1), NumberStyles.HexNumber);
      var red = (0xFF0000 & parsed) >> 16;
      var green = (0xFF00 & parsed) >> 8;
      var blue = 0xFF & parsed;
      var alpha = (int) (colorAsText.Length == 9 ? (0xFF000000 & parsed) >> 24 : 255);
      return Color.FromArgb(alpha, red, green, blue);
    }
  }
}