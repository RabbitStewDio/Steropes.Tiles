using System;
using System.Collections.Specialized;
using System.Linq;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Actions
{
  public class AddTilesCommand : ModelCommand
  {
    public AddTilesCommand(MainModel model) : base(model)
    {
      Model.Selection.CollectionChanged += OnSelectionChanged;
    }

    void OnSelectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      Enabled = Model.Content != null && Model.Selection.OfType<ITextureTileParent>().Any();
    }

    public override void OnActionPerformed(object source, EventArgs args)
    {
      if (Model.Content == null)
      {
        return;
      }

      var insertPoint = Model.Selection.OfType<ITextureTileParent>().Where(IsValidInsertPoint).FirstOrDefault();
      if (insertPoint == null)
      {
        return;
      }

      var textureTile = new TextureTile();
      insertPoint.Tiles.Add(textureTile);
      Model.Selection.Clear();
      Model.Selection.Add(textureTile);
    }

    public bool IsValidInsertPoint(ITextureTileParent p)
    {
      if (p is TextureGroup g)
      {
        return g.MatcherType == MatcherType.Basic;
      }

      return true;
    }
  }
}