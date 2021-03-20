using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Steropes.Tiles.TemplateGenerator.Editors
{
    public class Validator<T> : IValidator
        where T : Control
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

        public Validator<T> ForProvider(ErrorProvider provider)
        {
            this.errorProvider = provider ?? throw new ArgumentNullException(nameof(provider));
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
}
