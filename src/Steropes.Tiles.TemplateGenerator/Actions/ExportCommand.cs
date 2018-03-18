using System;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Steropes.Tiles.TemplateGenerator.Layout;

namespace Steropes.Tiles.TemplateGenerator.Actions
{
  public sealed class ExportCommand: ModelCommand
  {
    public ExportCommand(MainModel model) : base(model)
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

      var gen = new GridGenerator();
      gen.Regenerate(insertPoint);

      var producer = new GridCollectionPainter(Model.Preferences);
      var bitmap = producer.Produce(insertPoint);

      var fname = QueryFile(null);
      if (fname != null)
      {
        bitmap.Save(fname, ImageFormat.Png);
      }

    }

    string QueryFile(string oldPath)
    {
      var dialog = new SaveFileDialog
      {
        Filter = "Textures|*.png|All files (*.*)|*.*",
        Title = "Select a Texture file",
        CheckFileExists = false,
        OverwritePrompt = true,
        AddExtension = true,
        DereferenceLinks = true
      };
      if (oldPath != null)
      {
        dialog.FileName = oldPath;
      }

      if (dialog.ShowDialog() == DialogResult.OK)
      {
        return dialog.FileName;
      }

      return null;
    }

  }
}