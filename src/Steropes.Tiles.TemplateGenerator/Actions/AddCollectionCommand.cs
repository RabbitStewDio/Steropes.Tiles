using System;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Actions
{
  public sealed class AddCollectionCommand : ModelCommand
  {
    int counter;

    public AddCollectionCommand(MainModel model) : base(model)
    {
      Enabled = RefreshEnabled();
    }

    public override void OnActionPerformed(object source, EventArgs args)
    {
      if (Model.Content == null)
      {
        return;
      }

      counter += 1;
      var c = new TextureCollection
      {
        Id = $"path\\imageFile{counter}"
      };

      Model.Content.Collections.Add(c);
      Model.Selection.Clear();
      Model.Selection.Add(c);
    }
  }
}