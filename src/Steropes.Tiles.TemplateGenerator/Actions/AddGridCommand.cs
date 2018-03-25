using System;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Actions
{
  public sealed class AddGridCommand : ModelCommand
  {
    int counter;

    public AddGridCommand(MainModel model) : base(model)
    {
      Enabled = RefreshEnabled();
    }

    protected override bool RefreshEnabled()
    {
      return base.RefreshEnabled() && SelectedCollection != null;
    }

    public override void OnActionPerformed(object source, EventArgs args)
    {
      if (Model.Content == null)
      {
        return;
      }

      var insertPoint = SelectedCollection;
      if (insertPoint == null)
      {
        return;
      }

      counter += 1;
      var textureGrid = new TextureGrid()
      {
        Name = $"grid-{counter}",
      };
      textureGrid.FormattingMetaData.Border = Model.Preferences.DefaultGridBorder;
      textureGrid.FormattingMetaData.Padding = Model.Preferences.DefaultGridPadding;
      textureGrid.FormattingMetaData.Margin = Model.Preferences.DefaultGridMargin;
      insertPoint.Grids.Add(textureGrid);
      Model.Selection.Clear();
      Model.Selection.Add(textureGrid);
    }
  }
}