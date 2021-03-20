using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Steropes.Tiles.TemplateGenerator.Editors
{
    public class GlobalValidator : IValidator
    {
        readonly List<Control> controls;
        readonly List<Tuple<Func<bool>, string>> errorChecks;
        ErrorProvider errorProvider;
        bool valid;

        public GlobalValidator(params Control[] controls)
        {
            this.controls = new List<Control>(controls);
            this.errorChecks = new List<Tuple<Func<bool>, string>>();
        }

        public GlobalValidator ForProvider(ErrorProvider provider)
        {
            this.errorProvider = provider ?? throw new ArgumentNullException(nameof(provider));
            return this;
        }

        public GlobalValidator WithErrorCondition(Func<bool> errorCheck, string message)
        {
            errorChecks.Add(new Tuple<Func<bool>, string>(errorCheck, message));
            return this;
        }

        public void Validate()
        {
            ValidateInternal();
            Console.WriteLine("Manual Validate all done : " + Valid);
        }

        void ValidateInternal()
        {
            foreach (var control in controls)
            {
                var error = errorProvider?.GetError(control);
                if (!string.IsNullOrEmpty(error))
                {
                    if (!errorChecks.Select(e => e.Item2).Contains(error))
                    {
                        // any error not caused by this validator has precedence.
                        // This ensures that global rules spanning multiple controls
                        // are only evaluated when the more basic single-control
                        // validations pass.
                        Console.WriteLine("Unable to valiate : " + Valid);
                        return;
                    }
                }
            }

            foreach (var check in errorChecks)
            {
                if (check.Item1())
                {
                    ApplyError(check.Item2);
                    Valid = false;
                    return;
                }
            }

            ApplyError(null);
            Valid = true;
        }

        void ApplyError(string error)
        {
            Console.WriteLine("Applying error " + error);
            foreach (var control in controls)
            {
                errorProvider?.SetError(control, error);
            }
        }

        public event EventHandler ValidationStateChanged;

        // Not used ..
        public event EventHandler InputReceived { add { } remove { } }

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

        public void OnFormInputReceived(object sender, EventArgs e)
        {
            Console.WriteLine("Trigger Validate all");
            Validate();
            Console.WriteLine("Trigger Validate all done : " + Valid);
        }
    }

    public class FormValidator : IEnumerable<IValidator>
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
}
