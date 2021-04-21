using JetBrains.Annotations;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Steropes.Tiles.TemplateGen.Models
{
    /// <summary>
    ///    Represents a single texture file (or sprite sheet). The texture can be subdivided
    ///    into separate texture grids.
    /// </summary>
    public class TileTextureCollection : INotifyPropertyChanged, IFormattingInfoProvider
    {
        public event EventHandler? ContentsChanged;
        
        string? id;
        string? lastExportLocation;

        public TileTextureCollection(FormattingMetaData? md = null, TextureSetFile? parent = null)
        {
            Parent = parent;
            Grids = new ObservableCollection<TextureGrid>();
            Grids.CollectionChanged += OnCollectionChanged;
            FormattingMetaData = md ?? new FormattingMetaData();
            FormattingMetaData.PropertyChanged += OnFormattingDataChanged;
        }

        void OnFormattingDataChanged(object? sender, PropertyChangedEventArgs e)
        {
            FireContentsChanged();
        }

        public string? LastExportLocation
        {
            get { return lastExportLocation; }
            set
            {
                if (value == lastExportLocation) return;
                lastExportLocation = value;
                OnPropertyChanged();
            }
        }

        public TileTextureCollection WithTextureGrid(TextureGrid g)
        {
            Grids.Add(g);
            return this;
        }

        public FormattingMetaData FormattingMetaData { get; }

        public ObservableCollection<TextureGrid> Grids { get; }

        public TextureSetFile? Parent { get; set; }

        public string? Id
        {
            get { return id; }
            set
            {
                if (value == id)
                {
                    return;
                }

                id = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (TextureGrid item in e.OldItems)
                {
                    item.Parent = null;
                }
            }

            if (e.NewItems != null)
            {
                foreach (TextureGrid item in e.NewItems)
                {
                    item.Parent?.Grids.Remove(item);
                    item.Parent = this;
                }
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            FireContentsChanged();
        }

        public void FireContentsChanged()
        {
            ContentsChanged?.Invoke(this, EventArgs.Empty);
            Parent?.FireContentsChanged();
        }

        public TileTextureCollection CreateDeepCopy()
        {
            var retval = new TileTextureCollection(FormattingMetaData.CreateCopy());
            foreach (var g in Grids)
            {
                retval.Grids.Add(g.CreateDeepCopy());
            }

            return retval;
        }
    }
}
