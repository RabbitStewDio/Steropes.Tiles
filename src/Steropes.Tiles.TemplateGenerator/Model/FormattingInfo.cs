using System.Drawing;

namespace Steropes.Tiles.TemplateGenerator.Model
{
  public class FormattingInfo
  {
    int border;
    int margin;
    int padding;

    Color borderColor;
    Color textColor;
    Color backgroundColor;

    public FormattingInfo()
    {
    }

    public int Border
    {
      get { return border; }
    }

    public int Margin
    {
      get { return margin; }
    }

    public int Padding
    {
      get { return padding; }
    }

    public Color BorderColor
    {
      get { return borderColor; }
    }

    public Color TextColor
    {
      get { return textColor; }
    }

    public Color BackgroundColor
    {
      get { return backgroundColor; }
    }

    public void Apply(FormattingMetaData md)
    {
      backgroundColor = md.BackgroundColor ?? backgroundColor;
      textColor = md.TextColor ?? textColor;
      borderColor = md.BorderColor ?? borderColor;

      border = md.Border ?? border;
      margin = md.Margin ?? margin;
      padding = md.Padding ?? padding;
    }
  }
}