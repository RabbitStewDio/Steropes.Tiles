using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;

namespace Steropes.Tiles.TemplateGen.Views
{
    public class ArgbColorView: UserControl
    {
        public static readonly StyledProperty<Color> ColorProperty = AvaloniaProperty.Register<ArgbColorView, Color>(nameof(Color));

        public Color Color
        {
            get { return GetValue(ColorProperty); }
            set
            {
                SetValue(ColorProperty, value);
            }
        }

        public ArgbColorView()
        {
            this.InitializeComponent();
            this.DataContextChanged += OnDataContextChanged;
            this.PropertyChanged += (_, e) =>
            {
                if (e.Property.Name == nameof(Color))
                {
                    DataContext = Color;
                }
            };
            
            
        }

        void OnDataContextChanged(object? sender, EventArgs e)
        {
            if (DataContext is Color c)
            {
                Color = c;
            }
            else
            {
                Color = default;
            }
        }

        void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
