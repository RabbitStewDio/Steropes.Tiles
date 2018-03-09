using System;
using System.Drawing;
using System.Windows.Forms;

namespace Steropes.Tiles.TemplateGenerator.Editors
{
  public static class InputExtensions 
  {
    public static Color? AsNullable(this Color c)
    {
      if (c == Color.Empty)
      {
        return null;
      }

      return c;
    }

    public static decimal? GetValue(this NumericUpDown n)
    {
      if (string.IsNullOrEmpty(n.Text))
      {
        return null;
      }

      return n.Value;
    }

    public static void SetValue(this NumericUpDown n, decimal? text)
    {
      if (!text.HasValue)
      {
        n.Text = "";
      }
      else
      {
        n.Value = Clamp(text.Value, n.Minimum, n.Maximum);
      }
    }

    static decimal Clamp(decimal value, decimal min, decimal max)
    {
      return Math.Max(min, Math.Min(value, max));
    }
  }
}