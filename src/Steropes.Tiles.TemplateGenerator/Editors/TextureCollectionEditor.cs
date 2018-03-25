using System;
using System.IO;
using System.Windows.Forms;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Editors
{
  public partial class TextureCollectionEditor : UserControl, IDetailEditor<TextureCollection>
  {
    readonly FormValidator validator;
    string basePath;

    public TextureCollectionEditor()
    {

      InitializeComponent();

      validator = new FormValidator()
      {
        fileNameBox.CreateValidator()
          .ForProvider(errorProvider)
          .WithErrorCondition(Validators.TextEmpty, "Texture reference must be defined.")
      };
      validator.InputReceived += OnInputReceived;
    }

    void OnInputReceived(object s, EventArgs e)
    {
      InputReceived?.Invoke(this, EventArgs.Empty);
    }

    public bool Valid => validator.Valid;

    public void ApplyFrom(TextureCollection source)
    {
      try
      {
        validator.SuspendValidation();

        if (source == null)
        {
          this.fileNameBox.Text = "";
          this.basePath = "";
        }
        else
        {
          this.fileNameBox.Text = source.Id;
          this.basePath = source.Parent?.BasePath ?? "";
        }

      }
      finally
      {
        validator.ResumeValidation();
      }
    }

    public TextureCollection ApplyTo(TextureCollection target)
    {
      if (target == null)
      {
        target = new TextureCollection();
      }

      target.Id = fileNameBox.Text;
      return target;
    }

    public event EventHandler InputReceived;

    string QueryFile()
    {
      var dialog = new OpenFileDialog
      {
        Filter = "Texture Files|*.png;*.jpg;*.gif|All files (*.*)|*.*",
        Title = "Select a Texture File",
        Multiselect = false,
        CheckFileExists = true,
        DereferenceLinks = true,
        AddExtension = true
      };

      if (dialog.ShowDialog() == DialogResult.OK)
      {
        return dialog.FileName;
      }

      return null;
    }

    void OnSelectFile(object sender, EventArgs e)
    {
      var path = QueryFile();
      if (path == null)
      {
        return;
      }

      try
      {
        var fp = MakeRelative(path);

        fp = Path.GetFileNameWithoutExtension(fp);
        fileNameBox.Text = fp;
      }
      catch (IOException)
      {
        // ignored ..
      }
    }

    string MakeRelative(string path)
    {
      var fp = Path.GetFullPath(path);
      if (fp.StartsWith(basePath))
      {
        fp = fp.Substring(basePath.Length);
      }

      return fp;
    }
  }
}
