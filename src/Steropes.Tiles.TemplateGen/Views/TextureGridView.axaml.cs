using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Steropes.Tiles.TemplateGen.Views
{
    public class TextureGridView : UserControl
    {
        public TextureGridView()
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}