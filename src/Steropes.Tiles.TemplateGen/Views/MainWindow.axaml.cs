using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DynamicData.Binding;
using ReactiveUI;
using Steropes.Tiles.TemplateGen.Bindings;
using Steropes.Tiles.TemplateGen.Models;
using Steropes.Tiles.TemplateGen.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Steropes.Tiles.TemplateGen.Views
{
    public class MainWindow : Window
    {
        /// <summary>
        /// Defines the <see cref="InitializedCommand"/> property.
        /// </summary>
        public static readonly DirectProperty<MainWindow, ICommand?> InitializedCommandProperty =
            AvaloniaProperty.RegisterDirect<MainWindow, ICommand?>(nameof(InitializedCommand),
                                                                   button => button.InitializedCommand, (button, command) => button.InitializedCommand = command, enableDataValidation: true);

        /// <summary>
        /// Defines the <see cref="ShutdownCommand"/> property.
        /// </summary>
        public static readonly DirectProperty<MainWindow, ICommand?> ShutdownCommandProperty =
            AvaloniaProperty.RegisterDirect<MainWindow, ICommand?>(nameof(ShutdownCommand),
                                                                   button => button.ShutdownCommand, (button, command) => button.ShutdownCommand = command, enableDataValidation: true);

        public static readonly DirectProperty<MainWindow, TileTextureCollection?> SelectedTextureCollectionProperty =
            AvaloniaProperty.RegisterDirect<MainWindow, TileTextureCollection?>(nameof(SelectedTextureCollection),
                                                                   button => button.SelectedTextureCollection, (button, command) => button.SelectedTextureCollection = command, enableDataValidation: true);

        public static readonly DirectProperty<MainWindow, TextureSetFile?> SelectedTextureSetProperty =
            AvaloniaProperty.RegisterDirect<MainWindow, TextureSetFile?>(nameof(SelectedTextureSet),
                                                                         button => button.SelectedTextureSet, (button, command) => button.SelectedTextureSet = command, enableDataValidation: true);

        ICommand? initCmd;
        ICommand? shutdownCmd;
        TileTextureCollection? selectedTextureCollection;
        TextureSetFile? selectedTextureSet;
        readonly TreeView structureTree;

        public MainWindow()
        {
            var o = this.BindProperty(x => x.SelectedTextureSet).Select(e => e != null);
            o.Subscribe(e => Console.WriteLine("Changed!"));
            
            SaveCommand = ReactiveCommand.CreateFromTask(SaveFile, o);
            SaveAsCommand = ReactiveCommand.CreateFromTask(SaveFileAs, o);
            OpenCommand = ReactiveCommand.CreateFromTask(OpenFile);

            ExportCommand = ReactiveCommand.CreateFromTask(ExportSelectedTemplateFile, this.BindProperty(e => e.SelectedTextureCollection).Select(e => e != null));
            ExportAllCommand = ReactiveCommand.CreateFromTask(ExportTemplateFiles, o);
            
            this.Opened += OnInitialized;
            this.DataContextChanged += OnDataContext;
            this.Closing += OnClosing;

            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            structureTree = this.FindControl<TreeView>("StructureTree");
            structureTree.WhenPropertyChanged(x => x.SelectedItem).Subscribe(_ => OnTreeSelectionChanged());
        }
        
        void OnTreeSelectionChanged()
        {
            var collection = structureTree.SelectedItem switch
            {
                TextureSetFile f => f.Collections.FirstOrDefault(),
                TileTextureCollection c => c,
                TextureGrid g => g.Parent,
                TextureTile t => t.Parent?.Parent,
                _ => null
            };

            var textureSet = structureTree.SelectedItem switch
            {
                TextureSetFile f => f,
                TileTextureCollection c => c.Parent,
                TextureGrid g => g.Parent?.Parent,
                TextureTile t => t.Parent?.Parent?.Parent,
                _ => null
            };

            Console.WriteLine("Selection changed " + structureTree.SelectedItem + " -> " + collection + " | " + textureSet);
            SelectedTextureCollection = collection;
            SelectedTextureSet = textureSet;
        }

        public TileTextureCollection? SelectedTextureCollection
        {
            get => selectedTextureCollection;
            set
            {
                SetAndRaise(SelectedTextureCollectionProperty, ref selectedTextureCollection, value);
            }
        }

        public TextureSetFile? SelectedTextureSet
        {
            get => selectedTextureSet;
            set
            {
                if (SetAndRaise(SelectedTextureSetProperty, ref selectedTextureSet, value))
                {
                    Console.WriteLine("Change fired!");
                }
            }
        }

        public ICommand? InitializedCommand
        {
            get => initCmd;
            set => SetAndRaise(InitializedCommandProperty, ref initCmd, value);
        }

        public ICommand? ShutdownCommand
        {
            get => shutdownCmd;
            set => SetAndRaise(ShutdownCommandProperty, ref shutdownCmd, value);
        }

        void OnClosing(object? sender, CancelEventArgs e)
        {
            ShutdownCommand?.Execute(null);
        }

        void OnDataContext(object? sender, EventArgs e)
        {
            InitializedCommand?.Execute(null);
        }

        void OnInitialized(object? sender, EventArgs e)
        {
            Console.WriteLine("Init Window: " + DataContext);
            InitializedCommand?.Execute(null);
        }

        public ReactiveCommand<Unit, Unit> SaveCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveAsCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenCommand { get; }
        public ReactiveCommand<Unit, Unit> ExportCommand { get; }
        public ReactiveCommand<Unit, Unit> ExportAllCommand { get; }

        List<FileDialogFilter> GetTileSetFilters()
        {
            return new List<FileDialogFilter>
            {
                new FileDialogFilter
                {
                    Name = "Tile Set Files (.tiles)", Extensions = new List<string> {"tiles"}
                },
                new FileDialogFilter
                {
                    Name = "All files",
                    Extensions = new List<string> {"*"}
                }
            };
        }

        List<FileDialogFilter> GetPngFilters()
        {
            return new List<FileDialogFilter>
            {
                new FileDialogFilter
                {
                    Name = "PNG Files (.png)", Extensions = new List<string> {"png"}
                },
                new FileDialogFilter
                {
                    Name = "All files",
                    Extensions = new List<string> {"*"}
                }
            };
        }


        (string? dir, string? file) FetchInitialFileName()
        {
            var sourcePath = SelectedTextureSet?.SourcePath;
            if (!string.IsNullOrEmpty(sourcePath))
            {
                Console.WriteLine("Using currently selected file " + sourcePath);
                return (Path.GetDirectoryName(sourcePath), Path.GetFileName(sourcePath));
            }
            
            if (DataContext is MainWindowViewModel dc)
            {
                foreach (var x in dc.UserPreferences.RecentFiles)
                {
                    if (!string.IsNullOrEmpty(x.FileName))
                    {
                        Console.WriteLine("Using first valid recent file " + x.FileName);
                        return (Path.GetDirectoryName(x.FileName), Path.GetFileName(x.FileName));
                    }
                }
            }

            Console.WriteLine("Using fallback " + Environment.CurrentDirectory);
            return (Environment.CurrentDirectory, null);
        }

        public async Task OpenFile()
        {
            if (DataContext is not MainWindowViewModel dc)
            {
                return;
            }

            var (dir, filename) = FetchInitialFileName();
            var dialog = new OpenFileDialog()
            {
                Title = "Open file",
                Filters = GetTileSetFilters(),
                AllowMultiple = false,
                InitialFileName = filename,
                Directory = dir
            };

            var selectedFile = await dialog.ShowAsync(this);
            if (selectedFile.Length == 0)
            {
                return;
            }

            var file = selectedFile[0];
            await dc.OpenFile(file);
        }

        async Task ExportTemplateFiles()
        {
            if (DataContext is not MainWindowViewModel dc)
            {
                return;
            }

            var (dir, _) = FetchInitialFileName();
            var dialog = new OpenFolderDialog()
            {
                Title = "Export Templates",
                Directory = dir
            };

            var selectedFile = await dialog.ShowAsync(this);
            if (selectedFile.Length == 0)
            {
                return;
            }

            // await dc.ExportTemplates(selectedFile);
        }

        async Task ExportSelectedTemplateFile()
        {
            if (DataContext is not MainWindowViewModel dc)
            {
                return;
            }

            var (dir, filename) = FetchInitialFileName();
            var dialog = new SaveFileDialog()
            {
                Title = "Save file",
                Filters = GetPngFilters(),
                InitialFileName = filename,
                Directory = dir
            };


            var selectedFile = await dialog.ShowAsync(this);
            if (selectedFile.Length == 0)
            {
                return;
            }

            await dc.ExportTemplate(selectedFile);
        }

        public async Task SaveFile()
        {
            if (DataContext is not MainWindowViewModel dc)
            {
                return;
            }

            var sourcePath = SelectedTextureSet?.SourcePath;
            string? fileName;
            if (!string.IsNullOrEmpty(sourcePath))
            {
                fileName = sourcePath;
            }
            else
            {
                fileName = await ShowSaveFileDialog();
                if (string.IsNullOrEmpty(fileName))
                {
                    return;
                }
            }

            await dc.SaveFile(fileName);
        }

        public async Task SaveFileAs()
        {
            if (DataContext is not MainWindowViewModel dc)
            {
                return;
            }

            var fileName = await ShowSaveFileDialog();
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            await dc.SaveFile(fileName);
        }

        async Task<string?> ShowSaveFileDialog()
        {
            var (dir, filename) = FetchInitialFileName();
            var dialog = new SaveFileDialog()
            {
                Title = "Save file",
                Filters = GetTileSetFilters(),
                InitialFileName = filename,
                Directory = dir
            };

            return await dialog.ShowAsync(this);
        }

        void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
