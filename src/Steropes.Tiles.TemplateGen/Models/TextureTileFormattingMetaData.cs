using Avalonia.Media;
using JetBrains.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Steropes.Tiles.TemplateGen.Models
{
    public sealed class TextureTileFormattingMetaData : INotifyPropertyChanged
    {
        Color? tileOutlineColor;
        Color? tileAnchorColor;
        Color? tileHighlightColor;

        public Color? TileOutlineColor
        {
            get { return tileOutlineColor; }
            set
            {
                if (value.Equals(tileOutlineColor)) return;
                tileOutlineColor = value;
                OnPropertyChanged();
            }
        }

        public Color? TileAnchorColor
        {
            get { return tileAnchorColor; }
            set
            {
                if (value.Equals(tileAnchorColor)) return;
                tileAnchorColor = value;
                OnPropertyChanged();
            }
        }

        public Color? TileHighlightColor
        {
            get { return tileHighlightColor; }
            set
            {
                if (value.Equals(tileHighlightColor)) return;
                tileHighlightColor = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TextureTileFormattingMetaData? CreateCopy()
        {
            return new TextureTileFormattingMetaData()
            {
                TileAnchorColor = TileAnchorColor,
                TileOutlineColor = TileOutlineColor,
                TileHighlightColor = TileHighlightColor
            };
        }
    }
}
