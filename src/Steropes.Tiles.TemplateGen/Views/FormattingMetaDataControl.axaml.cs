using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Steropes.Tiles.TemplateGen.Views
{
    public class FormattingMetaDataControl : UserControl
    {
        public FormattingMetaDataControl()
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}