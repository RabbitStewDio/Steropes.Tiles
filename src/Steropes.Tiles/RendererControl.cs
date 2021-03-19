using JetBrains.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Steropes.Tiles.DataStructures;

namespace Steropes.Tiles
{
  public class RendererControl: IRendererControl
  {
    Rect bounds;

    public RendererControl(IntDimension tileSize, RenderType renderType)
    {
      TileSize = tileSize;
      RenderType = renderType;
    }

    public IntDimension TileSize { get; }
    public RenderType RenderType { get; }

    public Rect Bounds
    {
      get { return bounds; }
      set
      {
        if (value.Equals(bounds)) return;
        bounds = value;
        OnPropertyChanged();
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}