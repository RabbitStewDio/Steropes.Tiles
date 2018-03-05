using System;
using System.Collections.Specialized;
using System.Linq;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Actions
{
  public class DeleteCommnad : ModelCommand
  {
    public DeleteCommnad(MainModel model) : base(model)
    {
      Model.Selection.CollectionChanged += OnSelectionChanged;
    }

    void OnSelectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      Enabled = Model.Selection.Count > 0;
    }

    public override void OnActionPerformed(object source, EventArgs args)
    {
      var selection = Model.Selection.ToList();

      foreach (var tile in selection.OfType<TextureTile>())
      {
        tile.Parent.Tiles.Remove(tile);
      }
      foreach (var tile in selection.OfType<TextureGroup>())
      {
        tile.Parent.Groups.Remove(tile);
      }
      foreach (var tile in selection.OfType<TextureGrid>())
      {
        tile.Parent.Grids.Remove(tile);
      }
      foreach (var tile in selection.OfType<TextureCollection>())
      {
        tile.Parent.Collections.Remove(tile);
      }
    }
  }
}