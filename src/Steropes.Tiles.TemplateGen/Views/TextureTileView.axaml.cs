using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Steropes.Tiles.TemplateGen.Views
{
    public class TextureTileView : UserControl
    {
        public TextureTileView()
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}