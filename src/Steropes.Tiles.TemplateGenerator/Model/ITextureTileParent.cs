using System.Collections.ObjectModel;

namespace Steropes.Tiles.TemplateGenerator.Model
{
  public interface ITextureTileParent
  {
    ObservableCollection<TextureTile> Tiles { get; }
  }
}