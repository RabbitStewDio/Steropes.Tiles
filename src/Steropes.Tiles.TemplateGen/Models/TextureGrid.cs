using JetBrains.Annotations;
using Steropes.Tiles.DataStructures;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Steropes.Tiles.TemplateGen.Models
{
    public class TextureGrid : INotifyPropertyChanged, IFormattingInfoProvider
    {
        int? anchorX;
        int? anchorY;

        int x;
        int y;
        int? width;
        int? height;

        int? cellWidth;
        int? cellHeight;
        int cellSpacing;

        MatcherType matcherType;
        string? pattern;
        string? name;
        string? cellMapElements;

        public TextureGrid(FormattingMetaData? md = null, TextureTileFormattingMetaData? tmd = null, TileTextureCollection? parent = null)
        {
            Parent = parent;
            Tiles = new BulkChangeObservableCollection<TextureTile>();
            Tiles.CollectionChanged += OnTilesChanged;
            FormattingMetaData = md ?? new FormattingMetaData();
            FormattingMetaData.PropertyChanged += OnFormattingDataChanged;
            TextureTileFormattingMetaData = tmd ?? new TextureTileFormattingMetaData();
            TextureTileFormattingMetaData.PropertyChanged += OnFormattingDataChanged;
        }

        void OnFormattingDataChanged(object? sender, PropertyChangedEventArgs e)
        {
            FireContentsChanged();
        }

        public TextureGrid WithTextureTile([NotNull] TextureTile t)
        {
            if (t == null)
            {
                throw new ArgumentNullException(nameof(t));
            }

            Tiles.Add(t);
            return this;
        }

        public TextureTileFormattingMetaData TextureTileFormattingMetaData { get; }
        public FormattingMetaData FormattingMetaData { get; }

        public int CellSpacing
        {
            get { return cellSpacing; }
            set
            {
                if (value == cellSpacing) return;
                cellSpacing = value;
                OnPropertyChanged();
            }
        }

        public MatcherType[] AvailableMatcherTypes => Enum.GetValues<MatcherType>();

        public bool IsCellMapping => MatcherType == MatcherType.CellMap;

        public string? CellMapElements
        {
            get { return cellMapElements; }
            set
            {
                if (value == cellMapElements) return;
                cellMapElements = value;
                OnPropertyChanged();
            }
        }

        public MatcherType MatcherType
        {
            get { return matcherType; }
            set
            {
                if (value == matcherType) return;
                matcherType = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCellMapping));
            }
        }

        public string? Pattern
        {
            get { return pattern; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    value = null;
                }

                if (value == pattern) return;
                pattern = value;
                OnPropertyChanged();
            }
        }

        public string? Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    value = null;
                }

                if (value == name)
                {
                    return;
                }

                name = value;
                OnPropertyChanged();
            }
        }

        public IntPoint Position => new IntPoint(X, Y);

        public int X
        {
            get { return x; }
            set
            {
                if (value == x)
                {
                    return;
                }

                x = value;
                OnPropertyChanged();
            }
        }

        public int Y
        {
            get { return y; }
            set
            {
                if (value == y)
                {
                    return;
                }

                y = value;
                OnPropertyChanged();
            }
        }

        public int? CellWidth
        {
            get { return cellWidth; }
            set
            {
                if (value == cellWidth) return;
                cellWidth = value;
                OnPropertyChanged();
            }
        }

        public int? CellHeight
        {
            get { return cellHeight; }
            set
            {
                if (value == cellHeight) return;
                cellHeight = value;
                OnPropertyChanged();
            }
        }

        public int? Width
        {
            get { return width; }
            set
            {
                if (value == width)
                {
                    return;
                }

                width = value;
                OnPropertyChanged();
            }
        }

        public int? Height
        {
            get { return height; }
            set
            {
                if (value == height)
                {
                    return;
                }

                height = value;
                OnPropertyChanged();
            }
        }

        public int? AnchorX
        {
            get { return anchorX; }
            set
            {
                if (value == anchorX)
                {
                    return;
                }

                anchorX = value;
                OnPropertyChanged();
            }
        }

        public int? AnchorY
        {
            get { return anchorY; }
            set
            {
                if (value == anchorY)
                {
                    return;
                }

                anchorY = value;
                OnPropertyChanged();
            }
        }

        public List<string> EffectiveCellMapElements
        {
            get
            {
                if (string.IsNullOrWhiteSpace(CellMapElements))
                {
                    return new List<string>();
                }

                return CellMapElements.Split(null).Distinct().ToList();
            }
        }


        /// <summary>
        ///  Returns the effective tile size, that is the standardized 
        /// space a tile consumes on screen. Tiles can be larger than
        /// that to overlap with neighbouring cells if needed, which is
        /// measured as EffectiveCellSize.
        /// </summary>
        public IntDimension EffectiveTileSize
        {
            get
            {
                var textureSetFile = Parent?.Parent;
                if (textureSetFile == null)
                {
                    return new IntDimension();
                }

                var retval = new IntDimension(textureSetFile.Width, textureSetFile.Height);
                if (matcherType == MatcherType.Corner)
                {
                    retval = new IntDimension(retval.Width / 2, retval.Height / 2);
                }

                return retval;
            }
        }

        public IntDimension EffectiveCellSize
        {
            get
            {
                var retval = EffectiveTileSize;
                return new IntDimension(cellWidth ?? retval.Width, cellHeight ?? retval.Height);
            }
        }

        /// <summary>
        ///  The effective anchor point, if not defined otherwise
        ///  will be aligned to the bottom edge of the graphic and
        ///  centred horizontally.
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        public IntPoint ComputeEffectiveAnchorPoint(TextureTile tile)
        {
            var tileSize = EffectiveTileSize;
            var size = EffectiveCellSize;
            var anchorPointX = tile.AnchorX ?? AnchorX ?? size.Width / 2;
            var anchorPointY = tile.AnchorY ?? AnchorY ?? size.Height - tileSize.Height / 2;
            return new IntPoint(anchorPointX, anchorPointY);
        }

        public TileTextureCollection? Parent { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        public BulkChangeObservableCollection<TextureTile> Tiles { get; }

        void OnTilesChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (TextureTile item in e.OldItems)
                {
                    item.Parent = null;
                }
            }

            if (e.NewItems != null)
            {
                foreach (TextureTile item in e.NewItems)
                {
                    item.Parent?.Tiles.Remove(item);
                    item.Parent = this;
                }
            }
            
            FireContentsChanged();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            FireContentsChanged();
        }

        public void FireContentsChanged()
        {
            Parent?.FireContentsChanged();
        }

        public TextureGrid CreateDeepCopy()
        {
            var retval = new TextureGrid(FormattingMetaData.CreateCopy(),
                                         TextureTileFormattingMetaData.CreateCopy())
            {
                X = X,
                Y = Y,
                Width = Width,
                Height = Height,
                AnchorX = AnchorX,
                AnchorY = AnchorY,
                CellWidth = CellWidth,
                CellHeight = CellHeight,
                CellSpacing = CellSpacing,
                MatcherType = MatcherType,
                Pattern = Pattern,
                Name = Name,
                CellMapElements = CellMapElements
            };

            foreach (var tile in Tiles)
            {
                retval.Tiles.Add(tile.CreateDeepCopy());
            }

            return retval;
        }
    }
}
