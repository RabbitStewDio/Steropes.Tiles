using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Steropes.Tiles.TemplateGenerator.Annotations;

namespace Steropes.Tiles.TemplateGenerator.Model
{
  public class TextureTile : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;
    public ITextureTileParent Parent { get; set; }
    public ObservableCollection<string> Tags { get; }
    int x;
    int y;
    int? anchorX;
    int? anchorY;

    public TextureTile()
    {
      Tags = new ObservableCollection<string>();
    }

    public int? AnchorX
    {
      get { return anchorX; }
      set
      {
        if (value == anchorX) return;
        anchorX = value;
        OnPropertyChanged();
      }
    }

    public int? AnchorY
    {
      get { return anchorY; }
      set
      {
        if (value == anchorY) return;
        anchorY = value;
        OnPropertyChanged();
      }
    }

    public int X
    {
      get { return x; }
      set
      {
        if (value == x) return;
        x = value;
        OnPropertyChanged();
      }
    }

    public int Y
    {
      get { return y; }
      set
      {
        if (value == y) return;
        y = value;
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