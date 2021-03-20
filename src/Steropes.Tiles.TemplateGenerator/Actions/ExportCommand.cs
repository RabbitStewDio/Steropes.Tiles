using System;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Steropes.Tiles.TemplateGenerator.Layout;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Actions
{
    public sealed class ExportCommand : ModelCommand
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
            using (var bitmap = producer.Produce(insertPoint))
            {
                var fname = QueryFile(MakeAbsolute(insertPoint.LastExportLocation));
                if (!string.IsNullOrEmpty(fname))
                {
                    insertPoint.LastExportLocation = Model.Content.MakeRelative(fname);
                    Model.Content.Properties = Model.Content.Properties
                                                    .Updated("last-exported", DateTime.UtcNow.ToString("o"));

                    bitmap.Save(fname, ImageFormat.Png);
                }
            }
        }

        string MakeAbsolute(string path)
        {
            if (path == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(Model.Content.BasePath))
            {
                return path;
            }

            var retval = Path.Combine(Model.Content.BasePath, path);
            Console.WriteLine("Combined: " + retval);
            return retval;
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
