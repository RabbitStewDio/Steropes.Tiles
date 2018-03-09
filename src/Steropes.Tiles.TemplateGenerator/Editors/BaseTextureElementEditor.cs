using System;
using System.Drawing;
using System.Windows.Forms;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Editors
{
  public partial class BaseTextureElementEditor : UserControl, IDetailEditor<FormattingMetaData>
  {
    public event EventHandler InputReceived;
    bool applyingData;

    public BaseTextureElementEditor()
    {
      InitializeComponent();
      marginInput.TextChanged += NotifyInputReceived;
      borderInput.TextChanged += NotifyInputReceived;
      paddingInput.TextChanged += NotifyInputReceived;
      
      borderColorInput.SelectedColorChanged += NotifyInputReceived;
      backgroundInput.SelectedColorChanged += NotifyInputReceived;
      textColorInput.SelectedColorChanged += NotifyInputReceived;

      titleInput.TextChanged += NotifyInputReceived;

      Valid = true;
    }

    public bool Valid { get; }

    void NotifyInputReceived(object source, EventArgs arg)
    {
      if (applyingData)
      {
        return;
      }

      InputReceived?.Invoke(this, EventArgs.Empty);
    }

    public void ApplyFrom(FormattingMetaData data)
    {
      try
      {
        applyingData = true;

        if (data == null)
        {
          marginInput.Text = "";
          paddingInput.Text = "";
          borderInput.Text = "";

          borderColorInput.SelectedColor = Color.Empty;
          backgroundInput.SelectedColor = Color.Empty;
          textColorInput.SelectedColor = Color.Empty;
          titleInput.Text = "";
        }
        else
        {
          marginInput.SetValue(data.Margin);
          paddingInput.SetValue(data.Padding);
          borderInput.SetValue(data.Border);

          borderColorInput.SelectedColor = data.BorderColor ?? Color.Empty;
          backgroundInput.SelectedColor = data.BackgroundColor ?? Color.Empty;
          textColorInput.SelectedColor = data.TextColor ?? Color.Empty;
          titleInput.Text = data.Title;
        }
      }
      finally
      {
        applyingData = false;
      }
    }

    public FormattingMetaData ApplyTo(FormattingMetaData data)
    {
      if (data == null)
      {
        data = new FormattingMetaData();
      }

      data.Margin = (int?) marginInput.GetValue();
      data.Padding = (int?) paddingInput.GetValue();
      data.Border = (int?) borderInput.GetValue();

      data.BorderColor = borderColorInput.SelectedColor.AsNullable();
      data.BackgroundColor = backgroundInput.SelectedColor.AsNullable();
      data.TextColor = textColorInput.SelectedColor.AsNullable();
      
      data.Title = titleInput.Text;
      return data;
    }

  }
}