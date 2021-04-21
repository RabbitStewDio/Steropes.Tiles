using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Steropes.Tiles.TemplateGen.Views
{
    public class TextureFileView : UserControl
    {
        public TextureFileView()
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}