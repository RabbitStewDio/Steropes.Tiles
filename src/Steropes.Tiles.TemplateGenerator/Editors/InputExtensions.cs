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
        n.Value = 0;
        n.Text = "";
      }
      else
      {
        // NumericUpDown does not necessarily recognize that a changed value
        // should trigger a refresh of the display text. This forces a refresh
        // by cycling the current value through two separate states before 
        // setting the value. Ugly, but heck, its WinForms ..
        n.Value = n.Minimum;
        n.Value = n.Maximum;
        n.Value = Clamp(text.Value, n.Minimum, n.Maximum);
      }
    }

    static decimal Clamp(decimal value, decimal min, decimal max)
    {
      return Math.Max(min, Math.Min(value, max));
    }
  }
}