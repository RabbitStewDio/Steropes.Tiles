using System;
using System.Windows.Forms;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Actions
{
  public class OpenRecentFileCommand : OpenCommand
  {
    readonly string name;

    public OpenRecentFileCommand(MainModel model, string name) : base(model)
    {
      this.name = name;
    }

    protected override string QueryFile()
    {
      return name;
    }
  }

  public class OpenCommand : Command
  {
    readonly MainModel model;

    public OpenCommand(MainModel model)
    {
      this.model = model ?? throw new ArgumentNullException(nameof(model));
    }

    protected virtual string QueryFile()
    {
      var dialog = new OpenFileDialog();
      dialog.Filter = "Tile Set Files|*.tiles;*.xml|All files (*.*)|*.*";
      dialog.Title = "Select a Tile Set File";
      dialog.Multiselect = false;
      dialog.CheckFileExists = true;
      dialog.DereferenceLinks = true;
      dialog.AddExtension = true;

      if (dialog.ShowDialog() == DialogResult.OK)
      {
        return dialog.FileName;
      }

      return null;
    }

    public override void OnActionPerformed(object source, EventArgs args)
    {
      var file = QueryFile();
      if (file == null)
      {
        return;
      }

      try
      {
        var doc = TextureFileLoader.Read(file);
        model.Content = doc;
        model.Preferences.AddRecentFile(file);
        
      }
      catch (Exception e)
      {
        MainModel.HandleError($"An error occurred while loading file '{file}.\n \nMessage: {e}", "Error Loading Document");
      }
    }
  }
}