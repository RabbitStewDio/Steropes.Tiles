using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Steropes.Tiles.TemplateGen.Views
{
    public class TextureCollectionView : UserControl
    {
        public TextureCollectionView()
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}