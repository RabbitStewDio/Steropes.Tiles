using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using Steropes.Tiles.Properties;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator
{
  public class GeneratorPreferences: INotifyPropertyChanged
  {
    TileType defaultTileType;
    int defaultWidth;
    int defaultHeight;
    int textSpacing;
    Color defaultTileHighlightColor;
    Color defaultTileColor;
    Color defaultTextColor;
    Font defaultFont;
    public event PropertyChangedEventHandler PropertyChanged;

    public GeneratorPreferences()
    {
      TileColors = new ObservableCollection<Color>
      {
        Color.Aquamarine,
        Color.CornflowerBlue,
        Color.Coral,
        Color.Gold,
        Color.MediumOrchid,
        Color.Lavender
      };

      RecentFiles = new ObservableCollection<string>();
      DefaultTileType = TileType.Isometric;
      DefaultWidth = 96;
      DefaultHeight = 48;
      
      DefaultBorderColor = Color.DarkGray;
      DefaultTileColor = Color.DarkGray;
      DefaultTileHighlightColor = Color.DimGray;
      DefaultTextColor = Color.Black;
      TextSpacing = 5;
      DefaultFont = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular);
    }

    public Font DefaultFont
    {
      get { return defaultFont; }
      set
      {
        if (Equals(value, defaultFont)) return;
        defaultFont = value;
        OnPropertyChanged();
      }
    }

    public int TextSpacing
    {
      get { return textSpacing; }
      set
      {
        if (value == textSpacing) return;
        textSpacing = value;
        OnPropertyChanged();
      }
    }

    public Color DefaultTileHighlightColor
    {
      get { return defaultTileHighlightColor; }
      set
      {
        if (value.Equals(defaultTileHighlightColor)) return;
        defaultTileHighlightColor = value;
        OnPropertyChanged();
      }
    }

    public Color DefaultTileColor
    {
      get { return defaultTileColor; }
      set
      {
        if (value.Equals(defaultTileColor)) return;
        defaultTileColor = value;
        OnPropertyChanged();
      }
    }

    public Color DefaultTextColor
    {
      get { return defaultTextColor; }
      set
      {
        if (value.Equals(defaultTextColor)) return;
        defaultTextColor = value;
        OnPropertyChanged();
      }
    }

    public int DefaultWidth
    {
      get { return defaultWidth; }
      set
      {
        if (value == defaultWidth) return;
        defaultWidth = value;
        OnPropertyChanged();
      }
    }

    public int DefaultHeight
    {
      get { return defaultHeight; }
      set
      {
        if (value == defaultHeight) return;
        defaultHeight = value;
        OnPropertyChanged();
      }
    }

    public TileType DefaultTileType
    {
      get { return defaultTileType; }
      set
      {
        if (value == defaultTileType) return;
        defaultTileType = value;
        OnPropertyChanged();
      }
    }

    public ObservableCollection<string> RecentFiles { get; }

    public ObservableCollection<Color> TileColors { get; }
    public Color DefaultBorderColor { get; set; }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}