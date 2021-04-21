using Avalonia.Media;
using JetBrains.Annotations;
using SkiaSharp;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Steropes.Tiles.TemplateGen.Models.Prefs
{
    public class GeneratorPreferences : INotifyPropertyChanged
    {
        TileType defaultTileType;
        int defaultWidth;
        int defaultHeight;
        int textSpacing;
        Color defaultTileHighlightColor;
        Color defaultTileAnchorColor;
        Color defaultTileColor;
        Color defaultTextColor;
        Font defaultFont;
        Color defaultBorderColor;
        Color? defaultBackgroundColor;
        public event PropertyChangedEventHandler? PropertyChanged;
        int? defaultGridBorder;
        int? defaultGridMargin;
        int? defaultGridPadding;
        int? defaultCollectionBorder;
        int? defaultCollectionMargin;
        int? defaultCollectionPadding;
        int? defaultCellPadding;

        public GeneratorPreferences()
        {
            TileColors = new BulkChangeObservableCollection<Color>
            {
                Colors.Aquamarine,
                Colors.CornflowerBlue,
                Colors.Coral,
                Colors.Gold,
                Colors.MediumOrchid,
                Colors.Lavender
            };
            TileColors.CollectionChanged += (_, _) => OnPropertyChanged(nameof(TileColors));

            RecentFiles = new BulkChangeObservableCollection<string>();
            RecentFiles.CollectionChanged += (_, _) => OnPropertyChanged(nameof(RecentFiles));

            DefaultTileType = TileType.Isometric;
            DefaultWidth = 96;
            DefaultHeight = 48;

            DefaultBorderColor = Colors.DarkGray;
            DefaultTileColor = Colors.DarkGray;
            DefaultTileAnchorColor = Colors.Chocolate;
            DefaultTileHighlightColor = Colors.DimGray;
            DefaultTextColor = Colors.Black;
            TextSpacing = 5;
            DefaultFont = new Font(SKTypeface.Default.FamilyName, 8);

            DefaultCellPadding = 1;
            DefaultCollectionBorder = 1;
            DefaultCollectionMargin = 1;
            DefaultCollectionPadding = 2;
            DefaultGridBorder = 1;
            DefaultGridMargin = 1;
            DefaultGridPadding = 2;
        }

        public int? DefaultCellPadding
        {
            get { return defaultCellPadding; }
            set
            {
                if (value == defaultCellPadding) return;
                defaultCellPadding = value;
                OnPropertyChanged();
            }
        }

        public int? DefaultGridBorder
        {
            get { return defaultGridBorder; }
            set
            {
                if (value == defaultGridBorder) return;
                defaultGridBorder = value;
                OnPropertyChanged();
            }
        }

        public int? DefaultGridMargin
        {
            get { return defaultGridMargin; }
            set
            {
                if (value == defaultGridMargin) return;
                defaultGridMargin = value;
                OnPropertyChanged();
            }
        }

        public int? DefaultGridPadding
        {
            get { return defaultGridPadding; }
            set
            {
                if (value == defaultGridPadding) return;
                defaultGridPadding = value;
                OnPropertyChanged();
            }
        }

        public int? DefaultCollectionBorder
        {
            get { return defaultCollectionBorder; }
            set
            {
                if (value == defaultCollectionBorder) return;
                defaultCollectionBorder = value;
                OnPropertyChanged();
            }
        }

        public int? DefaultCollectionMargin
        {
            get { return defaultCollectionMargin; }
            set
            {
                if (value == defaultCollectionMargin) return;
                defaultCollectionMargin = value;
                OnPropertyChanged();
            }
        }

        public int? DefaultCollectionPadding
        {
            get { return defaultCollectionPadding; }
            set
            {
                if (value == defaultCollectionPadding) return;
                defaultCollectionPadding = value;
                OnPropertyChanged();
            }
        }

        public Font DefaultFont
        {
            get { return defaultFont; }
            set
            {
                if (defaultFont == value) return;
                defaultFont = value;
                OnPropertyChanged();
            }
        }

        public int TextSpacing
        {
            get { return textSpacing; }
            set
            {
                if (value == textSpacing) return;
                textSpacing = value;
                OnPropertyChanged();
            }
        }

        public Color DefaultTileHighlightColor
        {
            get { return defaultTileHighlightColor; }
            set
            {
                if (value.Equals(defaultTileHighlightColor)) return;
                defaultTileHighlightColor = value;
                OnPropertyChanged();
            }
        }

        public Color DefaultTileAnchorColor
        {
            get { return defaultTileAnchorColor; }
            set
            {
                if (value.Equals(defaultTileAnchorColor)) return;
                defaultTileAnchorColor = value;
                OnPropertyChanged();
            }
        }

        public Color DefaultTileColor
        {
            get { return defaultTileColor; }
            set
            {
                if (value.Equals(defaultTileColor)) return;
                defaultTileColor = value;
                OnPropertyChanged();
            }
        }

        public Color DefaultTextColor
        {
            get { return defaultTextColor; }
            set
            {
                if (value.Equals(defaultTextColor)) return;
                defaultTextColor = value;
                OnPropertyChanged();
            }
        }

        public int DefaultWidth
        {
            get { return defaultWidth; }
            set
            {
                if (value == defaultWidth) return;
                defaultWidth = value;
                OnPropertyChanged();
            }
        }

        public int DefaultHeight
        {
            get { return defaultHeight; }
            set
            {
                if (value == defaultHeight) return;
                defaultHeight = value;
                OnPropertyChanged();
            }
        }

        public TileType DefaultTileType
        {
            get { return defaultTileType; }
            set
            {
                if (value == defaultTileType) return;
                defaultTileType = value;
                OnPropertyChanged();
            }
        }

        public BulkChangeObservableCollection<string> RecentFiles { get; }

        public BulkChangeObservableCollection<Color> TileColors { get; }

        public Color DefaultBorderColor
        {
            get { return defaultBorderColor; }
            set
            {
                if (value.Equals(defaultBorderColor)) return;
                defaultBorderColor = value;
                OnPropertyChanged();
            }
        }

        public Color? DefaultBackgroundColor
        {
            get { return defaultBackgroundColor; }
            set
            {
                if (value.Equals(defaultBackgroundColor)) return;
                defaultBackgroundColor = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Save()
        { }

        public void AddRecentFile(string file)
        {
            if (RecentFiles.Contains(file))
            {
                RecentFiles.Remove(file);
            }

            RecentFiles.Add(file);
            if (RecentFiles.Count > 10)
            {
                RecentFiles.RemoveAt(0);
            }
        }
    }
}
