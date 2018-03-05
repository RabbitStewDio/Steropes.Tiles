using System;

namespace Steropes.Tiles.TemplateGenerator.Actions
{
  public class NewCommand: Command
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
      var c = propertiesDialog.PerformEdit(model.Content);
      if (c != null)
      {
        model.Content = c;
        model.Content.Modified = true;
      }
    }
  }
}