using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Steropes.Tiles.TemplateGen.Models
{
    public class GeneratorPreferencesWriter
    {
        static readonly XNamespace Namespace = "http://www.steropes-ui.org/namespaces/tiles-preferences/1.0";
        readonly GeneratorPreferences preferences;

        public GeneratorPreferencesWriter(GeneratorPreferences preferences)
        {
            this.preferences = preferences ?? throw new ArgumentNullException(nameof(preferences));
        }

        public void Save()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Steropes.Tiles/preferences.xml");
            EnsureParentDirectoryExists(path);
            Save(path);
        }

        public void Save(string path)
        {
            Create().Save(path);
        }

        public async Task SaveAsync(string? path = null)
        {
            path ??= Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Steropes.Tiles/preferences.xml");
            EnsureParentDirectoryExists(path);

            var doc = Create();

            await using var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
            await doc.SaveAsync(stream, default, default);
        }

        XDocument Create()
        {
            var root = new XElement(Namespace + "preferences");
            root.Add(CreateRecentFilesList());
            root.Add(CreateTileColorList());
            root.Add(CreateDefaultTileSettings());
            root.Add(CreateFormattingSettings());
            return new XDocument(root);
        }

        XElement CreateRecentFilesList()
        {
            var e = new XElement(Namespace + "recent-files");
            e.AddRange(preferences.RecentFiles
                                  .Where(f => !string.IsNullOrWhiteSpace(f))
                                  .Select(recentFile => new XElement(Namespace + "file", recentFile)));
            return e;
        }

        XElement CreateTileColorList()
        {
            var e = new XElement(Namespace + "tile-colors");
            e.AddRange(preferences.TileColors.Select(c => new XElement(Namespace + "color", c.AsText())));
            return e;
        }

        XElement CreateDefaultTileSettings()
        {
            var e = new XElement(Namespace + "tile-settings");

            e.Add(new XAttribute("width", preferences.DefaultWidth));
            e.Add(new XAttribute("height", preferences.DefaultHeight));
            e.Add(new XAttribute("tile-type", preferences.DefaultTileType));

            return e;
        }

        XElement CreateFormattingSettings()
        {
            var e = new XElement(Namespace + "formatting-settings");

            e.Add(new XAttribute("text-color", preferences.DefaultTextColor.AsText()));
            e.Add(new XAttribute("tile-color", preferences.DefaultTileColor.AsText()));
            e.Add(new XAttribute("highlight-color", preferences.DefaultTileHighlightColor.AsText()));
            e.Add(new XAttribute("anchor-color", preferences.DefaultTileAnchorColor.AsText()));
            e.Add(new XAttribute("border-color", preferences.DefaultBorderColor.AsText()));
            var bg = preferences.DefaultBackgroundColor?.AsText();
            if (bg != null)
            {
                e.Add(new XAttribute("background-color", bg));
            }

            e.Add(new XAttribute("text-spacing", preferences.TextSpacing));

            var tf = preferences.DefaultFont.Typeface;
            if (tf != null)
            {
                e.Add(new XAttribute("font-face", tf));
            }

            e.Add(new XAttribute("font-size", preferences.DefaultFont.Size));
            e.Add(new XAttribute("font-style", preferences.DefaultFont.Italic ? "Italic" : "Normal"));
            e.Add(new XAttribute("font-weight", preferences.DefaultFont.Bold ? "Bold" : "Normal"));

            return e;
        }

        public static void EnsureParentDirectoryExists(string path)
        {
            var parent = Path.GetDirectoryName(path);
            if (parent != null)
            {
                Directory.CreateDirectory(parent);
            }
        }
    }
}
