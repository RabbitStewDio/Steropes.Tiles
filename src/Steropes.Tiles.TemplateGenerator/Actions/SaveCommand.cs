using System;
using System.Windows.Forms;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Actions
{
    public sealed class SaveCommand : ModelCommand
    {
        readonly bool alwaysAsk;

        public SaveCommand(MainModel model, bool alwaysAsk) : base(model)
        {
            this.alwaysAsk = alwaysAsk;
            Enabled = RefreshEnabled();
        }

        string QueryFile(string oldPath)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "Tile Set Files|*.tiles;*.xml|All files (*.*)|*.*";
            dialog.Title = "Select a Tile Set File";
            dialog.CheckFileExists = false;
            dialog.OverwritePrompt = true;
            dialog.AddExtension = true;
            dialog.DereferenceLinks = true;
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

        public override void OnActionPerformed(object source, EventArgs args)
        {
            try
            {
                PerformSave();
            }
            catch (SaveException e)
            {
                MainModel.HandleError($"Unable to save file to {e.File}.\n\nError Message: {e}", "Error Saving File");
            }
            catch (Exception e)
            {
                MainModel.HandleError($"Unable to save file.\n\nError Message: {e}", "Error Saving File");
            }
        }

        public void PerformSave()
        {
            if (Model.Content == null)
            {
                return;
            }

            var file = Model.Content.SourcePath;
            if (!alwaysAsk && !string.IsNullOrEmpty(file))
            {
                try
                {
                    PerformSave(file);
                    return;
                }
                catch (Exception)
                {
                    // something went wrong. Ask the user for a different file ...
                }
            }

            file = QueryFile(file);
            if (!string.IsNullOrEmpty(file))
            {
                PerformSave(file);
            }
        }

        void PerformSave(string file)
        {
            try
            {
                Model.Content.Properties = Model.Content.Properties
                                                .Updated("last-saved", DateTime.UtcNow.ToString("o"));

                var document = TextureFileWriter.GenerateXml(Model.Content);
                document.Save(file);
                Model.Content.Modified = false;
                Model.Content.SourcePath = file;
                Model.Preferences.AddRecentFile(file);
            }
            catch (Exception e)
            {
                throw new SaveException(file, "Unable to save content", e);
            }
        }
    }
}
