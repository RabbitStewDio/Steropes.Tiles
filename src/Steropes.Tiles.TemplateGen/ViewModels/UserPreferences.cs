using JetBrains.Annotations;
using ReactiveUI;
using Steropes.Tiles.TemplateGen.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Steropes.Tiles.TemplateGen.ViewModels
{
    public class UserPreferences: ReactiveObject
    {
        
        public UserPreferences()
        {
            Preferences = new GeneratorPreferences();
            RecentFiles = new ObservableCollection<RecentFileViewModel>();
        }

        string RecentFilesStorageLocation => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Steropes.Tiles/recentFiles.txt");
        
        public ObservableCollection<RecentFileViewModel> RecentFiles { get; }

        [NotNull] public GeneratorPreferences Preferences
        {
            get;
        }

        public async Task LoadDefaults(MainWindowViewModel m)
        {
            try
            {
                var l = new GeneratorPreferencesLoader(Preferences);
                l.Load();
            }
            catch
            {
                // ignored too
            }

            var path = RecentFilesStorageLocation;
            if (File.Exists(path))
            {
                try
                {
                    var lines = await File.ReadAllLinesAsync(path);
                    foreach (var file in lines)
                    {
                        RecentFiles.Clear();
                        if (File.Exists(file))
                        {
                            RecentFiles.Add(new RecentFileViewModel(m, file));
                        }
                    }
                }
                catch(Exception e)
                {
                    // ignored
                }
            }
        }
        
        public async Task SaveDefaults()
        {
            try
            {
                var s = new GeneratorPreferencesWriter(Preferences);
                await s.SaveAsync();
            }
            catch
            {
                // ignored too
            }

            try
            {
                var r = RecentFiles.Select(e => e.FileName).ToArray();
                var path = RecentFilesStorageLocation;
                GeneratorPreferencesWriter.EnsureParentDirectoryExists(path);
                await File.WriteAllLinesAsync(path, r);
            }
            catch
            {
                // ignored too
            }
        }

        public void AddRecentFile(MainWindowViewModel mainWindowViewModel, string fileName)
        {
            var r = RecentFiles.ToArray();
            foreach (var rf in r)
            {
                if (rf.FileName == fileName)
                {
                    RecentFiles.Remove(rf);
                }
            }
            
            RecentFiles.Insert(0, new RecentFileViewModel(mainWindowViewModel, fileName));
        }
    }
}
