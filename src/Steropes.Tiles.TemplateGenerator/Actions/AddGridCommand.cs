using System;
using System.Collections.Specialized;
using System.Linq;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Actions
{
  public class AddGridCommand : ModelCommand
  {
    int counter;

    public AddGridCommand(MainModel model) : base(model)
    {
      Model.Selection.CollectionChanged += OnSelectionChanged;
    }

    void OnSelectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      Enabled = Model.Content != null && Model.Selection.OfType<TextureCollection>().Any();
    }

    public override void OnActionPerformed(object source, EventArgs args)
    {
      if (Model.Content == null)
      {
        return;
      }

      var insertPoint = Model.Selection.OfType<TextureCollection>().FirstOrDefault();
      if (insertPoint == null)
      {
        return;
      }

      counter += 1;
      var textureGrid = new TextureGrid() { Name = $"Grid {counter}"};
      insertPoint.Grids.Add(textureGrid);
      Model.Selection.Clear();
      Model.Selection.Add(textureGrid);
    }
  }
}