using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Steropes.Tiles.TemplateGen.Models
{
    public class TextureSetFile : INotifyPropertyChanged, IFormattingInfoProvider
    {
        public event EventHandler? TextureSetContentsChanged;

        readonly Dictionary<string, string> properties;

        int width;
        int height;
        bool modified;
        string? name;
        string? sourcePath;
        TileType tileType;
        TextureSetFile? parent;

        public TextureSetFile(FormattingMetaData? md = null)
        {
            properties = new Dictionary<string, string>();

            Collections = new ObservableCollection<TileTextureCollection>();
            Collections.CollectionChanged += OnCollectionChanged;
            FormattingMetaData = md ?? new FormattingMetaData();
            IncludeFiles = new ObservableCollection<TextureSetFile>();
            IncludeFiles.CollectionChanged += OnIncludesChanged;

            Level = new BulkChangeObservableCollection<object>();
        }

        public ObservableCollection<TextureSetFile> IncludeFiles { get; }
        public ObservableCollection<TileTextureCollection> Collections { get; }

        public BulkChangeObservableCollection<object> Level { get; }
        
        public void FireContentsChanged()
        {
            Modified = true;
            TextureSetContentsChanged?.Invoke(this, EventArgs.Empty);
        }

        public TextureSetFile WithTextureCollection(TileTextureCollection c)
        {
            if (c == null)
            {
                throw new ArgumentNullException(nameof(c));
            }

            Collections.Add(c);
            return this;
        }

        public FormattingMetaData FormattingMetaData { get; }

        public TextureSetFile? Parent
        {
            get { return parent; }
            set
            {
                if (value == parent)
                {
                    return;
                }

                parent = value;
                OnPropertyChanged();
            }
        }

        public int Width
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

        public int Height
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

        public TileType[] AvailableTileTypes => Enum.GetValues<TileType>();

        public TileType TileType
        {
            get { return tileType; }
            set
            {
                if (value == tileType)
                {
                    return;
                }

                tileType = value;
                OnPropertyChanged();
            }
        }

        public string? SourcePath
        {
            get { return sourcePath; }
            set
            {
                var oldSourcePath = sourcePath;
                if (value == sourcePath)
                {
                    return;
                }

                sourcePath = value;
                OnPropertyChanged();
                ModifyTemplatePaths(oldSourcePath, sourcePath);
            }
        }

        void ModifyTemplatePaths(string? oldSourcePath, string? newSourcePath)
        {
            // first: Make all paths absolute.
            if (oldSourcePath != null)
            {
                foreach (var c in Collections)
                {
                    if (c.LastExportLocation == null)
                    {
                        continue;
                    }

                    var l = c.LastExportLocation;
                    if (!Path.IsPathRooted(l))
                    {
                        l = Path.Combine(oldSourcePath, l);
                    }

                    c.LastExportLocation = l;
                }
            }

            if (newSourcePath != null)
            {
                foreach (var c in Collections)
                {
                    if (c.LastExportLocation == null)
                    {
                        continue;
                    }

                    var l = c.LastExportLocation;
                    if (Path.IsPathRooted(l))
                    {
                        c.LastExportLocation = Path.GetRelativePath(newSourcePath, l);
                    }
                }
            }
        }

        public string? Name
        {
            get { return name; }
            set
            {
                if (value == name)
                {
                    return;
                }

                name = value;
                OnPropertyChanged();
            }
        }

        public bool Modified
        {
            get { return modified; }
            set
            {
                if (value == modified)
                {
                    return;
                }

                modified = value;
                OnPropertyChanged();
            }
        }

        public IReadOnlyDictionary<string, string> Properties
        {
            get { return properties; }
            set
            {
                if (Equals(value, properties))
                {
                    return;
                }

                properties.Clear();
                foreach (var (key, v) in value)
                {
                    properties.Add(key, v);
                }

                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (TileTextureCollection item in e.OldItems)
                {
                    item.Parent = null;
                }
            }

            if (e.NewItems != null)
            {
                foreach (TileTextureCollection item in e.NewItems)
                {
                    item.Parent?.Collections.Remove(item);
                    item.Parent = this;
                }
            }

            OnPropertyChanged(nameof(Collections));
            RebuildContents();
        }

        void OnIncludesChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(IncludeFiles));
            RebuildContents();
        }

        void RebuildContents()
        {
            var l = new List<object>();
            l.AddRange(IncludeFiles);
            l.AddRange(Collections);
            Level.ReplaceAll(l);
        }
        
        public string BasePath
        {
            get
            {
                try
                {
                    var path = SourcePath;
                    if (path != null)
                    {
                        var p = Path.GetFullPath(path);
                        var retval = Directory.GetParent(p)?.FullName;
                        if (!string.IsNullOrEmpty(retval))
                        {
                            if (retval.EndsWith("" + Path.DirectorySeparatorChar) ||
                                retval.EndsWith("" + Path.AltDirectorySeparatorChar))
                            {
                                return retval;
                            }

                            return retval + Path.DirectorySeparatorChar;
                        }
                    }

                    return "";
                }
                catch (IOException)
                {
                    // yeah, ugly, but this stuff above can fail for a myriad of reasons,
                    // not all of them sane.
                    return "";
                }
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            FireContentsChanged();
        }
    }
}
