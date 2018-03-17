using System;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Actions
{
  public class NewCommand : Command
  {
    readonly MainModel model;
    readonly TileSetPropertiesDialog propertiesDialog;

    public NewCommand(MainModel model, TileSetPropertiesDialog propertiesDialog)
    {
      this.model = model ?? throw new ArgumentNullException(nameof(model));
      this.propertiesDialog = propertiesDialog ?? throw new ArgumentNullException(nameof(propertiesDialog));
    }

    public override void OnActionPerformed(object source, EventArgs args)
    {
      var content = new TextureFile
      {
        Width = model.Content?.Width ?? model.Preferences.DefaultWidth,
        Height = model.Content?.Height ?? model.Preferences.DefaultHeight,
        TileType = model.Content?.TileType ?? model.Preferences.DefaultTileType,
        Name = model.Content?.Name ?? "New Tile Set"
      };

      var c = propertiesDialog.PerformEdit(content);
      if (c != null)
      {
        model.Content = c;
        model.Content.Modified = true;
        model.Selection.Clear();
        model.Selection.Add(c);
      }
    }
  }
}