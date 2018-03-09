using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Steropes.Tiles.TemplateGenerator.Editors
{
  public interface IValidator
  {
    void Validate();
    event EventHandler ValidationStateChanged;
    event EventHandler InputReceived;
    bool Valid { get; }
  }

  public class FormValidator: IEnumerable<IValidator>
  {
    readonly List<IValidator> validators;
    bool valid;
    bool suspended;

    public FormValidator()
    {
      validators = new List<IValidator>();
    }

    public event EventHandler ValidationStateChanged;
    public event EventHandler InputReceived;

    public bool Valid
    {
      get { return valid; }
      set
      {
        if (valid == value)
        {
          return;
        }
        valid = value;
        ValidationStateChanged?.Invoke(this, EventArgs.Empty);
      }
    }

    public void Add(IValidator v)
    {
      validators.Add(v ?? throw new ArgumentNullException());
      v.ValidationStateChanged += OnValidationChanged;
      v.InputReceived += OnInputReceived;
    }

    void OnInputReceived(object sender, EventArgs e)
    {
      if (suspended)
      {
        return;
      }

      InputReceived?.Invoke(this, EventArgs.Empty);
    }

    void OnValidationChanged(object sender, EventArgs e)
    {
      if (suspended)
      {
        return;
      }

      ValidateAll();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public IEnumerator<IValidator> GetEnumerator()
    {
      return validators.GetEnumerator();
    }

    public bool ValidateAll(bool force = false)
    {
      var computedValidState = true;
      foreach (var v in validators)
      {
        if (force)
        {
          v.Validate();
        }
        computedValidState &= v.Valid;
      }

      Valid = computedValidState;
      return computedValidState;
    }

    public void SuspendValidation()
    {
      suspended = true;
    }

    public void ResumeValidation()
    {
      suspended = false;
      ValidateAll(true);
      // force event ..
      ValidationStateChanged?.Invoke(this, EventArgs.Empty);
    }
  }

  public static class Validators
  {
    public static Validator<ComboBox> CreateValidator(this ComboBox control)
    {
      var v = new Validator<ComboBox>(control);
      control.SelectedIndexChanged += v.OnInputReceived;
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

    public static Validator<NumericUpDown> CreateValidator(this NumericUpDown control)
    {
      var v = new Validator<NumericUpDown>(control);
      control.TextChanged += v.OnInputReceived;
      return v;
    }

    public class Validator<T> : IValidator where T : Control
    {
      readonly T control;
      readonly List<Tuple<Func<T, bool>, string>> errorChecks;
      ErrorProvider errorProvider;
      bool valid;

      public Validator(T control)
      {
        this.errorChecks = new List<Tuple<Func<T, bool>, string>>();
        this.control = control ?? throw new ArgumentNullException(nameof(control));
      }

      public event EventHandler ValidationStateChanged;
      public event EventHandler InputReceived;

      public bool Valid
      {
        get { return valid; }
        set
        {
          if (valid == value)
          {
            return;
          }
          valid = value;
          ValidationStateChanged?.Invoke(this, EventArgs.Empty);
        }
      }

      public Validator<T> ForProvider(ErrorProvider errorProvider)
      {
        this.errorProvider = errorProvider ?? throw new ArgumentNullException(nameof(errorProvider));
        return this;
      }

      public Validator<T> WithErrorCondition(Func<T, bool> errorCheck, string message)
      {
        errorChecks.Add(new Tuple<Func<T, bool>, string>(errorCheck, message));
        return this;
      }

      public void OnInputReceived(object source, EventArgs args)
      {
        Validate();
        InputReceived?.Invoke(this, EventArgs.Empty);
      }

      public void Validate()
      {
        foreach (var c in errorChecks)
        {
          var fn = c.Item1;
          if (fn(control))
          {
            errorProvider?.SetError(control, c.Item2);
            Valid = false;
            return;
          }
        }

        errorProvider?.SetError(control, null);
        Valid = true;
      }
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