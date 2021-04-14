using JetBrains.Annotations;
using ReactiveUI;
using Steropes.Tiles.DataStructures;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

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

        readonly Dictionary<string, CellMappingDeclaration> cellMappingsByKey;
        bool cellMappingsByKeyDirty;
        
        public TextureGrid(FormattingMetaData? md = null, TextureTileFormattingMetaData? tmd = null, TileTextureCollection? parent = null)
        {
            Parent = parent;
            Tiles = new BulkChangeObservableCollection<TextureTile>();
            Tiles.CollectionChanged += OnTilesChanged;
            FormattingMetaData = md ?? new FormattingMetaData();
            FormattingMetaData.PropertyChanged += OnFormattingDataChanged;
            TextureTileFormattingMetaData = tmd ?? new TextureTileFormattingMetaData();
            TextureTileFormattingMetaData.PropertyChanged += OnFormattingDataChanged;
            CellMappings = new BulkChangeObservableCollection<CellMappingDeclaration>();
            CellMappings.CollectionChanged += OnCellMappingsChanged;

            AddCellMappingCommand = ReactiveCommand.Create(OnAddCellMapping);
            RemoveCellMappingCommand = ReactiveCommand.Create<CellMappingDeclaration>(OnRemoveCellMapping);

            cellMappingsByKey = new Dictionary<string, CellMappingDeclaration>();
            cellMappingsByKeyDirty = true;
        }

        public bool TryGetCellMapping(string key, out CellMappingDeclaration d)
        {
            if (cellMappingsByKeyDirty)
            {
                cellMappingsByKey.Clear();
                foreach (var m in CellMappings.ToArray())
                {
                    if (m.Key != null)
                    {
                        cellMappingsByKey[m.Key] = m;
                    } 
                }
            }

            return cellMappingsByKey.TryGetValue(key, out d);
        }
        
        void OnAddCellMapping()
        {
            CellMappings.Add(new CellMappingDeclaration()
            {
                HighlightColor = TextureParserExtensions.Colors[CellMappings.Count % TextureParserExtensions.Colors.Count]
            });
        }

        void OnRemoveCellMapping(CellMappingDeclaration? d)
        {
            if (d == null)
            {
                return;
            }

            CellMappings.Remove(d);
        }

        public BulkChangeObservableCollection<CellMappingDeclaration> CellMappings { get; }
        
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

        
        [Obsolete]
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

        public ICommand AddCellMappingCommand { get; }
        public ICommand RemoveCellMappingCommand { get; }


        void OnCellMappingsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (CellMappingDeclaration item in e.OldItems)
                {
                    item.PropertyChanged -= OnCellMappingChanged;
                }
            }

            if (e.NewItems != null)
            {
                foreach (CellMappingDeclaration item in e.NewItems)
                {
                    item.PropertyChanged += OnCellMappingChanged;
                }
            }
            
            FireContentsChanged();
        }

        void OnCellMappingChanged(object? sender, PropertyChangedEventArgs e)
        {
            cellMappingsByKey.Clear();
            cellMappingsByKeyDirty = true;
        }

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
            };

            foreach (var tile in Tiles)
            {
                retval.Tiles.Add(tile.CreateDeepCopy());
            }

            foreach (var m in CellMappings)
            {
                retval.CellMappings.Add(m.CreateCopy());
            }

            return retval;
        }
    }
}
