using System;
using System.Drawing;
using System.Windows.Forms;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Editors
{
    public partial class BaseTextureElementEditor : UserControl, IDetailEditor<FormattingMetaData>
    {
        protected FormValidator Validator { get; }
        public event EventHandler InputReceived;

        public BaseTextureElementEditor()
        {
            InitializeComponent();

            Validator = new FormValidator
            {
                marginInput.CreateValidator()
                           .ForProvider(errorProvider)
                           .WithErrorCondition(Validators.LessThan(0), "Margin must be a positive number"),
                borderInput.CreateValidator()
                           .ForProvider(errorProvider)
                           .WithErrorCondition(Validators.LessThan(0), "Border must be a positive number"),
                paddingInput.CreateValidator()
                            .ForProvider(errorProvider)
                            .WithErrorCondition(Validators.LessThan(0), "Padding must be a positive number"),

                backgroundInput.CreateValidator().ForProvider(errorProvider),
                borderColorInput.CreateValidator().ForProvider(errorProvider),
                textColorInput.CreateValidator().ForProvider(errorProvider),

                titleInput.CreateValidator().ForProvider(errorProvider)
            };
            Validator.InputReceived += NotifyInputReceived;
        }

        public bool Valid => Validator.Valid;

        protected void NotifyInputReceived(object source, EventArgs arg)
        {
            InputReceived?.Invoke(this, EventArgs.Empty);
        }

        public void ApplyFrom(FormattingMetaData data)
        {
            try
            {
                Validator.SuspendValidation();

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
                Validator.ResumeValidation();
            }
        }

        public FormattingMetaData ApplyTo(FormattingMetaData data)
        {
            if (data == null)
            {
                data = new FormattingMetaData();
            }

            data.Margin = (int?)marginInput.GetValue();
            data.Padding = (int?)paddingInput.GetValue();
            data.Border = (int?)borderInput.GetValue();

            data.BorderColor = borderColorInput.SelectedColor.AsNullable();
            data.BackgroundColor = backgroundInput.SelectedColor.AsNullable();
            data.TextColor = textColorInput.SelectedColor.AsNullable();

            data.Title = titleInput.Text;
            return data;
        }
    }
}
