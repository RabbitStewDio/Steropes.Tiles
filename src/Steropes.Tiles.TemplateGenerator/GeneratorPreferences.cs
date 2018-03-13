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

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}