using System;
using System.Windows.Forms;
using Krypton.Toolkit;

namespace Steropes.Tiles.TemplateGenerator.Editors
{
  public static class Validators
  {
    public static Validator<ComboBox> CreateValidator(this ComboBox control)
    {
      var v = new Validator<ComboBox>(control);
      control.SelectedIndexChanged += v.OnInputReceived;
      return v;
    }

    public static GlobalValidator CreateGlobalValidatorFor(this FormValidator validator, params Control[] control)
    {
      var v = new GlobalValidator(control);
      validator.InputReceived += v.OnFormInputReceived;
      validator.Add(v);
      return v;
    }

    public static Validator<TextBox> CreateValidator(this TextBox control)
    {
      var v = new Validator<TextBox>(control);
      control.TextChanged += v.OnInputReceived;
      return v;
    }

    public static bool TextEmpty(Control c)
    {
      return string.IsNullOrEmpty(c.Text);
    }

    public static Validator<KryptonColorButton> CreateValidator(this KryptonColorButton control)
    {
      var v = new Validator<KryptonColorButton>(control);
      control.SelectedColorChanged += v.OnInputReceived;
      return v;
    }

    public static Validator<NumericUpDown> CreateValidator(this NumericUpDown control)
    {
      var v = new Validator<NumericUpDown>(control);
      control.TextChanged += v.OnInputReceived;
      return v;
    }

    public static Func<NumericUpDown, bool> LessThan(decimal limit)
    {
      return n =>
      {
        if (string.IsNullOrEmpty(n.Text))
        {
          // there are other checks for that if needed.
          return false;
        }
        return n.Value < limit;
      };
    }
  }
}