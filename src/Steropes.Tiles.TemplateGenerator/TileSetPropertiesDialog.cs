using System;
using System.Windows.Forms;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator
{
  public partial class TileSetPropertiesDialog : Form
  {
    public TileSetPropertiesDialog()
    {
      InitializeComponent();
      editor.ValidationStateChanged += OnValidStateChange;
    }

    void OnValidStateChange(object sender, EventArgs e)
    {
      okButton.Enabled = editor.Valid;
    }

    public TextureFile PerformEdit(TextureFile source = null)
    {
      this.editor.ApplyFrom(source);
      var result = ShowDialog();
      if (result == DialogResult.OK)
      {
        return editor.ApplyTo(source);
      }

      return null;
    }
  }
}