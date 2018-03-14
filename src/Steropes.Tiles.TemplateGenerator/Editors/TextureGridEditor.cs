using System;
using System.Windows.Forms;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Editors
{
  public partial class TextureGridEditor : UserControl, IDetailEditor<TextureGrid>
  {
    readonly FormValidator validator;

    public TextureGridEditor()
    {
      InitializeComponent();

      var matcherTypes = (MatcherType[]) Enum.GetValues(typeof(MatcherType));
      foreach (var type in matcherTypes)
      {
        matchTypeBox.Items.Add(type);
      }

      validator = new FormValidator()
      {
        nameTextBox.CreateValidator().ForProvider(errorProvider),
        matchTypeBox.CreateValidator().ForProvider(errorProvider),
        patternBox.CreateValidator().ForProvider(errorProvider),

        xBox.CreateValidator().ForProvider(errorProvider)
          .WithErrorCondition(Validators.LessThan(0), "X Offset must be a positive number"),
        yBox.CreateValidator().ForProvider(errorProvider)
          .WithErrorCondition(Validators.LessThan(0), "X Offset must be a positive number"),
        widthBox.CreateValidator().ForProvider(errorProvider)
          .WithErrorCondition(Validators.LessThan(1), "Grid Width must be a positive number"),
        heightBox.CreateValidator().ForProvider(errorProvider)
          .WithErrorCondition(Validators.LessThan(1), "Grid Height must be a positive number"),
        cellWidthBox.CreateValidator().ForProvider(errorProvider)
          .WithErrorCondition(Validators.LessThan(1), "Cell Width must be a positive number"),
        cellHeightBox.CreateValidator().ForProvider(errorProvider)
          .WithErrorCondition(Validators.LessThan(1), "Cell Height must be a positive number"),
        anchorXBox.CreateValidator().ForProvider(errorProvider)
          .WithErrorCondition(Validators.LessThan(0), "Anchor-X offset must be a positive number"),
        anchorYBox.CreateValidator().ForProvider(errorProvider)
          .WithErrorCondition(Validators.LessThan(0), "Anchor-Y offset must be a positive number"),
        cellMapElementBox.CreateValidator().ForProvider(errorProvider),
        cellSpacingBox.CreateValidator().ForProvider(errorProvider)
          .WithErrorCondition(Validators.LessThan(0), "Cell Spacing must be a positive number")
      };

      validator.InputReceived += OnInputReceived;
      matchTypeBox.SelectedValueChanged += OnMatchTypeChanged;
    }

    void OnMatchTypeChanged(object sender, EventArgs e)
    {
      var m = (MatcherType) matchTypeBox.SelectedItem;
      widthBox.Enabled = m.CanAddTiles();
      heightBox.Enabled = m.CanAddTiles();
      patternBox.Enabled = !m.CanAddTiles();
      cellMapElementBox.Enabled = m == MatcherType.CellMap;
    }

    public event EventHandler InputReceived;

    void OnInputReceived(object s, EventArgs e)
    {
      InputReceived?.Invoke(this, EventArgs.Empty);
    }

    public bool Valid => validator.Valid;

    public void ApplyFrom(TextureGrid data)
    {
      try
      {
        validator.SuspendValidation();

        if (data == null)
        {
          widthBox.Text = "";
          heightBox.Text = "";
          xBox.Text = "";
          yBox.Text = "";
          cellWidthBox.Text = "";
          cellHeightBox.Text = "";
          cellSpacingBox.Text = "";
          anchorXBox.Text = "";
          anchorYBox.Text = "";

          nameTextBox.Text = "";
          patternBox.Text = "";
          matchTypeBox.SelectedItem = MatcherType.Basic;
          cellMapElementBox.Text = "";
        }
        else
        {
          widthBox.SetValue(data.Width);
          heightBox.SetValue(data.Height);
          xBox.SetValue(data.X);
          yBox.SetValue(data.Y);
          cellWidthBox.SetValue(data.CellWidth);
          cellHeightBox.SetValue(data.CellHeight);
          cellSpacingBox.SetValue(data.CellSpacing);
          anchorXBox.SetValue(data.AnchorX);
          anchorYBox.SetValue(data.AnchorY);

          nameTextBox.Text = data.Name;
          patternBox.Text = data.Pattern;
          matchTypeBox.SelectedItem = data.MatcherType;
          cellMapElementBox.Text = data.CellMapElements;
        }
      }
      finally
      {
        validator.ResumeValidation();
      }
    }

    public TextureGrid ApplyTo(TextureGrid data)
    {
      if (data == null)
      {
        data = new TextureGrid();
      }

      data.Width = (int?) widthBox.GetValue();
      data.Height = (int?) heightBox.GetValue();
      data.X = (int) xBox.Value;
      data.Y = (int) yBox.Value;
      data.CellWidth = (int?) cellWidthBox.GetValue();
      data.CellHeight = (int?) cellHeightBox.GetValue();
      data.AnchorX = (int?) anchorXBox.GetValue();
      data.AnchorX = (int?) anchorYBox.GetValue();
      data.CellMapElements = cellMapElementBox.Text;
      data.CellSpacing = (int) cellSpacingBox.Value;
      data.Name = nameTextBox.Text;
      data.Pattern = patternBox.Text;
      if (matchTypeBox.SelectedItem != null)
      {
        data.MatcherType = (MatcherType) matchTypeBox.SelectedItem;
      }
      return data;
    }
  }
}
