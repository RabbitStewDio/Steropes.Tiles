using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Steropes.Tiles.TemplateGen.Models;
using Steropes.Tiles.TemplateGen.Models.Prefs;
using Steropes.Tiles.TemplateGen.Models.Rendering;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Steropes.Tiles.TemplateGen.ViewModels
{
    public partial class MainWindowViewModel
    {
        
        void OnAddCollection()
        {
            var textureSet = SelectedItem switch
            {
                TextureSetFile s => s,
                TileTextureCollection c => c.Parent,
                TextureGrid g => g.Parent?.Parent,
                TextureTile t => t.Parent?.Parent?.Parent,
                _ => null
            };

            textureSet?.Collections.Add(new TileTextureCollection()
            {
                Id = "New Collection"
            }.WithTextureGrid(new TextureGrid()
                                                                                      
            {
                Name = "New Texture Grid"
            }));
        }

        void OnAddGrid()
        {
            var textureCollection = SelectedItem switch
            {
                TileTextureCollection c => c,
                TextureGrid g => g.Parent,
                TextureTile t => t.Parent?.Parent,
                _ => null
            };

            textureCollection?.Grids.Add(new TextureGrid()
            {
                Name = "New Texture Grid"
            });
        }

        void OnAddTile()
        {
            var textureGrid = SelectedItem switch
            {
                TextureGrid g => g,
                TextureTile t => t.Parent,
                _ => null
            };

            textureGrid?.Tiles.Add(new TextureTile());
        }

        
        bool HasAnySelection(object? selectedItem) => selectedItem != null;

        bool CanAddGrid(object? selectedItem) => selectedItem switch
        {
            TileTextureCollection => true,
            TextureGrid => true,
            TextureTile => true,
            _ => false
        };

        bool CanAddTile(object? selectedItem) => selectedItem switch
        {
            TextureGrid => true,
            TextureTile => true,
            _ => false
        };

        public Task OpenFile(string fileName)
        {
            try
            {
                var x = TextureSetFileLoader.Read(fileName);
                TextureFiles.Add(x);
                return Task.CompletedTask;
            }
            catch(Exception e)
            {
                return Task.FromException(e);
            }
        }

        public async Task SaveFile(string fileName)
        {
            var textureSet = SelectedItem switch
            {
                TextureSetFile s => s,
                TileTextureCollection c => c.Parent,
                TextureGrid g => g.Parent?.Parent,
                TextureTile t => t.Parent?.Parent?.Parent,
                _ => null
            };
            
            if (textureSet == null)
            {
                return;
            }

            GeneratorPreferencesWriter.EnsureParentDirectoryExists(fileName);
            var doc = TextureSetFileWriter.GenerateXml(textureSet);

            await using var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
            await doc.SaveAsync(stream, default, default);

            textureSet.SourcePath = fileName;
            UserPreferences.AddRecentFile(this, fileName);
        }

        public async Task ExportTemplate(string fileName)
        {
            var textureCollection = SelectedItem switch
            {
                TileTextureCollection c => c,
                TextureGrid g => g.Parent,
                TextureTile t => t.Parent?.Parent,
                _ => null
            };

            if (textureCollection == null)
            {
                return;
            }
            
            GeneratorPreferencesWriter.EnsureParentDirectoryExists(fileName);
            await using var outputStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
            var stream = CreateImage(textureCollection);
            await stream.CopyToAsync(outputStream);

            var sourcePath = Path.GetDirectoryName(textureCollection.Parent?.SourcePath);
            if (sourcePath != null)
            {
                textureCollection.LastExportLocation = Path.GetRelativePath(sourcePath, fileName);
            }
            else
            {
                textureCollection.LastExportLocation = fileName;
            }
        }

        MemoryStream CreateImage(TileTextureCollection tc)
        {
            var tg = new TextureCollectionPainter(UserPreferences.Preferences);
            var exportBitmap = tg.CreateBitmap(tc);
            var stream = exportBitmap.Write();
            return stream;
        }


        public void OnCreateNew()
        {
            this.TextureFiles.Clear();
            this.TextureFiles.Add(new TextureSetFile().WithTextureCollection(new TileTextureCollection().WithTextureGrid(new TextureGrid())));
        }

        void OnQuit()
        {
            AvaloniaLocator.Current.GetService<IControlledApplicationLifetime>()?.Shutdown();
        }

        void OnZoomIn()
        {
            var bs = Array.BinarySearch(Scales, ViewScale);
            if (bs >= 0)
            {
                ViewScale = Scales[Math.Min(Scales.Length - 1, bs + 1)];
            }
            else
            {
                var nextLargerIndex = ~bs;
                ViewScale = Scales[Math.Min(Scales.Length - 1, nextLargerIndex)];
            }
        }

        void OnZoomOut()
        {
            var bs = Array.BinarySearch(Scales, ViewScale);
            if (bs >= 0)
            {
                ViewScale = Scales[Math.Max(0, bs - 1)];
            }
            else
            {
                var nextLargerIndex = ~bs;
                ViewScale = Scales[Math.Max(0, nextLargerIndex - 1)];
            }
        }

        public bool CanZoomIn
        {
            get
            {
                var bs = Array.BinarySearch(Scales, ViewScale);

                if (bs < 0)
                {
                    var nextLargest = ~bs;
                    return nextLargest != Scales.Length;
                }

                if (bs == Scales.Length - 1)
                {
                    return false;
                }

                return true;
            }
        }

        public bool CanZoomOut
        {
            get
            {
                var bs = Array.BinarySearch(Scales, ViewScale);
                if (bs < 0)
                {
                    var nextLargest = ~bs;
                    return nextLargest != 0;
                }

                if (bs == 0)
                {
                    return false;
                }

                return true;
            }
        }
    }
}
