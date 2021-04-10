using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ReactiveUI;
using Serilog;
using SkiaSharp;
using Steropes.Tiles.TemplateGen.Bindings;
using Steropes.Tiles.TemplateGen.Models;
using Steropes.Tiles.TemplateGen.Models.Rendering;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Steropes.Tiles.TemplateGen.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        static readonly ILogger Logger = SLog.ForContext<MainWindowViewModel>();
        static readonly float[] Scales = {0.25f, 0.5f, 0.75f, 1, 1.25f, 1.5f, 2, 4, 6, 8, 10};

        bool hasValidModel;
        bool canExportSelection;
        object? selectedItem;
        float viewScale;
        TileTextureCollection? selectedTextureCollection;
        Bitmap? previewImage;
        SKBitmap? previewBitmap;

        public MainWindowViewModel()
        {
            TextureFiles = new BulkChangeObservableCollection<TextureSetFile>();
            TextureFiles.Add(CreateSample());

            UserPreferences = new UserPreferences();

            InitializeCommand = ReactiveCommand.CreateFromTask(OnInitialized);
            ShutDownCommand = ReactiveCommand.CreateFromTask(OnShutDown);
            TestCommand = ReactiveCommand.CreateFromTask(OnTest);

            ZoomInCommand = ReactiveCommand.Create(OnZoomIn, this.BindProperty(e => e.CanZoomIn));
            ZoomOutCommand = ReactiveCommand.Create(OnZoomOut, this.BindProperty(e => e.CanZoomOut));
            CreateNewCommand = ReactiveCommand.Create(OnCreateNew);
            QuitCommand = ReactiveCommand.Create(OnQuit);

            var selectedItemBinding = this.BindProperty(e => e.SelectedItem);
            AddCollectionCommand = ReactiveCommand.Create(OnAddCollection, selectedItemBinding.Select(HasAnySelection));
            AddGridCommand = ReactiveCommand.Create(OnAddGrid, selectedItemBinding.Select(CanAddGrid));
            AddTileCommand = ReactiveCommand.Create(OnAddTile, selectedItemBinding.Select(CanAddTile));
            DeleteCommand = ReactiveCommand.Create(OnDelete, selectedItemBinding.Select(HasAnySelection));
            DuplicateCommand = ReactiveCommand.Create(OnDuplicate, selectedItemBinding.Select(HasAnySelection));

            ArrangeTilesCommand = ReactiveCommand.Create(OnArrangeTiles, selectedItemBinding.Select(HasAnySelection));
            GenerateTilesCommand = ReactiveCommand.Create(OnGenerateTiles, selectedItemBinding.Select(HasAnySelection));

            var uri = new Uri("avares://Steropes.Tiles.TemplateGen/Assets/Icons/New-Document.png", UriKind.RelativeOrAbsolute);
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            PreviewImage = new Bitmap(assets.Open(uri));
        }

        void OnDuplicate()
        {
            if (selectedItem is TileTextureCollection c)
            {
                var d = c.CreateDeepCopy();
                d.Id = $"{d.Id} (Copy)";
                c.Parent?.Collections.Add(d);
            }
            else if (selectedItem is TextureGrid g)
            {
                var d = g.CreateDeepCopy();
                d.Name = $"{d.Name} (Copy)";
                g.Parent?.Grids.Add(d);
            }
            else if (selectedItem is TextureTile t)
            {
                t.Parent?.Tiles.Add(t.CreateDeepCopy());
            }
        }

        void Remove<TParent, TItem>(TParent parent, ObservableCollection<TItem> collection, TItem c)
        {
            var idx = collection.IndexOf(c);
            if (idx == -1)
            {
                return;
            }

            collection.Remove(c);
            if (collection.Count > idx)
            {
                SelectedItem = collection[idx];
            }
            else if (collection.Count == idx &&
                     idx > 0)
            {
                SelectedItem = collection[idx - 1];
            }
            else if (collection.Count > 0)
            {
                SelectedItem = collection[0];
            }
            else
            {
                SelectedItem = parent;
            }
        }

        void OnDelete()
        {
            if (selectedItem is TileTextureCollection c)
            {
                var parent = c.Parent;
                if (parent == null)
                {
                    return;
                }

                Remove(parent, parent.Collections, c);
                return;
            }

            if (selectedItem is TextureGrid g)
            {
                var parent = g.Parent;
                if (parent == null)
                {
                    return;
                }

                Remove(parent, parent.Grids, g);
                return;
            }
            
            if (selectedItem is TextureTile t)
            {
                var parent = t.Parent;
                if (parent == null)
                {
                    return;
                }

                Remove(parent, parent.Tiles, t);
                return;
            }
        }

        void OnArrangeTiles()
        {
            var collections = selectedItem switch
            {
                TextureSetFile tf => tf.Collections.ToArray(),
                TileTextureCollection tc => new[] {tc},
                TextureGrid gd => new[] {gd.Parent},
                TextureTile t => new[] {t.Parent?.Parent},
                _ => Array.Empty<TileTextureCollection>()
            };

            foreach (var c in collections)
            {
                if (c != null)
                {
                    TextureGridAutoLayout.ArrangeGrids(UserPreferences.Preferences, c);
                }
            }
        }

        void OnGenerateTiles()
        {
            var grids = selectedItem switch
            {
                TextureSetFile tf => tf.Collections.SelectMany(e => e.Grids),
                TileTextureCollection tc => tc.Grids,
                TextureGrid gd => Enumerable.Repeat(gd, 1),
                TextureTile t => Enumerable.Repeat(t.Parent, 1),
                _ => Array.Empty<TextureGrid>()
            };

            foreach (var g in grids)
            {
                if (g != null)
                {
                    TextureTileGenerator.Regenerate(g);
                }
            }
        }

        static TextureSetFile CreateSample()
        {
            return new TextureSetFile()
            {
                Modified = false,
                Width = 32,
                Height = 16,
                Name = "Built-in Sample",
                SourcePath = Path.Combine(Environment.CurrentDirectory, "sample.tileset"),
                TileType = TileType.Isometric
            }.WithTextureCollection(new TileTextureCollection()
                {
                    Id = "Sample Collection"
                }.WithTextureGrid(new TextureGrid()
                                      {
                                          AnchorX = 5,
                                          AnchorY = 5,
                                          CellSpacing = 1,
                                          Name = "Sample Texture Grid"
                                      }.WithTextureTile(new TextureTile(false, 0, 0, "Tag1", "Tag2"))
                                       .WithTextureTile(new TextureTile(false, 1, 0, "ATag1", "BTag2")))
            );
        }


        public ReactiveCommand<Unit, Unit> InitializeCommand { get; }
        public ReactiveCommand<Unit, Unit> ShutDownCommand { get; }
        public ReactiveCommand<Unit, Unit> TestCommand { get; }
        public ReactiveCommand<Unit, Unit> CreateNewCommand { get; }
        public ReactiveCommand<Unit, Unit> ZoomInCommand { get; }
        public ReactiveCommand<Unit, Unit> ZoomOutCommand { get; }
        public ReactiveCommand<Unit, Unit> AddCollectionCommand { get; }
        public ReactiveCommand<Unit, Unit> AddGridCommand { get; }
        public ReactiveCommand<Unit, Unit> AddTileCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteCommand { get; }
        public ReactiveCommand<Unit, Unit> DuplicateCommand { get; }
        public ReactiveCommand<Unit, Unit> QuitCommand { get; }
        public ReactiveCommand<Unit, Unit> ArrangeTilesCommand { get; }
        public ReactiveCommand<Unit, Unit> GenerateTilesCommand { get; }

        public ObservableCollection<TextureSetFile> TextureFiles { get; }
        public UserPreferences UserPreferences { get; }

        public bool HasValidModel
        {
            get => hasValidModel;
            set
            {
                this.RaiseAndSetIfChanged(ref hasValidModel, value);
            }
        }

        public bool CanExportSelection
        {
            get => canExportSelection;
            set
            {
                this.RaiseAndSetIfChanged(ref canExportSelection, value);
            }
        }

        public object? SelectedItem
        {
            get => selectedItem;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedItem, value);
                var canExport = selectedItem switch
                {
                    TextureSetFile => true,
                    TextureGrid => true,
                    TextureTile => true,
                    _ => false
                };
                CanExportSelection = canExport;
            }
        }

        public float ViewScaleSliderValue
        {
            get => viewScale * 10;
            set
            {
                ViewScale = value / 10;
            }
        }

        public float ViewScale
        {
            get => viewScale;
            set
            {
                this.TryRaiseAndSetIfChanged(ref viewScale, Math.Max(Scales[0], Math.Min(Scales[^1], value)))
                    .AndRaise(nameof(ViewScaleSliderValue))
                    .AndRaise(nameof(CanZoomIn))
                    .AndRaise(nameof(CanZoomOut));
            }
        }

        public TileTextureCollection? SelectedTextureSet
        {
            get => selectedTextureCollection;
            set
            {
                Logger.Verbose("Selection changed");
                var oldTextureCollection = selectedTextureCollection;
                var (_, changed) = this.TryRaiseAndSetIfChanged(ref selectedTextureCollection, value);
                if (!changed)
                {
                    return;
                }

                if (oldTextureCollection != null)
                {
                    oldTextureCollection.ContentsChanged -= OnTextureCollectionChanged;
                    var p = oldTextureCollection.Parent;
                    if (p != null)
                    {
                        p.PropertyChanged -= OnTextureCollectionChanged;
                    }
                }

                if (selectedTextureCollection != null)
                {
                    selectedTextureCollection.ContentsChanged += OnTextureCollectionChanged;
                    var p = selectedTextureCollection.Parent;
                    if (p != null)
                    {
                        p.PropertyChanged += OnTextureCollectionChanged;
                    }
                }

                RefreshPreviewImage();
            }
        }

        void OnTextureCollectionChanged(object? sender, EventArgs e)
        {
            Logger.Verbose("Preview Image Contents Changed");
            RefreshPreviewImage(4);
        }

        void RefreshPreviewImage(int scale = 1)
        {
            var tc = SelectedTextureSet;
            if (tc == null || tc.Parent == null)
            {
                Logger.Verbose("Preview Image null");
                PreviewImage = null;
                return;
            }

            Logger.Verbose("Preview Image change");
            var tg = new TextureCollectionPainter(UserPreferences.Preferences);
            previewBitmap = tg.CreateBitmap(tc, scale, previewBitmap);
            var stream = previewBitmap.Write();

            var avaloniaBitmap = new Bitmap(stream);
            PreviewImage = avaloniaBitmap;
        }

        public Bitmap? PreviewImage
        {
            get => previewImage;
            private set => this.TryRaiseAndSetIfChanged(ref previewImage, value);
        }

        async Task OnInitialized()
        {
            Logger.Verbose("Init called");
            ViewScale = 1;
            await UserPreferences.LoadDefaults(this);
        }

        async Task OnShutDown()
        {
            Logger.Verbose("Shutdown called");
            await UserPreferences.SaveDefaults();
        }

        async Task OnTest()
        {
            HasValidModel = !HasValidModel;
        }
    }
}
