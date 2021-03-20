using System;
using System.Drawing;
using System.Windows.Forms;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Editors
{
    public partial class TextureGridElementEditor : UserControl, IDetailEditor<TextureTileFormattingMetaData>
    {
        readonly FormValidator validator;

        public TextureGridElementEditor()
        {
            InitializeComponent();

            validator = new FormValidator
            {
                outlineColorBox.CreateValidator().ForProvider(errorProvider),
                highlightColorBox.CreateValidator().ForProvider(errorProvider)
            };
            validator.InputReceived += OnInputReceived;
        }

        void OnInputReceived(object sender, EventArgs e)
        {
            InputReceived?.Invoke(this, e);
        }

        public bool Valid => validator.Valid;

        public void ApplyFrom(TextureTileFormattingMetaData data)
        {
            try
            {
                validator.SuspendValidation();

                if (data == null)
                {
                    outlineColorBox.SelectedColor = Color.Empty;
                    highlightColorBox.SelectedColor = Color.Empty;
                }
                else
                {
                    outlineColorBox.SelectedColor = data.TileOutlineColor ?? Color.Empty;
                    highlightColorBox.SelectedColor = data.TileHighlightColor ?? Color.Empty;
                }
            }
            finally
            {
                validator.ResumeValidation();
            }
        }

        public TextureTileFormattingMetaData ApplyTo(TextureTileFormattingMetaData data)
        {
            if (data == null)
            {
                data = new TextureTileFormattingMetaData();
            }

            data.TileHighlightColor = highlightColorBox.SelectedColor.AsNullable();
            data.TileOutlineColor = outlineColorBox.SelectedColor.AsNullable();

            return data;
        }

        public event EventHandler InputReceived;
    }
}
