using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using Steropes.Tiles.TemplateGenerator.Annotations;

namespace Steropes.Tiles.TemplateGenerator.Model
{
  public class FormattingMetaData: INotifyPropertyChanged
  {
    string title;

    int? border;
    int? margin;
    int? padding;

    Color? borderColor;
    Color? textColor;
    Color? backgroundColor;

    public event PropertyChangedEventHandler PropertyChanged;

    public FormattingMetaData()
    {
    }

    public string Title
    {
      get { return title; }
      set
      {
        if (value == title) return;
        title = value;
        OnPropertyChanged();
      }
    }

    public int? Border
    {
      get { return border; }
      set
      {
        if (value == border) return;
        border = value;
        OnPropertyChanged();
      }
    }

    public int? Margin
    {
      get { return margin; }
      set
      {
        if (value == margin) return;
        margin = value;
        OnPropertyChanged();
      }
    }

    public int? Padding
    {
      get { return padding; }
      set
      {
        if (value == padding) return;
        padding = value;
        OnPropertyChanged();
      }
    }

    public Color? BorderColor
    {
      get { return borderColor; }
      set
      {
        if (value.Equals(borderColor)) return;
        borderColor = value;
        OnPropertyChanged();
      }
    }

    public Color? TextColor
    {
      get { return textColor; }
      set
      {
        if (value.Equals(textColor)) return;
        textColor = value;
        OnPropertyChanged();
      }
    }

    public Color? BackgroundColor
    {
      get { return backgroundColor; }
      set
      {
        if (value.Equals(backgroundColor)) return;
        backgroundColor = value;
        OnPropertyChanged();
      }
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}