using System;
using System.Windows.Forms;
using Steropes.Tiles.TemplateGenerator.Model;
using static Steropes.Tiles.TemplateGenerator.Editors.Validators;

namespace Steropes.Tiles.TemplateGenerator.Editors
{
    public partial class TextureFileEditor : UserControl, IDetailEditor<TextureFile>
    {
        readonly FormValidator formValidator;

        public TextureFileEditor()
        {
            InitializeComponent();

            var tileTypes = (TileType[])Enum.GetValues(typeof(TileType));
            foreach (var type in tileTypes)
            {
                typeTypeBox.Items.Add(type);
            }

            formValidator = new FormValidator
            {
                nameTextBox.CreateValidator().ForProvider(errorProvider),
                typeTypeBox.CreateValidator().ForProvider(errorProvider),

                heightBox.CreateValidator()
                         .ForProvider(errorProvider)
                         .WithErrorCondition(TextEmpty, "Height must be defined.")
                         .WithErrorCondition(LessThan(1), "Height must be non zero."),
                widthBox.CreateValidator()
                        .ForProvider(errorProvider)
                        .WithErrorCondition(TextEmpty, "Width must be defined.")
                        .WithErrorCondition(LessThan(1), "Width must be non zero."),
            };
            formValidator.InputReceived += OnInputReceived;
            formValidator.ValidationStateChanged += OnValidationStateChanged;
        }

        bool IsNotCorrectSize()
        {
            var w = (int)widthBox.Value;
            var h = (int)heightBox.Value;
            if (w % 8 == 0 && h % 8 == 0)
            {
                return false;
            }

            return true;
        }

        bool IsIrregularShape()
        {
            if (TileType.Isometric.Equals(typeTypeBox.SelectedItem))
            {
                var w = (int)widthBox.Value;
                var h = (int)heightBox.Value;
                return w != h * 2;
            }

            return false;
        }

        void OnValidationStateChanged(object sender, EventArgs e)
        {
            ValidationStateChanged?.Invoke(this, EventArgs.Empty);
        }

        void OnInputReceived(object s, EventArgs e)
        {
            InputReceived?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler InputReceived;
        public event EventHandler ValidationStateChanged;

        public bool Valid => formValidator.Valid;

        public void ApplyFrom(TextureFile source)
        {
            try
            {
                formValidator.SuspendValidation();

                if (source == null)
                {
                    this.widthBox.Value = 64;
                    this.heightBox.Value = 32;
                    this.nameTextBox.Text = "New Tile Set";
                    this.typeTypeBox.SelectedItem = TileType.Isometric;
                }
                else
                {
                    this.widthBox.Value = source.Width;
                    this.heightBox.Value = source.Height;
                    this.nameTextBox.Text = source.Name;
                    this.typeTypeBox.SelectedItem = source.TileType;
                }
            }
            finally
            {
                formValidator.ResumeValidation();
            }
        }

        public TextureFile ApplyTo(TextureFile target)
        {
            if (target == null)
            {
                target = new TextureFile();
            }

            target.Name = nameTextBox.Text;
            target.Height = (int)heightBox.Value;
            target.Width = (int)widthBox.Value;
            target.TileType = (TileType)typeTypeBox.SelectedItem;
            return target;
        }
    }
}
