using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator
{
  public partial class TileSetPropertiesDialog : Form
  {
    public TileSetPropertiesDialog()
    {
      InitializeComponent();

      var tileTypes = (TileType[]) Enum.GetValues(typeof(TileType));
      foreach (var type in tileTypes)
      {
        typeTypeBox.Items.Add(type);
      }
    }

    public TextureFile PerformEdit(TextureFile source = null)
    {
      if (source == null)
      {
        this.heightBox.Value = 0;
        this.widthBox.Value = 0;
        this.nameTextBox.Text = "New Tile Set";
        this.typeTypeBox.SelectedItem = TileType.Grid;
      }
      else
      {
        this.heightBox.Value = source.Height;
        this.widthBox.Value = source.Width;
        this.nameTextBox.Text = source.Name;
        this.typeTypeBox.SelectedItem = source.TileType;
      }

      var result = ShowDialog();
      if (result == DialogResult.OK)
      {
        if (source == null)
        {
          source = new TextureFile();
        }

        source.Name = nameTextBox.Text;
        source.Height = (int) heightBox.Value;
        source.Width = (int) widthBox.Value;
        source.TileType = (TileType) typeTypeBox.SelectedItem;
        return source;
      }
      else
      {
        return null;
      }
    }
  }
}
